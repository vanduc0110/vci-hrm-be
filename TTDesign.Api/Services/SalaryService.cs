using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class SalaryService : GenericService<Salary>, ISalaryService
  {
    private readonly ISalaryRepository _salaryRepository;
    private readonly IUserRepository _userRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<SalaryService> _logger;

    public SalaryService( ISalaryRepository salaryRepository, IUserRepository userRepository,
      INotificationService notificationService, ILogger<SalaryService> logger ) : base( salaryRepository )
    {
      _salaryRepository = salaryRepository;
      _userRepository = userRepository;
      _notificationService = notificationService;
      _logger = logger;
    }

    public async Task<IEnumerable<SalaryResponse>> GetList()
    {
      return await _salaryRepository.GetActiveSalaries();
    }

    public async Task<SalaryResponse?> GetByUser( long userId )
    {
      return await _salaryRepository.GetActiveByUser( userId );
    }

    public async Task<long> Create( SalaryResource resource, long creator )
    {
      var existing = await _salaryRepository.GetByCondition( s => s.UserId == resource.UserId && s.IsActive );
      if ( existing != null ) throw new Exception( "Nhân viên này đã có cấu hình lương. Vui lòng sử dụng chức năng cập nhật." );

      var salary = new Salary
      {
        UserId = resource.UserId,
        BasicSalary = resource.BasicSalary,
        AllowanceHousing = resource.AllowanceHousing,
        AllowanceTransport = resource.AllowanceTransport,
        AllowanceFood = resource.AllowanceFood,
        AllowanceOther = resource.AllowanceOther,
        EffectiveDate = resource.EffectiveDate,
        Notes = resource.Notes,
        IsActive = true,
        CreatedBy = creator,
        CreatedDate = DateTime.UtcNow,
      };
      await _salaryRepository.CreateAsync( salary );

      try {
        var admins = await _userRepository.GetListByCondition( u => u.IsActive &&
          ( u.Position == (int)Enums.UserPosition.System || u.Position == (int)Enums.UserPosition.Director ) );
        var adminIds = string.Join( ",", admins.Select( u => u.Id ) );
        if ( !string.IsNullOrEmpty( adminIds ) ) {
          var creatorUser = await _userRepository.GetByConditionNoTrack( u => u.Id == creator );
          var employee = await _userRepository.GetByConditionNoTrack( u => u.Id == resource.UserId );
          await _notificationService.Create( 0, new Dictionary<string, string>
          {
            { "Title", "Cấu hình lương chờ phê duyệt" },
            { "Content", $"{creatorUser?.FullName ?? "HR"} vừa tạo cấu hình lương cho {employee?.FullName ?? "nhân viên"}, cần phê duyệt trước khi tính lương." },
            { "ObjectId", salary.Id.ToString() },
            { "CreatedBy", creator.ToString() },
            { "UserName", creatorUser?.FullName ?? "" },
            { "To", adminIds },
          } );
        }
      }
      catch ( Exception ex ) {
        _logger.LogError( ex, "Lỗi gửi notification khi tạo cấu hình lương {Id}", salary.Id );
      }

      return salary.Id;
    }

    public async Task Update( SalaryResource resource, long editor )
    {
      var salary = await _salaryRepository.GetByCondition( s => s.Id == resource.Id );
      if ( salary == null ) throw new Exception( "Không tìm thấy bảng lương" );

      salary.BasicSalary = resource.BasicSalary;
      salary.AllowanceHousing = resource.AllowanceHousing;
      salary.AllowanceTransport = resource.AllowanceTransport;
      salary.AllowanceFood = resource.AllowanceFood;
      salary.AllowanceOther = resource.AllowanceOther;
      salary.EffectiveDate = resource.EffectiveDate;
      salary.Notes = resource.Notes;
      // Khi HR sửa cấu hình lương → reset về chờ duyệt
      salary.ApprovedBy = null;
      salary.ApprovedDate = null;
      salary.ModifiedBy = editor;
      salary.ModifiedDate = DateTime.UtcNow;
      _salaryRepository.Update( salary );
    }

    public async Task Approve( long id, long approvedBy )
    {
      var salary = await _salaryRepository.GetByCondition( s => s.Id == id && s.IsActive );
      if ( salary == null ) throw new Exception( "Không tìm thấy cấu hình lương" );
      if ( salary.ApprovedBy.HasValue ) throw new Exception( "Cấu hình lương này đã được duyệt" );
      salary.ApprovedBy = approvedBy;
      salary.ApprovedDate = DateTime.UtcNow;
      salary.ModifiedBy = approvedBy;
      salary.ModifiedDate = DateTime.UtcNow;
      _salaryRepository.Update( salary );

      try {
        var approver = await _userRepository.GetByConditionNoTrack( u => u.Id == approvedBy );
        var employee = await _userRepository.GetByConditionNoTrack( u => u.Id == salary.UserId );
        await _notificationService.Create( 0, new Dictionary<string, string>
        {
          { "Title", "Cấu hình lương đã được phê duyệt" },
          { "Content", $"{approver?.FullName ?? "Admin"} đã phê duyệt cấu hình lương cho {employee?.FullName ?? "nhân viên"}. Cấu hình sẽ được áp dụng trong lần tính lương tiếp theo." },
          { "ObjectId", salary.Id.ToString() },
          { "CreatedBy", approvedBy.ToString() },
          { "UserName", approver?.FullName ?? "" },
          { "To", salary.CreatedBy.ToString() },
        } );
      }
      catch ( Exception ex ) {
        _logger.LogError( ex, "Lỗi gửi notification khi duyệt cấu hình lương {Id}", id );
      }
    }

    public async Task Deactivate( long id, long editor )
    {
      var salary = await _salaryRepository.GetByCondition( s => s.Id == id );
      if ( salary == null ) throw new Exception( "Không tìm thấy bảng lương" );
      salary.IsActive = false;
      salary.ModifiedBy = editor;
      salary.ModifiedDate = DateTime.UtcNow;
      _salaryRepository.Update( salary );
    }
  }
}
