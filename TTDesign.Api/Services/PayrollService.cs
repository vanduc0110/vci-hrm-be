using ClosedXML.Excel;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class PayrollService : GenericService<Payroll>, IPayrollService
  {
    private readonly IPayrollRepository _payrollRepository;
    private readonly IPayrollDetailRepository _payrollDetailRepository;
    private readonly ISalaryRepository _salaryRepository;
    private readonly IBonusRepository _bonusRepository;
    private readonly ITaxBracketRepository _taxBracketRepository;
    private readonly ISocialInsuranceRateRepository _rateRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserInfoRepository _userInfoRepository;
    private readonly ITimesheetRepository _timesheetRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<PayrollService> _logger;

    public PayrollService(
      IPayrollRepository payrollRepository,
      IPayrollDetailRepository payrollDetailRepository,
      ISalaryRepository salaryRepository,
      IBonusRepository bonusRepository,
      ITaxBracketRepository taxBracketRepository,
      ISocialInsuranceRateRepository rateRepository,
      IUserRepository userRepository,
      IUserInfoRepository userInfoRepository,
      ITimesheetRepository timesheetRepository,
      INotificationService notificationService,
      ILogger<PayrollService> logger ) : base( payrollRepository )
    {
      _payrollRepository = payrollRepository;
      _payrollDetailRepository = payrollDetailRepository;
      _salaryRepository = salaryRepository;
      _bonusRepository = bonusRepository;
      _taxBracketRepository = taxBracketRepository;
      _rateRepository = rateRepository;
      _userRepository = userRepository;
      _userInfoRepository = userInfoRepository;
      _timesheetRepository = timesheetRepository;
      _notificationService = notificationService;
      _logger = logger;
    }

    public async Task<IEnumerable<PayrollResponse>> GetList( int month, int year, long[]? allowedUserIds = null )
    {
      var payrolls = await _payrollRepository.GetListByMonthYear( month, year );
      if ( allowedUserIds != null && allowedUserIds.Length > 0 )
        payrolls = payrolls.Where( p => allowedUserIds.Contains( p.UserId ) );
      return payrolls.Select( p => MapToResponse( p ) );
    }

    public async Task<PayrollResponse?> GetDetail( long id )
    {
      var payroll = await _payrollRepository.GetWithDetails( id );
      if ( payroll == null ) return null;
      return MapToResponse( payroll );
    }

    public async Task Calculate( PayrollCalculateRequest request, long creator )
    {
      IEnumerable<long> userIds;
      if ( request.UserId.HasValue ) {
        userIds = new [] { request.UserId.Value };
      }
      else {
        var users = await _userRepository.GetListByCondition( u => u.IsActive );
        userIds = users.Select( u => u.Id );
      }

      var rate = await _rateRepository.GetByConditionNoTrack( r => r.IsActive );
      var taxBrackets = ( await _taxBracketRepository.GetListByCondition( t => t.Year == request.Year && t.IsActive ) )
        .OrderBy( t => t.FromAmount ).ToList();

      var daysInMonth = DateTime.DaysInMonth( request.Year, request.Month );
      var standardWorkDays = CountStandardWorkDays( request.Year, request.Month );
      var standardHoursPerDay = 8.0;
      var standardHours = standardWorkDays * standardHoursPerDay;

      foreach ( var userId in userIds ) {
        try {
          await CalculateForUser( request, userId, rate, taxBrackets, standardWorkDays, standardHours, creator );
        }
        catch ( Exception ex ) {
          _logger.LogError( ex, "Lỗi tính lương user {UserId} tháng {Month}/{Year}", userId, request.Month, request.Year );
        }
      }
    }

    private async Task CalculateForUser( PayrollCalculateRequest request, long userId,
      SocialInsuranceRate? rate, List<TaxBracket> taxBrackets,
      double standardWorkDays, double standardHours, long creator )
    {
      var salaryConfig = await _salaryRepository.GetByConditionNoTrack( s => s.UserId == userId && s.IsActive && s.ApprovedBy != null );
      if ( salaryConfig == null ) return;

      var user = await _userRepository.GetByConditionNoTrack( u => u.Id == userId );
      if ( user == null ) return;

      var timesheets = await _timesheetRepository.GetTimesheets(
        t => t.UserId == userId &&
             t.Date.Year == request.Year &&
             t.Date.Month == request.Month );

      double actualWorkHours = 0;
      double overtimeHours = 0;
      decimal overtimeSalary = 0;

      var hourlyRate = standardHours > 0 ? salaryConfig.BasicSalary / (decimal)standardHours : 0;

      foreach ( var ts in timesheets ) {
        if ( ts.FingerPrinter == null ) continue;
        var total = ts.FingerPrinter.HourTotal;
        var workHours = Math.Min( total, 8.0 );
        var otHours = Math.Max( 0, total - 8.0 );
        actualWorkHours += workHours;
        overtimeHours += otHours;

        if ( otHours > 0 ) {
          bool isHoliday = !string.IsNullOrEmpty( ts.HolidayName );
          bool isWeekend = ts.Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
          var multiplier = isHoliday ? 3.0m : ( isWeekend ? 2.0m : 1.5m );
          overtimeSalary += hourlyRate * multiplier * (decimal)otHours;
        }
      }

      var bonusTotal = ( await _bonusRepository.GetListByCondition(
        b => b.UserId == userId && b.Month == request.Month && b.Year == request.Year && b.ApprovedBy != null ) )
        .Sum( b => b.Amount );

      var workRatio = standardHours > 0 ? (decimal)actualWorkHours / (decimal)standardHours : 0;
      var earnedBasic = salaryConfig.BasicSalary * workRatio;
      var totalAllowance = salaryConfig.AllowanceHousing + salaryConfig.AllowanceTransport +
                           salaryConfig.AllowanceFood + salaryConfig.AllowanceOther;
      var grossSalary = earnedBasic + totalAllowance + overtimeSalary + bonusTotal;

      decimal socialInsurance = 0, healthInsurance = 0, unemploymentInsurance = 0;
      if ( rate != null ) {
        var insBase = salaryConfig.BasicSalary;
        socialInsurance = Math.Round( insBase * rate.SocialInsuranceEmployee / 100, 0 );
        healthInsurance = Math.Round( insBase * rate.HealthInsuranceEmployee / 100, 0 );
        unemploymentInsurance = Math.Round( insBase * rate.UnemploymentInsuranceEmployee / 100, 0 );
      }

      var dependent = 0;
      if ( user != null ) {
        var userInfo = await GetUserInfo( userId );
        dependent = userInfo?.Dependent ?? 0;
      }

      var personalDeduction = rate?.PersonalDeduction ?? 11_000_000m;
      var dependentDeduction = ( rate?.DependentDeduction ?? 4_400_000m ) * dependent;
      var totalInsurance = socialInsurance + healthInsurance + unemploymentInsurance;
      var taxableIncome = grossSalary - totalInsurance - personalDeduction - dependentDeduction;
      var incomeTax = taxableIncome > 0 ? CalculateIncomeTax( taxableIncome, taxBrackets ) : 0;

      var totalDeduction = totalInsurance + incomeTax;
      var netSalary = grossSalary - totalDeduction;

      // remove existing draft payroll for same month/year/user
      var existing = await _payrollRepository.GetByCondition(
        p => p.UserId == userId && p.Month == request.Month && p.Year == request.Year );
      if ( existing != null ) {
        if ( existing.Status != (int)Enums.PayrollStatus.Draft && existing.Status != (int)Enums.PayrollStatus.Rejected )
          return; // không ghi đè payroll đã qua bước xác nhận/duyệt
        await _payrollDetailRepository.DeleteByCondition( d => d.PayrollId == existing.Id );
        _payrollRepository.Delete( existing );
      }

      var payroll = new Payroll
      {
        UserId = userId,
        Month = request.Month,
        Year = request.Year,
        BasicSalary = salaryConfig.BasicSalary,
        TotalAllowance = totalAllowance,
        OvertimeSalary = overtimeSalary,
        Bonus = bonusTotal,
        GrossSalary = grossSalary,
        SocialInsurance = socialInsurance,
        HealthInsurance = healthInsurance,
        UnemploymentInsurance = unemploymentInsurance,
        IncomeTax = incomeTax,
        TotalDeduction = totalDeduction,
        NetSalary = netSalary,
        StandardWorkDays = standardWorkDays,
        ActualWorkHours = actualWorkHours,
        OvertimeHours = overtimeHours,
        Status = (int)Enums.PayrollStatus.Draft,
        CreatedBy = creator,
        CreatedDate = DateTime.UtcNow,
      };
      await _payrollRepository.CreateAsync( payroll );

      var details = new List<PayrollDetail>();
      if ( socialInsurance > 0 )
        details.Add( new PayrollDetail { PayrollId = payroll.Id, Description = "BHXH (8%)", DeductionType = (int)Enums.DeductionType.SocialInsurance, Amount = socialInsurance, CreatedBy = creator, CreatedDate = DateTime.UtcNow } );
      if ( healthInsurance > 0 )
        details.Add( new PayrollDetail { PayrollId = payroll.Id, Description = "BHYT (1.5%)", DeductionType = (int)Enums.DeductionType.HealthInsurance, Amount = healthInsurance, CreatedBy = creator, CreatedDate = DateTime.UtcNow } );
      if ( unemploymentInsurance > 0 )
        details.Add( new PayrollDetail { PayrollId = payroll.Id, Description = "BHTN (1%)", DeductionType = (int)Enums.DeductionType.UnemploymentInsurance, Amount = unemploymentInsurance, CreatedBy = creator, CreatedDate = DateTime.UtcNow } );
      if ( incomeTax > 0 )
        details.Add( new PayrollDetail { PayrollId = payroll.Id, Description = "Thuế TNCN", DeductionType = (int)Enums.DeductionType.IncomeTax, Amount = incomeTax, CreatedBy = creator, CreatedDate = DateTime.UtcNow } );

      if ( details.Count > 0 )
        await _payrollDetailRepository.CreatesAsync( details );
    }

    public async Task Update( PayrollUpdateRequest request, long editor )
    {
      var payroll = await _payrollRepository.GetByCondition( p => p.Id == request.Id );
      if ( payroll == null ) throw new Exception( "Không tìm thấy bảng lương" );
      if ( payroll.Status != (int)Enums.PayrollStatus.Draft )
        throw new Exception( "Chỉ được cập nhật bảng lương ở trạng thái Draft" );

      if ( request.Bonus.HasValue ) {
        payroll.Bonus = request.Bonus.Value;
        payroll.GrossSalary = payroll.BasicSalary + payroll.TotalAllowance + payroll.OvertimeSalary + payroll.Bonus;
        payroll.NetSalary = payroll.GrossSalary - payroll.TotalDeduction;
      }
      if ( request.Notes != null ) payroll.Notes = request.Notes;
      payroll.ModifiedBy = editor;
      payroll.ModifiedDate = DateTime.UtcNow;
      _payrollRepository.Update( payroll );
    }

    public async Task LeadConfirm( long id, long confirmedBy )
    {
      var payroll = await _payrollRepository.GetByCondition( p => p.Id == id );
      if ( payroll == null ) throw new Exception( "Không tìm thấy bảng lương" );
      if ( payroll.Status != (int)Enums.PayrollStatus.Draft )
        throw new Exception( "Chỉ xác nhận được bảng lương ở trạng thái Nháp" );

      payroll.Status = (int)Enums.PayrollStatus.LeadConfirmed;
      payroll.ApprovedBy = confirmedBy;
      payroll.ApprovedDate = DateTime.UtcNow;
      payroll.ModifiedBy = confirmedBy;
      payroll.ModifiedDate = DateTime.UtcNow;
      _payrollRepository.Update( payroll );

      try {
        var hrUsers = await _userRepository.GetListByCondition( u => u.IsActive && u.TeamUsers.Any( t => t.TeamId == Enums.TEAM_HR ) );
        var hrUserIds = string.Join( ",", hrUsers.Select( u => u.Id ) );
        if ( !string.IsNullOrEmpty( hrUserIds ) ) {
          var user = await _userRepository.GetByConditionNoTrack( u => u.Id == confirmedBy );
          var userName = user?.FullName ?? "Không rõ";
          await _notificationService.Create( 0, new Dictionary<string, string>
          {
            { "Title", "Bảng lương đã được Lead xác nhận" },
            { "Content", $"{userName} đã xác nhận bảng lương tháng {payroll.Month}/{payroll.Year}" },
            { "ObjectId", id.ToString() },
            { "CreatedBy", confirmedBy.ToString() },
            { "To", hrUserIds },
          } );
        }
      }
      catch ( Exception ex ) {
        _logger.LogError( ex, "Lỗi gửi notification khi Lead xác nhận bảng lương {Id}", id );
      }
    }

    public async Task HRApprove( long id, long approvedBy )
    {
      var payroll = await _payrollRepository.GetByCondition( p => p.Id == id );
      if ( payroll == null ) throw new Exception( "Không tìm thấy bảng lương" );
      if ( payroll.Status != (int)Enums.PayrollStatus.LeadConfirmed )
        throw new Exception( "HR chỉ chốt được bảng lương sau khi Lead đã xác nhận" );

      payroll.Status = (int)Enums.PayrollStatus.HRApproved;
      payroll.ModifiedBy = approvedBy;
      payroll.ModifiedDate = DateTime.UtcNow;
      _payrollRepository.Update( payroll );

      try {
        var admins = await _userRepository.GetListByCondition( u => u.IsActive &&
          ( u.Position == (int)Enums.UserPosition.System || u.Position == (int)Enums.UserPosition.Director ) );
        var adminIds = string.Join( ",", admins.Select( u => u.Id ) );
        if ( !string.IsNullOrEmpty( adminIds ) ) {
          var user = await _userRepository.GetByConditionNoTrack( u => u.Id == approvedBy );
          var userName = user?.FullName ?? "HR";
          await _notificationService.Create( 0, new Dictionary<string, string>
          {
            { "Title", "Bảng lương chờ Giám đốc duyệt ngân sách" },
            { "Content", $"{userName} đã chốt bảng lương tháng {payroll.Month}/{payroll.Year}, cần Giám đốc phê duyệt ngân sách." },
            { "ObjectId", id.ToString() },
            { "CreatedBy", approvedBy.ToString() },
            { "To", adminIds },
          } );
        }
      }
      catch ( Exception ex ) {
        _logger.LogError( ex, "Lỗi gửi notification khi HR chốt lương {Id}", id );
      }
    }

    public async Task DirectorApprove( long id, long approvedBy )
    {
      var payroll = await _payrollRepository.GetByCondition( p => p.Id == id );
      if ( payroll == null ) throw new Exception( "Không tìm thấy bảng lương" );
      if ( payroll.Status != (int)Enums.PayrollStatus.HRApproved )
        throw new Exception( "Giám đốc chỉ duyệt được bảng lương sau khi HR đã chốt" );

      payroll.Status = (int)Enums.PayrollStatus.DirectorApproved;
      payroll.ModifiedBy = approvedBy;
      payroll.ModifiedDate = DateTime.UtcNow;
      _payrollRepository.Update( payroll );

      try {
        var hrUsers = await _userRepository.GetListByCondition( u => u.IsActive && u.TeamUsers.Any( t => t.TeamId == Enums.TEAM_HR ) );
        var hrUserIds = string.Join( ",", hrUsers.Select( u => u.Id ) );
        if ( !string.IsNullOrEmpty( hrUserIds ) ) {
          var user = await _userRepository.GetByConditionNoTrack( u => u.Id == approvedBy );
          var userName = user?.FullName ?? "Không rõ";
          await _notificationService.Create( 0, new Dictionary<string, string>
          {
            { "Title", "Bảng lương đã được Giám đốc duyệt" },
            { "Content", $"{userName} đã duyệt ngân sách bảng lương tháng {payroll.Month}/{payroll.Year}" },
            { "ObjectId", id.ToString() },
            { "CreatedBy", approvedBy.ToString() },
            { "To", hrUserIds },
          } );
        }
      }
      catch ( Exception ex ) {
        _logger.LogError( ex, "Lỗi gửi notification khi Giám đốc duyệt bảng lương {Id}", id );
      }
    }

    public async Task MarkPaid( long id, long editor )
    {
      var payroll = await _payrollRepository.GetByCondition( p => p.Id == id );
      if ( payroll == null ) throw new Exception( "Không tìm thấy bảng lương" );
      if ( payroll.Status != (int)Enums.PayrollStatus.DirectorApproved )
        throw new Exception( "Chỉ thanh toán được bảng lương sau khi Giám đốc đã duyệt" );

      payroll.Status = (int)Enums.PayrollStatus.Paid;
      payroll.PaidDate = DateTime.UtcNow;
      payroll.ModifiedBy = editor;
      payroll.ModifiedDate = DateTime.UtcNow;
      _payrollRepository.Update( payroll );

      try {
        await _notificationService.Create( 0, new Dictionary<string, string>
        {
          { "Title", "Lương đã được thanh toán" },
          { "Content", $"Lương tháng {payroll.Month}/{payroll.Year} của bạn đã được chuyển khoản thành công." },
          { "ObjectId", id.ToString() },
          { "CreatedBy", editor.ToString() },
          { "To", payroll.UserId.ToString() },
        } );
      }
      catch ( Exception ex ) {
        _logger.LogError( ex, "Lỗi gửi notification khi đánh dấu đã trả lương {Id}", id );
      }
    }

    public async Task Cancel( long id, long editor )
    {
      var payroll = await _payrollRepository.GetByCondition( p => p.Id == id );
      if ( payroll == null ) throw new Exception( "Không tìm thấy bảng lương" );
      if ( payroll.Status == (int)Enums.PayrollStatus.Paid )
        throw new Exception( "Không thể hủy bảng lương đã thanh toán" );

      payroll.Status = (int)Enums.PayrollStatus.Canceled;
      payroll.ModifiedBy = editor;
      payroll.ModifiedDate = DateTime.UtcNow;
      _payrollRepository.Update( payroll );
    }

    public async Task Reject( long id, string reason, long rejectedBy )
    {
      var payroll = await _payrollRepository.GetByCondition( p => p.Id == id );
      if ( payroll == null ) throw new Exception( "Không tìm thấy bảng lương" );
      if ( payroll.Status != (int)Enums.PayrollStatus.Draft && payroll.Status != (int)Enums.PayrollStatus.HRApproved )
        throw new Exception( "Chỉ có thể từ chối bảng lương ở trạng thái Nháp hoặc HR đã chốt" );

      payroll.Status = (int)Enums.PayrollStatus.Rejected;
      payroll.Notes = reason;
      payroll.ModifiedBy = rejectedBy;
      payroll.ModifiedDate = DateTime.UtcNow;
      _payrollRepository.Update( payroll );

      // Gửi thông báo cho tất cả nhân viên HR
      try {
        var hrUsers = await _userRepository.GetListByCondition( u => u.IsActive && u.TeamUsers.Any( t => t.TeamId == Enums.TEAM_HR ) );
        var hrUserIds = string.Join( ",", hrUsers.Select( u => u.Id ) );
        if ( !string.IsNullOrEmpty( hrUserIds ) ) {
          var user = await _userRepository.GetByConditionNoTrack( u => u.Id == rejectedBy );
          var userName = user?.FullName ?? "Không rõ";
          await _notificationService.Create( 0, new Dictionary<string, string>
          {
            { "Title", "Bảng lương bị từ chối" },
            { "Content", $"{userName} đã từ chối bảng lương tháng {payroll.Month}/{payroll.Year}. Lý do: {reason}" },
            { "ObjectId", id.ToString() },
            { "CreatedBy", rejectedBy.ToString() },
            { "To", hrUserIds },
          } );
        }
      }
      catch ( Exception ex ) {
        _logger.LogError( ex, "Lỗi gửi notification khi từ chối bảng lương {Id}", id );
      }
    }

    public async Task<int> GetTotalPending( int month, int year, int position, long[] teamIds, bool hasPayrollFull )
    {
      var payrolls = ( await _payrollRepository.GetListByMonthYear( month, year ) ).ToList();
      if ( Common.ValidRoleAdmin( position ) ) {
        // System/Director: HRApproved payrolls + salary configs chờ duyệt + bonuses chờ duyệt
        var hrApprovedCount   = payrolls.Count( p => p.Status == (int)Enums.PayrollStatus.HRApproved );
        var pendingSalaries   = ( await _salaryRepository.GetListByCondition( s => s.IsActive && s.ApprovedBy == null ) ).Count();
        var pendingBonuses    = ( await _bonusRepository.GetListByCondition( b => b.ApprovedBy == null ) ).Count();
        return hrApprovedCount + pendingSalaries + pendingBonuses;
      }
      if ( hasPayrollFull || teamIds.Contains( Enums.TEAM_HR ) ) {
        // HR: LeadConfirmed payrolls chờ chốt lương
        return payrolls.Count( p => p.Status == (int)Enums.PayrollStatus.LeadConfirmed );
      }
      // Lead/SubLead/PM: Draft payrolls trong team của họ chờ xác nhận
      var userIds = new HashSet<long>();
      foreach ( var teamId in teamIds ) {
        var members = await _userRepository.GetListByCondition( u => u.IsActive && u.TeamUsers.Any( t => t.TeamId == teamId ) );
        foreach ( var m in members ) userIds.Add( m.Id );
      }
      return payrolls.Count( p => p.Status == (int)Enums.PayrollStatus.Draft && userIds.Contains( p.UserId ) );
    }

    public async Task<byte[]?> Export( int month, int year )
    {
      var payrolls = await _payrollRepository.GetListByMonthYear( month, year );
      if ( !payrolls.Any() ) return null;

      using var wb = new XLWorkbook();
      var ws = wb.Worksheets.Add( $"Lương {month:D2}/{year}" );

      var headers = new [] {
        "STT","Mã NV","Họ tên","Ngân hàng","Số TK",
        "Lương CB","Phụ cấp","Lương OT","Thưởng","Gross",
        "BHXH","BHYT","BHTN","Thuế TNCN","Tổng KT","Thực lĩnh","Trạng thái"
      };
      for ( int i = 0; i < headers.Length; i++ )
        ws.Cell( 1, i + 1 ).Value = headers [ i ];

      var row = 2;
      foreach ( var p in payrolls.OrderBy( p => p.User?.FullName ) ) {
        ws.Cell( row, 1 ).Value = row - 1;
        ws.Cell( row, 2 ).Value = p.User?.StaffId ?? string.Empty;
        ws.Cell( row, 3 ).Value = p.User?.FullName ?? string.Empty;
        ws.Cell( row, 4 ).Value = string.Empty;
        ws.Cell( row, 5 ).Value = string.Empty;
        ws.Cell( row, 6 ).Value = (double)p.BasicSalary;
        ws.Cell( row, 7 ).Value = (double)p.TotalAllowance;
        ws.Cell( row, 8 ).Value = (double)p.OvertimeSalary;
        ws.Cell( row, 9 ).Value = (double)p.Bonus;
        ws.Cell( row, 10 ).Value = (double)p.GrossSalary;
        ws.Cell( row, 11 ).Value = (double)p.SocialInsurance;
        ws.Cell( row, 12 ).Value = (double)p.HealthInsurance;
        ws.Cell( row, 13 ).Value = (double)p.UnemploymentInsurance;
        ws.Cell( row, 14 ).Value = (double)p.IncomeTax;
        ws.Cell( row, 15 ).Value = (double)p.TotalDeduction;
        ws.Cell( row, 16 ).Value = (double)p.NetSalary;
        ws.Cell( row, 17 ).Value = Enum.GetName( typeof( Enums.PayrollStatus ), p.Status ) ?? string.Empty;
        row++;
      }

      ws.Columns().AdjustToContents();

      using var ms = new MemoryStream();
      wb.SaveAs( ms );
      return ms.ToArray();
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private static PayrollResponse MapToResponse( Payroll p, UserInfo? userInfo = null )
    {
      return new PayrollResponse
      {
        Id = p.Id,
        UserId = p.UserId,
        FullName = p.User?.FullName ?? string.Empty,
        StaffId = p.User?.StaffId ?? string.Empty,
        BankName = userInfo?.BankName,
        AccountBank = userInfo?.AccountBank,
        Month = p.Month,
        Year = p.Year,
        BasicSalary = p.BasicSalary,
        TotalAllowance = p.TotalAllowance,
        OvertimeSalary = p.OvertimeSalary,
        Bonus = p.Bonus,
        GrossSalary = p.GrossSalary,
        SocialInsurance = p.SocialInsurance,
        HealthInsurance = p.HealthInsurance,
        UnemploymentInsurance = p.UnemploymentInsurance,
        IncomeTax = p.IncomeTax,
        TotalDeduction = p.TotalDeduction,
        NetSalary = p.NetSalary,
        StandardWorkDays = p.StandardWorkDays,
        ActualWorkHours = p.ActualWorkHours,
        OvertimeHours = p.OvertimeHours,
        Status = Enum.GetName( typeof( Enums.PayrollStatus ), p.Status ) ?? p.Status.ToString(),
        ApprovedBy = p.ApprovedBy,
        ApprovedDate = p.ApprovedDate,
        PaidDate = p.PaidDate,
        Notes = p.Notes,
        Details = p.Details.Select( d => new PayrollDetailResponse
        {
          Id = d.Id,
          Description = d.Description,
          DeductionType = Enum.GetName( typeof( Enums.DeductionType ), d.DeductionType ) ?? d.DeductionType.ToString(),
          Amount = d.Amount,
          Notes = d.Notes,
        } ).ToList(),
      };
    }

    private static decimal CalculateIncomeTax( decimal taxableIncome, List<TaxBracket> brackets )
    {
      // Progressive tax: tax = taxableIncome × rate − quickDeduction
      var bracket = brackets.LastOrDefault( b => taxableIncome >= b.FromAmount );
      if ( bracket == null ) return 0;
      return Math.Max( 0, Math.Round( taxableIncome * bracket.TaxRate / 100 - bracket.QuickDeduction, 0 ) );
    }

    private static double CountStandardWorkDays( int year, int month )
    {
      var daysInMonth = DateTime.DaysInMonth( year, month );
      double count = 0;
      for ( int d = 1; d <= daysInMonth; d++ ) {
        var dow = new DateTime( year, month, d ).DayOfWeek;
        if ( dow != DayOfWeek.Saturday && dow != DayOfWeek.Sunday )
          count++;
      }
      return count;
    }

    private async Task<UserInfo?> GetUserInfo( long userId )
    {
      return await _userInfoRepository.GetUserByCondition( u => u.UserId == userId );
    }
  }
}
