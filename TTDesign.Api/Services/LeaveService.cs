using AutoMapper;
using ClosedXML.Excel;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class LeaveService : GenericService<LeaveRequest>, ILeaveService
  {
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveRequestDetailRepository _leaveRequestDetailRepository;
    private readonly ILogger<LeaveService> _logger;
    private readonly IMapper _mapper;
    private readonly ILeaveHistoryRepository _leaveHistoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ITimesheetService _timesheetService;
    private readonly ITimesheetRepository _timesheetRepository;
    private readonly ILeaveInformationRepository _leaveInformationRepository;
    private readonly ILeaveRepository _leaveRepository;
    private readonly ITeamService _teamService;
    private readonly INotificationService _notificationService;

    public LeaveService( ILeaveRequestRepository leaveRequestRepository,
      ILogger<LeaveService> logger,
      IMapper mapper,
      ILeaveInformationRepository leaveInformationRepository,
      ILeaveRepository leaveRepository,
      ILeaveHistoryRepository leaveHistoryRepository,
      ITeamRepository teamRepository,
      ILeaveRequestDetailRepository leaveRequestDetailRepository,
      ITimesheetService timesheetService,
      ITimesheetRepository timesheetRepository,
      ITeamService teamService,
      INotificationService notificationService,
      IUserRepository userRepository ) : base( leaveRequestRepository )
    {
      _leaveRequestRepository = leaveRequestRepository;
      _logger = logger;
      _mapper = mapper;
      _userRepository = userRepository;
      _leaveInformationRepository = leaveInformationRepository;
      _leaveRepository = leaveRepository;
      _leaveHistoryRepository = leaveHistoryRepository;
      _teamRepository = teamRepository;
      _timesheetService = timesheetService;
      _timesheetRepository = timesheetRepository;
      _leaveRequestDetailRepository = leaveRequestDetailRepository;
      _teamService = teamService;
      _notificationService = notificationService;
    }

    #region BaseServiceList
    public async Task<IEnumerable<LeaveRequestResponse>> GetRequestList( long userRequest, long year )
    {
      var records = await _leaveRequestRepository.GetListByCondition( o => o.CreatedBy == userRequest && o.StartDate.Year == year );
      var result = _mapper.Map<IEnumerable<LeaveRequestResponse>>( records.OrderByDescending( l => l.StartDate ) );
      var userTemp = new Dictionary<long, string>();
      foreach ( var item in result ) {
        if ( item.Reviewer > 0 ) {
          if ( userTemp.ContainsKey( item.Reviewer ) ) {
            item.ReviewerName = userTemp [ item.Reviewer ];
          }
          else {
            var user = await _userRepository.GetByConditionNoTrack( u => u.Id == item.Reviewer );
            item.ReviewerName = user!.UserName;
            userTemp.Add( item.Reviewer, user.UserName );
          }
        }
      }
      return result;
    }

    /// <summary>
    /// danh sách user request leave trong team leader quản lý
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public async Task<IEnumerable<LeaveRequestResponse>> GetList( BaseFilter filter )
    {
      var leaveRequests = new List<LeaveRequest>();
      if ( filter.TeamId is null ) {
        if ( filter.Year == 1 && filter.Month == 1 ) {
          // lấy tất cả leave request
          var dataAll = await _leaveRequestRepository.GetAll();
          leaveRequests = dataAll.ToList();
        }
        else {
          leaveRequests = ( List<LeaveRequest> ) await _leaveRequestRepository.GetListByCondition( o => o.StartDate.Year == filter.Year && o.StartDate.Month == filter.Month );
        }
      }
      else {
        // lấy danh sách leave request theo creator mà user login quản lý qua team
        var users = await _teamRepository.GetTeamUser( ( long ) filter.TeamId );
        var userIds = users.Where( x => x.Id != filter.UserId ).Select( u => u.Id ).ToList();
        if ( filter.Year == 1 && filter.Month == 1 ) {
          // lấy tất cả leave request
          var dataAll = await _leaveRequestRepository.GetAll();
          leaveRequests = dataAll.Where( o => userIds.Contains( o.CreatedBy ) ).ToList();
        }
        else {
          leaveRequests = ( List<LeaveRequest> ) await _leaveRequestRepository.GetListByCondition( o => o.StartDate.Year == filter.Year && o.StartDate.Month == filter.Month &&
         userIds.Contains( o.CreatedBy ) );
        }

      }
      var result = _mapper.Map<IEnumerable<LeaveRequestResponse>>( leaveRequests.OrderByDescending( l => l.StartDate ) );
      var userTemp = new Dictionary<long, string>();
      ///var teams = new Dictionary<long, Team>();
      foreach ( var item in result ) {
        if ( userTemp.ContainsKey( item.CreatedBy ) ) {
          item.CreatorName = userTemp [ item.CreatedBy ];
        }
        else {
          var user = await _userRepository.GetByConditionNoTrack( u => u.Id == item.CreatedBy );
          item.CreatorName = user!.FullName;
          userTemp.Add( item.CreatedBy, user.FullName );
        }
        if ( item.Reviewer > 0 ) {
          if ( userTemp.ContainsKey( item.Reviewer ) ) {
            item.ReviewerName = userTemp [ item.Reviewer ];
          }
          else {
            var user = await _userRepository.GetByConditionNoTrack( u => u.Id == item.Reviewer );
            item.ReviewerName = user!.FullName;
            userTemp.Add( item.Reviewer, user.FullName );
          }
        }
        // set team
        var teamUsers = await _teamService.GetTeamUserByUserId( item.CreatedBy );
        if ( teamUsers != null ) {
          foreach ( var teamUser in teamUsers ) {
            //if ( teams.ContainsKey( teamUser.TeamId ) ) {
            //  var team = teams [ teamUser.TeamId ];
            //  item.Teams = teamUsers.Select( tu => new TeamUserOption()
            //  {
            //    TeamCode = team!.Code,
            //    TeamName = team.Name
            //  } ).ToList();
            //  ;
            //}
            //else {
            //  var team = await _teamService.GetByCondition( t => t.Id == teamUser.TeamId );
            //  teams.Add( teamUser.TeamId, team! );
            //  item.Teams = teamUsers.Select( tu => new TeamUserOption()
            //  {
            //    TeamCode = team!.Code,
            //    TeamName = team.Name
            //  } ).ToList();
            //}
            var team = await _teamService.GetByCondition( t => t.Id == teamUser.TeamId );
            item.Teams.Add( new TeamUserOption()
            {
              TeamCode = team!.Code,
              TeamName = team.Name
            } );
            ;
          }
        }
      }
      return result;
    }
    #endregion

    #region BaseServiceResource
    public async Task<long> Create( LeaveRequestResource resource, long creator )
    {
      var leaveRequest = _mapper.Map<LeaveRequest>( resource );
      leaveRequest.CreatedBy = creator;
      leaveRequest.ModifiedBy = creator;
      leaveRequest.LeaveRequestDetails = _mapper.Map<List<LeaveRequestDetail>>( resource.LeaveDetail );
      // create
      await _leaveRequestRepository.CreateAsync( leaveRequest );
      // tạo noti yêu cầu approve/reject request
      await _notificationService.Create( ( int ) Enums.NotificationObjectType.LeaveRequest, leaveRequest );
      return leaveRequest.Id;
    }

    public async Task Update( LeaveRequestResource resource, long editor )
    {
      await _leaveRequestDetailRepository.DeleteByCondition( l => l.LeaveRequestId == resource.Id );
      var old = await _leaveRequestRepository.GetByCondition( o => o.Id == resource.Id );
      _mapper.Map( resource, old! );
      old!.Status = ( int ) Enums.LeaveRequestStatus.Pending;
      old.Reviewer = null;
      old.ModifiedBy = editor;
      old.LeaveRequestDetails = _mapper.Map<List<LeaveRequestDetail>>( resource.LeaveDetail );
      _leaveRequestRepository.Update( old );
      // assign notification
      await _notificationService.ChangeUserAssign( ( int ) Enums.NotificationObjectType.LeaveRequest, old.Id, ( int ) Enums.NotificationType.Information, old.CreatedBy );
    }
    #endregion

    #region others
    public async Task Approve( long id, bool isApprove, long reviewer )
    {
      var old = await _leaveRequestRepository.GetByCondition( o => o.Id == id );
      var nextAction = 0;
      if ( old!.Status == ( int ) Enums.LeaveRequestStatus.Pending && isApprove )
        nextAction = 1;
      else if ( old.Status == ( int ) Enums.LeaveRequestStatus.Approve && !isApprove )
        nextAction = 2;
      old!.Status = ( int ) ( isApprove ? Enums.LeaveRequestStatus.Approve : Enums.LeaveRequestStatus.Reject );
      old.Reviewer = reviewer;
      old.ModifiedBy = reviewer;
      _leaveRequestRepository.Update( old );
      old.LeaveRequestDetails = ( await _leaveRequestDetailRepository.GetListByCondition( l => l.LeaveRequestId == id ) ).ToList();
      // then apply timesheet
      // pending to reject: do nothing
      // pending to approve: add timesheet
      if ( nextAction == 1 ) {
        // lấy leave record
        if ( ( int ) Enums.LeaveType.Annual == old!.Type ) {
          await UsingLeave( id );
        }
        // apply timesheet
        await _timesheetService.ApplyLeaveRequestApproved( old );
      }
      // approve to reject: remove timesheet
      if ( nextAction == 2 ) {
        // lấy tính toán lại leave record
        if ( ( int ) Enums.LeaveType.Annual == old!.Type ) {
          await RevertLeave( id );
        }
        // thu hồi timesheet
        await _timesheetService.RemoveLeaveRequestReject( old );
      }
      // assign notification
      await _notificationService.ChangeUserAssign( ( int ) Enums.NotificationObjectType.LeaveRequest, id,
        isApprove ? ( int ) Enums.NotificationType.Approved : ( int ) Enums.NotificationType.Rejected, old.CreatedBy );
    }

    public async Task<IEnumerable<LeaveInformationResponse>> GetLeaveInformationResponses()
    {
      return _mapper.Map<IEnumerable<LeaveInformationResponse>>( await _leaveInformationRepository.GetAll() );
    }

    public async Task<RemainLeave> GetRemainLeave( long userId )
    {
      var leaveHistory = await _leaveHistoryRepository.GetLast( userId );
      return leaveHistory is null ? new RemainLeave() : _mapper.Map<RemainLeave>( leaveHistory );
    }

    public async Task<IEnumerable<LeaveHistoryResponse>> GetLeaveHistories( long userId, long year )
    {
      var records = await _leaveHistoryRepository.GetListByCondition( l => l.CreatedDate.Year == year && l.CreatedBy == userId );
      var result = _mapper.Map<IEnumerable<LeaveHistoryResponse>>( records );
      foreach ( var rec in result ) {
        var user = await _userRepository.GetByConditionNoTrack( x => x.Id == rec.ModifiedBy );
        rec.ModifiedName = user?.FullName;
      }
      return result;
    }

    /// <summary>
    /// khi leave request được approve, trừ số ngày nghỉ còn lại trong bảng leave đi,
    /// chỉ áp dụng với type annual
    /// </summary>
    /// <param name="leaveRequestId"></param>
    /// <returns></returns>
    private async Task UsingLeave( long leaveRequestId )
    {
      var leaveRequest = await _leaveRequestRepository.GetByCondition( l => l.Id == leaveRequestId );
      if ( leaveRequest is null || leaveRequest.Status != ( int ) Enums.LeaveRequestStatus.Approve ) {
        return;
      }
      var leaves = ( await _leaveRepository.GetListByCondition( l => l.UserId == leaveRequest.CreatedBy && l.Type == leaveRequest.Type && l.Using < l.Hours ) ).ToList();
      leaveRequest = ShareLeave( leaveRequest, ref leaves );
      _leaveRepository.Updates( leaves.ToArray() ); // cập nhật thông tin leave sau khi đã sử dụng cho leave request
      _leaveRequestRepository.Update( leaveRequest ); // tạo dữ liệu bảng leave history using: quan hệ giữa bảng leave request và leave
      // tạo leave history
      var lastHistory = await _leaveHistoryRepository.GetLast( leaveRequest.CreatedBy );
      await _leaveHistoryRepository.CreateAsync( new LeaveHistory()
      {
        CreatedBy = leaveRequest.CreatedBy,
        Description = $"Use {Enum.GetName( ( Enums.LeaveType ) leaveRequest.Type )} from {leaveRequest.StartDate.ToString( Enums.DATETIME_FORMAT )} to {leaveRequest.EndDate.ToString( Enums.DATETIME_FORMAT )}",
        Type = Common.MappingLeaveRequestTypeToLeaveHistoryTypeWhenCreateHistoryApproveRequest( leaveRequest.Type ),
        Unit = -leaveRequest.Hour,
        AnnualLeave = ( lastHistory != null ? lastHistory.AnnualLeave : 0 ) - ( leaveRequest.Type == ( int ) Enums.LeaveType.Annual ? leaveRequest.Hour / 8.0 : 0 ),
        LeaveRequestId = leaveRequest.Id
      } );
    }

    /// <summary>
    /// tính toán lại leave do request bị chuyển trạng thái từ approve sang reject
    /// </summary>
    /// <param name="leaveRequestId"></param>
    /// <returns></returns>
    private async Task RevertLeave( long leaveRequestId )
    {
      var leaveRequest = await _leaveRequestRepository.GetByConditionNoTrack( l => l.Id == leaveRequestId );
      if ( leaveRequest is null || leaveRequest.Status != ( int ) Enums.LeaveRequestStatus.Reject ) {
        return;
      }
      // tính toán lại danh sách leave đã sử dụng
      var leaves = await _leaveRepository.GetListBeginLeaveRequest( leaveRequest.Type, leaveRequestId ); // danh sách leave, kể từ leave id quan hệ với leave request id, tới leave id cuối cùng trong bảng
      var leaveRequests = await _leaveRequestRepository.GetListByLeaves( leaves.Select( l => l.Id ).ToList(), leaveRequestId ); // các leave request tương ứng quan hệ với danh sách leave trên
      var leaveRequestAfterMath = new List<LeaveRequest>(); // danh sách leave request sau khi update lại quan hệ với leaves
      foreach ( var request in leaveRequests ) { // tính toán phân bổ lại leave và các request đã được approve sau của request leave chỉ định
        leaveRequestAfterMath.Add( ShareLeave( request, ref leaves ) );
      }

      if ( leaveRequestAfterMath.Count() > 0 ) {
        // xóa leave history using
        await _leaveRequestRepository.RemoveLeaveHistoryUsing( leaveRequestAfterMath.Select( l => l.Id ).ToList() );

        // tạo lại leave history using
        _leaveRequestRepository.Updates( leaveRequestAfterMath.ToArray() );
      }

      await _leaveRequestRepository.RemoveLeaveHistoryUsing( new List<long> { leaveRequestId } );
      // cập nhật leave
      _leaveRepository.Updates( leaves.ToArray() );

      // cập nhật leave history tương ứng
      var leaveHistory = await _leaveHistoryRepository.GetByConditionNoTrack( l => l.LeaveRequestId == leaveRequestId );
      var histories = await _leaveHistoryRepository.GetListByConditionTrack( l => l.Id > leaveHistory!.Id );
      foreach ( var history in histories ) {
        if ( leaveHistory!.Type == ( int ) Enums.LeaveHistoryType.UsingAnnualLeave ) {
          history.AnnualLeave -= ( double ) leaveHistory.Unit! / 8; // vì giá trị thực tế của leaveHistory.Unit là giá trị âm
        }
      }
      if ( histories.Count() > 0 )
        _leaveHistoryRepository.Updates( histories.ToArray() );
      await _leaveHistoryRepository.DeleteByCondition( l => l.Id == leaveHistory!.Id );
    }

    /// <summary>
    /// logic tuần tự phân chia leave cho leave request
    /// </summary>
    /// <param name="request">leave request</param>
    /// <param name="leaves">danh sách leave sau khi được update</param>
    /// <returns>trả về request sau khi đã được cập nhật leave tương ứng kèm danh sách leave đã được update trường using</returns>
    private LeaveRequest ShareLeave( LeaveRequest request, ref List<Leave> leaves )
    {
      var hour = request.Hour;
      foreach ( var leave in leaves.Where( l => l.Using < l.Hours ).OrderBy( l => l.Id ) ) {
        if ( hour <= ( leave.Hours - leave.Using ) ) {
          leave.Using += hour;
          request.LeaveHistoryUsings.Add( new LeaveHistoryUsing()
          {
            LeaveId = leave.Id,
            LeaveRequestId = request.Id,
            Hours = hour
          } );
          break;
          ;
        }
        hour -= leave.Hours - leave.Using;
        request.LeaveHistoryUsings.Add( new LeaveHistoryUsing()
        {
          LeaveId = leave.Id,
          LeaveRequestId = request.Id,
          Hours = leave.Hours - leave.Using
        } );
        leave.Using = leave.Hours;
      }
      return request;
    }

    public async Task<bool> CheckTimesheetHadLock( DateTime start, DateTime end )
    {
      return await _timesheetService.Exist( t => t.Date >= start.Date && t.Date <= end.Date && t.LockBy != null );
    }

    public async Task<int> GetTotalLeaveRequest( long? teamId = null )
    {
      var leaveRequests = ( List<LeaveRequest> ) await _leaveRequestRepository.GetListByCondition( o => o.Status == ( int ) Enums.LeaveRequestStatus.Pending );
      if ( teamId is not null ) {
        var users = await _teamRepository.GetTeamUser( ( long ) teamId );
        var userIds = users.Select( u => u.Id ).ToList();
        leaveRequests = ( List<LeaveRequest> ) await _leaveRequestRepository.GetListByCondition( o => o.Status == ( int ) Enums.LeaveRequestStatus.Pending &&
          userIds.Contains( o.CreatedBy ) );
      }
      return leaveRequests != null ? leaveRequests.Count() : 0;
    }

    public async Task Delete( long id )
    {
      await _leaveRequestDetailRepository.DeleteByCondition( l => l.LeaveRequestId == id );
      await _leaveRequestRepository.DeleteByCondition( l => l.Id == id );
    }

    public async Task<LeaveReport> GetReport( long? teamId, long year )
    {
      var leaveReport = new LeaveReport()
      {
        AnnualLeaves = new List<LeaveReportDetail>(),
      };
      var users = await _userRepository.GetUsersDataByCondition( teamId );
      users = users.Where( u => u.IsActive && u.Id != Enums.SYSTEM_ID ).OrderBy( u => u.UserName ).ToArray();
      var startOfYear = new DateTime( ( int ) year, 1, 1 );
      foreach ( var user in users.OrderBy( u => u.StaffId ) ) {
        // danh sách leave request được approved trong năm       
        var leaveRequests = await _leaveRequestRepository.GetLeaveRequestDetails( l => l.CreatedBy == user.Id &&
          l.Status == ( int ) Enums.LeaveRequestStatus.Approve &&
          ( l.StartDate.Year == year || l.EndDate.Year == year ) );
        var leaveRequestDetailsByMonth = leaveRequests
          .SelectMany( l => l.LeaveRequestDetails.Select( d => new { l.Type, Detail = d } ) )
          .Where( x => x.Detail.Date >= startOfYear && x.Detail.Date < startOfYear.AddYears( 1 ) )
          .GroupBy( x => x.Detail.Date.Month )
          .ToDictionary(
              g => g.Key,
              g => g.GroupBy( x => x.Type )
                    .ToDictionary(
                        tg => tg.Key,
                        tg => tg.Sum( x => x.Detail.Hours ) )
          );

        // danh sách leave request được approved trong năm       
        var leaveRequestUsings = await _leaveRequestRepository.GetLeaveRequestDetails( l => l.CreatedBy == user.Id &&
          l.Status == ( int ) Enums.LeaveRequestStatus.Approve );

        var leaveRequestDetailsByMonthUsing = leaveRequestUsings
          .SelectMany( l => l.LeaveRequestDetails.Select( d => new { l.Type, Detail = d } ) )
          .GroupBy( x => x.Detail.Date.Month )
          .ToDictionary(
              g => g.Key,
              g => g.GroupBy( x => x.Type )
                    .ToDictionary(
                        tg => tg.Key,
                        tg => tg.Sum( x => x.Detail.Hours ) )
          );
        var leaveHistories = await _leaveHistoryRepository.GetListByCondition( l => l.CreatedBy == user.Id && !l.Description.StartsWith( "Use Annual" ) && !l.Description.StartsWith( "Take back" ) );
        leaveReport.AnnualLeaves.Add( new LeaveReportDetail()
        {
          UserId = user.Id,
          UserName = user.UserName,
          FullName = user.FullName,
          Staff = user.StaffId,
          TeamId = user.TeamUsers != null ? user.TeamUsers.Select( x => x.TeamId ).ToArray() : Array.Empty<long>(),
          Team = user.TeamUsers != null ? string.Join( ", ", user.TeamUsers.Select( x => x.Team.Code ) ) : string.Empty,
          Total = leaveHistories.Any() ? ( double ) ( leaveHistories.Sum( x => x.Unit ) / 8 ) : 0,
          TotalUsing = leaveRequestDetailsByMonthUsing.Values.Where( x => x.ContainsKey( ( int ) Enums.LeaveType.Annual ) ).Sum( x => x [ ( int ) Enums.LeaveType.Annual ] ) / 8,
          Jan = await CalLeaveDayByMonth( 1, leaveRequestDetailsByMonth ),
          Feb = await CalLeaveDayByMonth( 2, leaveRequestDetailsByMonth ),
          Mar = await CalLeaveDayByMonth( 3, leaveRequestDetailsByMonth ),
          Apr = await CalLeaveDayByMonth( 4, leaveRequestDetailsByMonth ),
          May = await CalLeaveDayByMonth( 5, leaveRequestDetailsByMonth ),
          Jun = await CalLeaveDayByMonth( 6, leaveRequestDetailsByMonth ),
          Jul = await CalLeaveDayByMonth( 7, leaveRequestDetailsByMonth ),
          Aug = await CalLeaveDayByMonth( 8, leaveRequestDetailsByMonth ),
          Sep = await CalLeaveDayByMonth( 9, leaveRequestDetailsByMonth ),
          Oct = await CalLeaveDayByMonth( 10, leaveRequestDetailsByMonth ),
          Nov = await CalLeaveDayByMonth( 11, leaveRequestDetailsByMonth ),
          Dec = await CalLeaveDayByMonth( 12, leaveRequestDetailsByMonth ),
        } );

      }
      return leaveReport;
    }
    private async Task<List<LeaveReportMonthly>> CalLeaveDayByMonth( int month, Dictionary<int, Dictionary<int, double>> leaveDetail )
    {
      var result = new List<LeaveReportMonthly>();
      if ( !leaveDetail.ContainsKey( month ) )
        return result;
      var valLeave = leaveDetail [ month ];

      foreach ( var key in valLeave.Keys ) {
        var reportLeaveType = new LeaveReportMonthly
        {
          Type = Enum.GetName( typeof( Enums.LeaveType ), key )!.ToString(),
          Day = Math.Round( valLeave [ key ] / 8, 1 )
        };
        result.Add( reportLeaveType );
      }
      return result;
    }
    public async Task<byte []?> ExportLeaveDayStaff( long? teamId, long year )
    {
      // get list user
      var users = await _userRepository.GetUsersDataByCondition( teamId );
      users = users.Where( u => u.IsActive && u.Id != Enums.SYSTEM_ID ).OrderBy( u => u.UserName ).ToArray();
      // get data report
      var report = await GetReport( teamId, year );
      byte [] workbookBytes;
      var pathFileTemp = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Excel", "LeaveDayStaffs.xlsx" );

      using ( MemoryStream templateStream = new() ) {
        using ( FileStream fileStream = new( pathFileTemp, FileMode.Open, FileAccess.Read ) ) {
          fileStream.CopyTo( templateStream );
          templateStream.Position = 0;
        }
        var workBook = new XLWorkbook( templateStream );
        var workSheetLeave = workBook.Worksheet( "Sheet1" );
        #region sheet Leave
        // set header
        workSheetLeave.Cell( $"A1" ).Value = $"BẢNG THEO DÕI NGÀY PHÉP CỦA NHÂN VIÊN NĂM {year}";
        workSheetLeave.Cell( $"F2" ).Value = $"Tổng số ngày phép còn năm {year - 1}";
        workSheetLeave.Cell( $"G2" ).Value = $"Tổng số ngày phép được hưởng năm {year}";
        workSheetLeave.Cell( $"H2" ).Value = $"Tổng số ngày phép chưa dùng năm {year}";
        workSheetLeave.Cell( $"I2" ).Value = $"NGÀY NGHỈ THÁNG  01/{year} ~ THÁNG 12/{year}";

        for ( var i = 0; i < report.AnnualLeaves.Count(); i++ ) {
          // cell STT
          workSheetLeave.Cell( $"A{5 + i}" ).Value = i + 1;
          // cell staff
          workSheetLeave.Cell( $"B{5 + i}" ).Value = report.AnnualLeaves [ i ].Staff;
          // cell full name 
          workSheetLeave.Cell( $"C{5 + i}" ).Value = report.AnnualLeaves [ i ].FullName;
          // cell start
          workSheetLeave.Cell( $"D{5 + i}" ).Value = $"{year}/01/01";
          // cell end
          workSheetLeave.Cell( $"E{5 + i}" ).Value = $"{year}/12/31";
          // cell total leave remain of pre year
          var leaveHistories = await _leaveHistoryRepository.GetListByCondition( l => l.CreatedBy == report.AnnualLeaves [ i ].UserId );
          leaveHistories = leaveHistories.OrderBy( l => l.CreatedDate ).ToList();
          double totalLeaveRemainPreYear = leaveHistories.Any() ? leaveHistories.Last().AnnualLeave : 0;
          workSheetLeave.Cell( $"F{5 + i}" ).Value = totalLeaveRemainPreYear;
          // cell total leave this year
          workSheetLeave.Cell( $"G{5 + i}" ).Value = report.AnnualLeaves [ i ].Total;
          // cell leave remain after this year
          workSheetLeave.Cell( $"H{5 + i}" ).Value = totalLeaveRemainPreYear + report.AnnualLeaves [ i ].Total - report.AnnualLeaves [ i ].TotalUsing;
          // cell jan
          workSheetLeave.Cell( $"I{5 + i}" ).Value = report.AnnualLeaves [ i ].Jan.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Jan ) : string.Empty;
          // cell fed                                                             
          workSheetLeave.Cell( $"J{5 + i}" ).Value = report.AnnualLeaves [ i ].Feb.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Feb ) : string.Empty;
          // cell mar                                                                                                        
          workSheetLeave.Cell( $"K{5 + i}" ).Value = report.AnnualLeaves [ i ].Mar.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Mar ) : string.Empty;
          // cell apr                                                                                                         
          workSheetLeave.Cell( $"L{5 + i}" ).Value = report.AnnualLeaves [ i ].Apr.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Apr ) : string.Empty;
          // cell may                                                                                                         
          workSheetLeave.Cell( $"M{5 + i}" ).Value = report.AnnualLeaves [ i ].May.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].May ) : string.Empty;
          // cell jun                                                                                                        
          workSheetLeave.Cell( $"N{5 + i}" ).Value = report.AnnualLeaves [ i ].Jun.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Jun ) : string.Empty;
          // cell jul                                                                                                         
          workSheetLeave.Cell( $"O{5 + i}" ).Value = report.AnnualLeaves [ i ].Jul.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Jul ) : string.Empty;
          // cell aug                                                                                                         
          workSheetLeave.Cell( $"P{5 + i}" ).Value = report.AnnualLeaves [ i ].Aug.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Aug ) : string.Empty;
          // cell sep                                                                                                         
          workSheetLeave.Cell( $"Q{5 + i}" ).Value = report.AnnualLeaves [ i ].Sep.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Sep ) : string.Empty;
          // cell oct                                                                                                        
          workSheetLeave.Cell( $"R{5 + i}" ).Value = report.AnnualLeaves [ i ].Oct.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Oct ) : string.Empty;
          // cell nov                                                                                                        
          workSheetLeave.Cell( $"S{5 + i}" ).Value = report.AnnualLeaves [ i ].Nov.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Nov ) : string.Empty;
          // cell dev                                                                                                        
          workSheetLeave.Cell( $"T{5 + i}" ).Value = report.AnnualLeaves [ i ].Dec.Count() > 0 ? GetTotalLeaveAnnual( report.AnnualLeaves [ i ].Dec ) : string.Empty;
        }
        #endregion      
        workSheetLeave.Name = "Theo dõi ngày nghỉ thường";
        workBook.Save();
        workbookBytes = templateStream.ToArray();
      }
      return workbookBytes;
    }

    private string GetTotalLeaveAnnual( List<LeaveReportMonthly> source )
    {
      var d = source.Where( x => x.Type == Enums.LeaveType.Annual.ToString() ).Sum( x => x.Day );
      return d > 0 ? d.ToString() : string.Empty;
    }

    public async Task UpdateAnunalLeave( long userId, double annualLeave, string notes )
    {
      var leaveHistories = await _leaveHistoryRepository.GetListByCondition( l => l.CreatedBy == userId && !l.Description.StartsWith( "Use Annual" ) && !l.Description.StartsWith( "Take back" ) );
      var last = leaveHistories.OrderBy( x => x.Id ).LastOrDefault();
      var leaveHistory = new LeaveHistory()
      {
        LeaveId = null,
        Type = ( int ) Enums.LeaveHistoryType.AddAnnualLeave,
        Description = notes,
        AnnualLeave = annualLeave,
        CreatedBy = userId,
        Unit = 0
      };
      await _leaveHistoryRepository.CreateAsync( leaveHistory );
    }
    #endregion
  }
}
