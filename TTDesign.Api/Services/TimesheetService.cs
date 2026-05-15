using AutoMapper;
using ClosedXML.Excel;
using System.Drawing;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class TimesheetService : GenericService<Timesheet>, ITimesheetService
  {
    private readonly ITimesheetRepository _timesheetRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITimesheetDetailRepository _timesheetDetailRepository;
    private readonly ITimesheetReportRepository _timesheetReportRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ITeamCategoryRepository _categoryRepository;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IHolidayRepository _holidayRepository;
    private readonly ISwapDayRepository _swapDayRepository;
    private readonly ISwapDayUserRepository _swapDayUserRepository;
    private readonly ISwapDayReferRepository _swapDayReferRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ILogger<TimesheetService> _logger;
    private readonly IMapper _mapper;
    private readonly IConfigRepository _configRepository;
    private readonly IWebHostEnvironment _env;

    public TimesheetService( ITimesheetRepository timesheetRepository,
      ITimesheetDetailRepository timesheetDetailRepository,
      ITimesheetReportRepository timesheetReportRepository,
      IProjectRepository projectRepository,
      ITeamCategoryRepository categoryRepository,
      IUserRepository userRepository,
      ILeaveRequestRepository leaveRequestRepository,
      IHolidayRepository holidayRepository,
      ISwapDayRepository swapDayRepository,
      ISwapDayUserRepository swapDayUserRepository,
      ISwapDayReferRepository swapDayReferRepository,
      ITeamRepository teamRepository,
      ILogger<TimesheetService> logger,
      IConfigRepository configRepository,
      IWebHostEnvironment env,
      IMapper mapper ) : base( timesheetRepository )
    {
      _timesheetRepository = timesheetRepository;
      _timesheetDetailRepository = timesheetDetailRepository;
      _timesheetReportRepository = timesheetReportRepository;
      _categoryRepository = categoryRepository;
      _projectRepository = projectRepository;
      _userRepository = userRepository;
      _leaveRequestRepository = leaveRequestRepository;
      _holidayRepository = holidayRepository;
      _swapDayRepository = swapDayRepository;
      _swapDayUserRepository = swapDayUserRepository;
      _swapDayReferRepository = swapDayReferRepository;
      _teamRepository = teamRepository;
      _logger = logger;
      _mapper = mapper;
      _configRepository = configRepository;
      _env = env;
    }

    #region BaseServiceList
    /// <summary>
    /// lấy danh sách ngày trong tháng
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TimesheetResponse>> GetList( BaseFilter filter )
    {
      var results = new List<TimesheetResponse>();
      if ( filter.Year < 2023 ) {
        return results;
      }
      if ( filter.DateCheck >= DateTime.UtcNow.Date.AddMonths( 2 ).AddDays( 1 - DateTime.UtcNow.Day ) ) {
        var holidays = await _holidayRepository.GetDataByCondition( h => h.Type == ( int ) Enums.HolidayType.Holiday && ( ( h.StartDate.Year == filter.Year && h.StartDate.Month == filter.Month ) ||
          ( h.EndDate.Year == filter.Year && h.EndDate.Month == filter.Month ) ) );
        var holidayCheck = new Dictionary<DateTime, string>();
        foreach ( var holiday in holidays ) {
          var start = holiday.StartDate;
          do {
            holidayCheck.Add( start, holiday.Name );
            start = start.AddDays( 1 );
          } while ( start <= holiday.EndDate );
        }
        var dateIndex = new DateTime( ( int ) filter.Year, ( int ) filter.Month, 1 );
        var dateEnd = new DateTime( ( int ) filter.Year, filter.Month != 12 ? ( int ) filter.Month + 1 : 12, filter.Month != 12 ? 1 : 31 );
        do {
          results.Add( new TimesheetResponse()
          {
            TimeIn = Enums.TimeCheckDefault,
            TimeOut = Enums.TimeCheckDefault,
            Date = dateIndex,
            HolidayName = holidayCheck.ContainsKey( dateIndex ) ? holidayCheck [ dateIndex ] : string.Empty,
          } );
          dateIndex = dateIndex.AddDays( 1 );
        } while ( dateIndex < dateEnd );
        return results;
      }
      // user không là admin thì ko xem được thông tin của user khác team
      if ( filter.TeamId is not null && !await _userRepository.Exist( u => u.Id == filter.UserId && u.TeamUsers.Any( x => x.TeamId == filter.TeamId ) ) ) {
        return results;
      }
      var timesheets = await _timesheetRepository.GetList( ( long ) filter.UserId!, filter.Year, filter.Month );
      var projects = new Dictionary<long, Project>();
      foreach ( var ts in timesheets ) {
        var tsResponse = new TimesheetResponse
        {
          Id = ts.Id,
          Lock = ts.LockBy is not null,
          IsWfh = ts.WfhRequestId is not null && ts.WfhRequestId > 0,
          TimeIn = ( ts.WfhRequestId is not null && ts.WfhRequestId > 0 ) ? Enums.TimeWfhCheckInDefault :
            ts.FingerPrinter == null ? Enums.TimeCheckDefault :
            ts.FingerPrinter.SwapDayRefer != null ? ( ts.FingerPrinter.SwapDayRefer.DateIn == ts.FingerPrinter.SwapDayRefer.DateIn.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.SwapDayRefer.DateIn.ToString( Enums.HOUR_FORMAT ) ) :
            ( ts.FingerPrinter.DateIn == ts.FingerPrinter.DateIn.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.DateIn.ToString( Enums.HOUR_FORMAT ) ),
          TimeOut = ( ts.WfhRequestId is not null && ts.WfhRequestId > 0 ) ? Enums.TimeWfhCheckOutDefault :
            ts.FingerPrinter == null ? Enums.TimeCheckDefault :
            ts.FingerPrinter.SwapDayRefer != null ? ( ts.FingerPrinter.SwapDayRefer.DateOut == ts.FingerPrinter.SwapDayRefer.DateOut.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.SwapDayRefer.DateOut.ToString( Enums.HOUR_FORMAT ) ) :
            ( ts.FingerPrinter.DateOut == ts.FingerPrinter.DateOut.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.DateOut.ToString( Enums.HOUR_FORMAT ) ),
          Date = ts.Date,
          HolidayName = ts.HolidayName,
          SwapDayDetail = ts.SwapDay is not null ? string.Format( ErrorMessageResource.SwapMessage, ts.SwapDay?.ToString( Enums.DATE_FORMAT ) ) : null,
          HourWorking = ts.TimesheetReports.Where( t => t.ProjectId > 0 ).Sum( t => t.Hours ),
          HourTotal = ( ts.WfhRequestId is not null && ts.WfhRequestId > 0 ) ? Enums.TimeWork :
            ts.FingerPrinter == null ? 0 :
            ts.FingerPrinter.SwapDayRefer != null ? ts.FingerPrinter.SwapDayRefer.HourTotal : ts.FingerPrinter.HourTotal,
          Tasks = ts.TimesheetReports.GroupBy( t => t.ProjectId ).Select( t => new TimesheetData()
          {
            ProjectId = t.Key,
            Type = t.Key == Enums.TimesheetReportPaidLeave ?
              Enum.GetName( Enums.TimesheetDetailType.PaidLeave )! :
              t.Key == Enums.TimesheetReportUnpaidLeave ? Enum.GetName( Enums.TimesheetDetailType.UnpaidLeave )! : Enum.GetName( Enums.TimesheetDetailType.Project )!,
            Hours = Common.FormatHoursDoubleToString( t.Sum( t => t.Hours ) )
          } ).ToList()
        }
      ;
        foreach ( var task in tsResponse.Tasks ) {
          if ( task.ProjectId == Enums.TimesheetReportPaidLeave ) {
            task.ProjectName = Enums.TimesheetReportPaidLeaveName;
          }
          else if ( task.ProjectId == Enums.TimesheetReportUnpaidLeave ) {
            task.ProjectName = Enums.TimesheetReportUnpaidLeaveName;
          }
          else if ( projects.ContainsKey( task.ProjectId ) ) {
            task.ProjectName = Common.FormatProjectName( projects [ task.ProjectId ] );
          }
          else {
            var project = await _projectRepository.GetByConditionNoTrack( p => p.Id == task.ProjectId );
            projects.Add( task.ProjectId, project! );
            task.ProjectName = Common.FormatProjectName( project! );
          }
        }
        results.Add( tsResponse );
      }
      return results;
    }
    #endregion

    #region BaseServiceDetail
    /// <summary>
    /// lấy danh sách task trong ngày
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TimesheetDetailResponse?> GetDetail( long id )
    {
      var ts = await _timesheetRepository.GetTimesheetDetailById( id );
      if ( ts == null )
        return null;
      //var types = new HashSet<int>
      //{
      //  (int) Enums.TimesheetDetailType.Project,
      //};
      ts.TimesheetDetails = ts.TimesheetDetails.Where( t => t.Type == ( int ) Enums.TimesheetDetailType.Project ).ToList();
      var result = new TimesheetDetailResponse()
      {
        Id = id,
        Lock = ts.LockBy is not null,
        IsWfh = ts.WfhRequestId > 0,
        TimeIn = ts.WfhRequestId > 0 ? Enums.TimeWfhCheckInDefault :
          ts.FingerPrinter == null ? Enums.TimeCheckDefault :
          ts.FingerPrinter.SwapDayRefer != null ? ( ts.FingerPrinter.SwapDayRefer.DateIn == ts.FingerPrinter.SwapDayRefer.DateIn.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.SwapDayRefer.DateIn.ToString( Enums.HOUR_FORMAT ) ) :
          ( ts.FingerPrinter.DateIn == ts.FingerPrinter.DateIn.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.DateIn.ToString( Enums.HOUR_FORMAT ) ),
        TimeOut = ts.WfhRequestId > 0 ? Enums.TimeWfhCheckOutDefault :
          ts.FingerPrinter == null ? Enums.TimeCheckDefault :
          ts.FingerPrinter.SwapDayRefer != null ? ( ts.FingerPrinter.SwapDayRefer.DateOut == ts.FingerPrinter.SwapDayRefer.DateOut.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.SwapDayRefer.DateOut.ToString( Enums.HOUR_FORMAT ) ) :
          ( ts.FingerPrinter.DateOut == ts.FingerPrinter.DateOut.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.DateOut.ToString( Enums.HOUR_FORMAT ) ),
        HourTotal = ts.WfhRequestId > 0 ? Common.FormatHoursDoubleToString( Enums.TimeWork ) :
          ts.FingerPrinter == null ? Common.FormatHoursDoubleToString( 0 ) :
          ts.FingerPrinter.SwapDayRefer != null ? Common.FormatHoursDoubleToString( ts.FingerPrinter.SwapDayRefer.HourTotal ) : Common.FormatHoursDoubleToString( ts.FingerPrinter.HourTotal ),
        HourWorking = Common.FormatHoursDoubleToString( ts.TimesheetDetails.Sum( t => t.Hours ) ),
        Projects = new List<TimesheetDetailProjectData>()
      };
      // format list task detail: tạo record title, sắp xếp dữ liệu
      if ( ts.TimesheetDetails.Any() ) {
        var tsDetailProject = new TimesheetDetailProjectData();
        long projectIdIndex = 0;
        double hours = 0;
        var objects = new Dictionary<long, string>();
        var categorys = new Dictionary<long, string>();
        foreach ( var tsDetail in ts.TimesheetDetails.OrderBy( t => t.ProjectId ) ) {
          if ( projectIdIndex == 0 || projectIdIndex != tsDetail.ProjectId ) {
            if ( projectIdIndex != 0 ) { // nếu là project đầu tiên thì không làm hành động này
              tsDetailProject.Hours = Common.FormatHoursDoubleToString( hours );
              result.Projects.Add( tsDetailProject );
            }
            // tạo record title chỉ chứa tên của project
            projectIdIndex = ( long ) tsDetail.ProjectId!;
            hours = 0;
            var project = await _projectRepository.GetByConditionNoTrack( p => p.Id == tsDetail.ProjectId );
            var categories = await _categoryRepository.GetListByConditionTrack( c => c.TeamId == project!.TeamId );
            tsDetailProject = new TimesheetDetailProjectData()
            {
              ProjectId = tsDetail.ProjectId!,
              ProjectName = Common.FormatProjectName( project! ),
              Tasks = new List<TimesheetDetailData>(),
              Categories = categories.Select( c => new CategoryProject
              {
                CategotyId = c.Id,
                CategoryName = c.Name,
                ProjectId = tsDetail.ProjectId!,
              } ).ToList()
            };
          }
          var tsDetailData = _mapper.Map<TimesheetDetailData>( tsDetail );
          // set category
          if ( categorys.ContainsKey( tsDetailData.CategoryId ) ) {
            tsDetailData.Category = categorys [ tsDetailData.CategoryId ];
          }
          else {
            var category = await _categoryRepository.GetByConditionNoTrack( c => c.Id == tsDetailData.CategoryId );
            tsDetailData.Category = category!.Name;
            categorys.Add( tsDetailData.CategoryId, category!.Name );
          }
          hours += tsDetail.Hours;
          tsDetailProject.Tasks!.Add( tsDetailData );
        }
        tsDetailProject.Hours = Common.FormatHoursDoubleToString( hours );
        result.Projects.Add( tsDetailProject );
      }
      else {
        result.Projects = new List<TimesheetDetailProjectData>();
      }
      return result;
    }
    #endregion

    #region BaseServiceResource
    // không sử dụng với timesheet service
    public Task<long> Create( TimesheetResource resource, long creator )
    {
      return Task.FromResult( ( long ) 0 );
    }

    public async Task Update( TimesheetResource resource, long editor )
    {
      // delete all record timesheet detail old: xóa những record do user khai báo
      await _timesheetDetailRepository.DeleteByCondition( t => t.TimesheetId == resource.Id && t.Type == ( int ) Enums.TimesheetDetailType.Project );
      if ( resource.Projects != null && resource.Projects.Count > 0 ) {
        // create record timesheet detail new
        var tsDetails = new List<TimesheetDetail>();
        foreach ( var project in resource.Projects )
          tsDetails.AddRange( project.Tasks!.Select( t => new TimesheetDetail()
          {
            TimesheetId = resource.Id,
            Type = ( int ) Enums.TimesheetDetailType.Project,
            ProjectId = project.ProjectId,
            TimesheetCategoryId = t.CategoryId,
            Description = t.Description,
            Hours = t.HourValid,
            CreatedBy = editor,
            ModifiedBy = editor,
          } ) );
        await _timesheetDetailRepository.CreatesAsync( tsDetails );
      }
      // update timesheet report
      await ResetTimesheetReport( resource.Id, Enum.GetName( Enums.TimesheetDetailType.Project )! );
    }
    #endregion

    #region other
    public async Task LockTimesheet( DateTime start, DateTime end, long userId, bool isLock = true )
    {
      var timesheets = await _timesheetRepository.GetListByCondition( t => t.Date >= start && t.Date <= end );
      if ( timesheets.Any() ) {
        foreach ( var timesheet in timesheets ) {
          timesheet.LockBy = isLock ? userId : null;
        }
        _timesheetRepository.Updates( timesheets.ToArray() );
      }
    }

    public async Task<Timesheet?> GetTimesheet( long id )
    {
      return await _timesheetRepository.GetTimesheetDetailById( id );
    }
    public async Task ApplyLeaveRequestApproved( LeaveRequest request )
    {
      foreach ( var requestDetail in request.LeaveRequestDetails ) {
        // get timesheet
        var ts = await _timesheetRepository.GetTimesheetDetail( t => t.UserId == request.CreatedBy && t.Date == requestDetail.Date );
        // define timesheet
        var tsDetail = new TimesheetDetail()
        {
          Type = request.Type == ( int ) Enums.LeaveType.SelfMaternity || request.Type == ( int ) Enums.LeaveType.FamilyMaternity || request.Type == ( int ) Enums.LeaveType.Unpaid ?
            ( int ) Enums.TimesheetDetailType.UnpaidLeave : ( int ) Enums.TimesheetDetailType.PaidLeave,
          TimesheetId = ts!.Id,
          ProjectId = request.Type == ( int ) Enums.LeaveType.SelfMaternity || request.Type == ( int ) Enums.LeaveType.FamilyMaternity || request.Type == ( int ) Enums.LeaveType.Unpaid ?
            ( int ) Enums.TimesheetReportUnpaidLeave : ( int ) Enums.TimesheetReportPaidLeave,
          ReferenceId = request.Id,
          Description = request.Reason,
          Hours = requestDetail.Hours,
          CreatedBy = request.CreatedBy,
          ModifiedBy = request.CreatedBy,
        };
        await _timesheetDetailRepository.CreateAsync( tsDetail );
        // update timesheet report
        await ResetTimesheetReport( ts.Id, request.Type == ( int ) Enums.LeaveType.SelfMaternity || request.Type == ( int ) Enums.LeaveType.FamilyMaternity || request.Type == ( int ) Enums.LeaveType.Unpaid ?
          Enum.GetName( Enums.TimesheetDetailType.UnpaidLeave )! : Enum.GetName( Enums.TimesheetDetailType.PaidLeave )! );
      }
    }

    public async Task RemoveLeaveRequestReject( LeaveRequest request )
    {
      foreach ( var requestDetail in request.LeaveRequestDetails ) {
        // get timesheet
        var ts = await _timesheetRepository.GetTimesheetDetail( t => t.UserId == request.CreatedBy && t.Date == requestDetail.Date );
        // delete timesheet detail 
        await _timesheetDetailRepository.DeleteByCondition( t => t.TimesheetId == ts!.Id && t.ReferenceId == request.Id );
        // update timesheet report
        await ResetTimesheetReport( ts!.Id, request.Type == ( int ) Enums.LeaveType.SelfMaternity || request.Type == ( int ) Enums.LeaveType.FamilyMaternity || request.Type == ( int ) Enums.LeaveType.Unpaid ?
          Enum.GetName( Enums.TimesheetDetailType.UnpaidLeave )! : Enum.GetName( Enums.TimesheetDetailType.PaidLeave )! );
      }
    }

    public async Task ApplyWfhRequestApproved( WfhRequest request )
    {
      var records = await _timesheetRepository.GetListByCondition( t => t.UserId == request.CreatedBy && request.StartTime <= t.Date && t.Date <= request.EndTime );
      if ( records.Any() ) {
        foreach ( var timesheet in records ) {
          timesheet.WfhRequestId = request.Id;
          _timesheetRepository.Update( timesheet );
          // update timesheet report
          await ResetTimesheetReport( timesheet.Id );
        }
      }
    }

    public async Task RemoveWfhRequestReject( WfhRequest request )
    {
      var records = await _timesheetRepository.GetTimesheets( t => t.UserId == request.CreatedBy && request.StartTime <= t.Date && t.Date <= request.EndTime );
      if ( records.Any() ) {
        foreach ( var timesheet in records ) {
          timesheet.WfhRequestId = null;
          _timesheetRepository.Update( timesheet );
          // update timesheet report
          await ResetTimesheetReport( timesheet.Id );
        }
      }
    }
    /// <summary>
    /// tổng hợp lại thông tin timesheet report theo project
    /// </summary>
    /// <param name="timesheetId"></param>
    /// <param name="type">Enums.TimesheetType.leave/ Enums.TimesheetType.holiday/ other</param>
    /// <returns></returns>
    private async Task ResetTimesheetReport( long timesheetId, string? type = "" )
    {
      if ( string.IsNullOrEmpty( type ) )
        return;
      // delete old record
      if ( type == Enum.GetName( Enums.TimesheetDetailType.PaidLeave )! ) {
        await _timesheetReportRepository.DeleteByCondition( t => t.TimesheetId == timesheetId && t.ProjectId == Enums.TimesheetReportPaidLeave );
      }
      else if ( type == Enum.GetName( Enums.TimesheetDetailType.UnpaidLeave )! ) {
        await _timesheetReportRepository.DeleteByCondition( t => t.TimesheetId == timesheetId && t.ProjectId == Enums.TimesheetReportUnpaidLeave );
      }
      else {
        await _timesheetReportRepository.DeleteByCondition( t => t.TimesheetId == timesheetId && t.ProjectId > 0 );
      }
      // create new record
      var tsDetails =
        type == Enum.GetName( Enums.TimesheetDetailType.PaidLeave )! ? await _timesheetDetailRepository.GetListByCondition( t => t.TimesheetId == timesheetId && t.ProjectId == Enums.TimesheetReportPaidLeave ) :
        type == Enum.GetName( Enums.TimesheetDetailType.UnpaidLeave )! ? await _timesheetDetailRepository.GetListByCondition( t => t.TimesheetId == timesheetId && t.ProjectId == Enums.TimesheetReportUnpaidLeave ) :
        await _timesheetDetailRepository.GetListByCondition( t => t.TimesheetId == timesheetId && t.ProjectId > 0 );
      if ( tsDetails.Any() ) {
        var tsReportNew =
          type == Enum.GetName( Enums.TimesheetDetailType.PaidLeave ) || type == Enum.GetName( Enums.TimesheetDetailType.UnpaidLeave ) ?
          tsDetails.GroupBy( t => t.ProjectId ).Select( t => new TimesheetReport()
          {
            TimesheetId = timesheetId,
            ProjectId = t.Key,
            Hours = t.Sum( t => t.Hours ),
          } ) :
          tsDetails.GroupBy( t => t.ProjectId ).Select( t => new TimesheetReport()
          {
            TimesheetId = timesheetId,
            ProjectId = t.Key,
            Hours = t.Where( t => t.Type == ( int ) Enums.TimesheetDetailType.Project ).Sum( t => t.Hours )
          } );
        await _timesheetReportRepository.CreatesAsync( tsReportNew );
      }
    }

    public async Task<bool> CheckTimesheetHadLock( DateTime date, DateTime? endDate = null )
    {
      if ( endDate is null )
        return await _timesheetRepository.Exist( t => t.Date == date && t.LockBy != null );
      return await _timesheetRepository.Exist( t => date <= t.Date && t.Date <= endDate && t.LockBy != null );
    }

    public async Task<DashboardTimesheet> GetDashboardTimesheet( long userId )
    {
      var ts = await _timesheetRepository.GetTimesheet( t => t.UserId == userId && t.Date == DateTime.UtcNow.Date );
      return new DashboardTimesheet()
      {
        Today = DateTime.UtcNow.Date,
        TimeIn = ts == null ? Enums.TimeCheckDefault : ts.WfhRequestId > 0 ? Enums.TimeWfhCheckInDefault :
          ts.FingerPrinter.SwapDayRefer != null ? ( ts.FingerPrinter.SwapDayRefer.DateIn == ts.FingerPrinter.SwapDayRefer.DateIn.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.SwapDayRefer.DateIn.ToString( Enums.HOUR_FORMAT ) ) :
          ( ts.FingerPrinter.DateIn == ts.FingerPrinter.DateIn.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.DateIn.ToString( Enums.HOUR_FORMAT ) ),
        TimeOut = ts == null ? Enums.TimeCheckDefault : ts.WfhRequestId > 0 ? Enums.TimeWfhCheckOutDefault :
          ts.FingerPrinter.SwapDayRefer != null ? ( ts.FingerPrinter.SwapDayRefer.DateOut == ts.FingerPrinter.SwapDayRefer.DateOut.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.SwapDayRefer.DateOut.ToString( Enums.HOUR_FORMAT ) ) :
          ( ts.FingerPrinter.DateOut == ts.FingerPrinter.DateOut.Date ? Enums.TimeCheckDefault : ts.FingerPrinter.DateOut.ToString( Enums.HOUR_FORMAT ) ),
      };
    }
    public async Task<DashboardTime> GetDashboardTime( long userId, DateTime date )
    {
      var result = new DashboardTime();
      // get timesheet
      var timesheets = await _timesheetRepository.GetTimesheetDetails( t => t.UserId == userId && t.Date.Year == date.Year && t.Date.Month == date.Month );
      if ( !timesheets.Any() ) {
        return result;
      }
      // get working hours
      var tsDetail = timesheets.SelectMany( t => t.TimesheetDetails ).GroupBy( t => t.Type ).ToDictionary( key => key.Key, val => val.Sum( t => t.Hours ) );
      result.WorkLog = tsDetail.ContainsKey( ( int ) Enums.TimesheetDetailType.Project ) ? tsDetail [ ( int ) Enums.TimesheetDetailType.Project ] : 0;
      result.Holiday = timesheets.Where( t => !string.IsNullOrEmpty( t.HolidayName ) ).Count() * 8;
      result.UnpaidLeave = tsDetail.ContainsKey( ( int ) Enums.TimesheetDetailType.UnpaidLeave ) ? tsDetail [ ( int ) Enums.TimesheetDetailType.UnpaidLeave ] : 0;
      result.PaidLeave = tsDetail.ContainsKey( ( int ) Enums.TimesheetDetailType.PaidLeave ) ? tsDetail [ ( int ) Enums.TimesheetDetailType.PaidLeave ] : 0;
      //result.WorkingDay = Common.FormatHourInputToData( ( result.WorkLog + result.UnpaidLeave + result.PaidLeave + result.Holiday ) / 8 );
      result.WorkingDay = result.WorkLog + result.UnpaidLeave + result.PaidLeave + result.Holiday;
      return result;
    }
    public async Task<DashboardProject> GetDashboardProject( long userId, DateTime month )
    {
      //var timesheets = await _timesheetRepository.GetList( userId, month.Year, month.Month );
      var result = new DashboardProject();
      //var tsReports = timesheets.SelectMany( t => t.TimesheetReports ).ToList();
      var tsReports = await _timesheetReportRepository.GetListByCondition( t => t.Timesheet.UserId == userId && t.Timesheet.Date.Year == month.Year && t.Timesheet.Date.Month == month.Month );
      // get working hours
      result.WorkingHour = tsReports.Where( t => t.ProjectId != Enums.TimesheetReportPaidLeave && t.ProjectId != Enums.TimesheetReportUnpaidLeave ).Sum( t => t.Hours );
      result.LeaveHour = tsReports.Where( t => t.ProjectId == Enums.TimesheetReportPaidLeave ).Sum( t => t.Hours );
      result.Projects = new HashSet<DashboardProjectDetail>();
      // get projects
      var projects = tsReports.Where( t => t.ProjectId != Enums.TimesheetReportPaidLeave && t.ProjectId != Enums.TimesheetReportUnpaidLeave ).GroupBy( t => t.ProjectId )
        .ToDictionary( key => key.Key, value => value.Sum( t => t.Hours ) );
      foreach ( var projectId in projects.Keys ) {
        var project = await _projectRepository.GetByConditionNoTrack( p => p.Id == projectId );
        result.Projects.Add( new DashboardProjectDetail()
        {
          Name = Common.FormatProjectName( project! ),
          WorkingHours = projects [ projectId ]
        } );
      }
      return result;
    }

    public async Task<IEnumerable<TimesheetReportDetail>> GetReport( long? teamId, DateTime start, DateTime end )
    {
      var users = await _userRepository.GetUsersDataByCondition( teamId );
      users = users.Where( u => u.IsActive && u.Id != Enums.SYSTEM_ID ).OrderBy( u => u.UserName ).ToArray();
      var reports = new HashSet<TimesheetReportDetail>();
      foreach ( var user in users ) {
        var timesheets = await _timesheetRepository.GetTimesheetDetails( t => t.UserId == user.Id && t.Date >= start && t.Date < end );
        // set summary hour
        var tsDetail = timesheets.SelectMany( t => t.TimesheetDetails ).GroupBy( t => t.Type ).ToDictionary( key => key.Key, val => val.Sum( t => t.Hours ) );
        //var report = await _timesheetRepository.GetReport( user.Id, start, end );
        var record = new TimesheetReportDetail()
        {
          UserId = user.Id,
          FullName = user.FullName,
          UserName = user.UserName,
          Teams = user.TeamUsers.Select( tu => new TeamUserOption()
          {
            TeamId = tu.Team.Id,
            TeamCode = tu.Team.Code,
            TeamName = tu.Team.Name
          } ).ToList(),
          WorkingDay = 0,
          WorkLog = tsDetail.ContainsKey( ( int ) Enums.TimesheetDetailType.Project ) ? tsDetail [ ( int ) Enums.TimesheetDetailType.Project ] : 0,
          Holiday = timesheets.Where( t => !string.IsNullOrEmpty( t.HolidayName ) ).Count() * 8,
          UnpaidLeave = tsDetail.ContainsKey( ( int ) Enums.TimesheetDetailType.UnpaidLeave ) ? tsDetail [ ( int ) Enums.TimesheetDetailType.UnpaidLeave ] : 0,
          PaidLeave = tsDetail.ContainsKey( ( int ) Enums.TimesheetDetailType.PaidLeave ) ? tsDetail [ ( int ) Enums.TimesheetDetailType.PaidLeave ] : 0,
        };
        var dayWorking = timesheets.Where( t => t.TimesheetDetails.Any( td => td.Type == ( int ) Enums.TimesheetDetailType.Project ) ).DistinctBy( x => x.Date ).Count();
        record.WorkingDay = dayWorking;
        reports.Add( record );
      }
      return reports;
    }

    public async Task<byte []?> ExportSummaryTimesheet( long? teamId, DateTime startFilter, DateTime endFilter, long []? teamIds )
    {
      // get list user
      var users = await _userRepository.GetUsersDataByCondition( teamId );
      users = users.Where( u => u.IsActive && u.Id != Enums.SYSTEM_ID ).OrderBy( u => u.UserName ).ToArray();
      // set data report
      byte [] workbookBytes;
      var pathFileTemp = Path.Combine( _env.WebRootPath, "Excel", "VCI.HRMTIMESHEET.xlsx" );

      using ( MemoryStream templateStream = new() ) {
        using ( FileStream fileStream = new( pathFileTemp, FileMode.Open, FileAccess.Read ) ) {
          fileStream.CopyTo( templateStream );
          templateStream.Position = 0;
        }

        var workBook = new XLWorkbook( templateStream );
        var workSheet = workBook.Worksheet( "VCI.HRM" );
        var config = await _configRepository.GetByCondition( x => x.Description == DateTime.UtcNow.Month.ToString() );

        var totalWorkingLogs = ( config == null || config.Code == "0" ) ?
                                 Common.NetworkDays( new DateTime( startFilter.Year, startFilter.Month, 1 ),
                                    new DateTime( startFilter.Year, startFilter.Month,
                                    DateTime.DaysInMonth( startFilter.Year, startFilter.Month ) ) )
                                 : int.Parse( config!.Code );

        // set header
        var header = $"BẢNG TỔNG KẾT NGÀY/GIỜ CÔNG TỪ {startFilter.ToString( Enums.DATE_FORMAT )} ĐẾN {endFilter.ToString( Enums.DATE_FORMAT )}";
        FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"B2" ), header, false, string.Empty, string.Empty, null );
        //FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"E3" ), $"{totalWorkingLogs}/{DateTime.DaysInMonth( startFilter.Year, startFilter.Month )}", false, string.Empty, string.Empty, null );
        workSheet.Cell( $"E3" ).Value = $"{totalWorkingLogs}/{DateTime.DaysInMonth( startFilter.Year, startFilter.Month )}";

        // set cell value
        int rowIndex = 0;
        long teamIdCheck = 0;
        string backgroundColorCheck = "#DBE5E0";
        users = await SortedUserByTeam( users.ToList() );
        foreach ( var user in users.DistinctBy( x => x.Id ) ) {

          if ( teamIdCheck == 0 || user.TeamUsers.Any( x => x.TeamId != teamIdCheck ) ) {
            teamIdCheck = user.TeamUsers.First()!.TeamId;
            backgroundColorCheck = backgroundColorCheck == "#FFFFFF" ? "#DBE5E0" : "#FFFFFF";
          }
          // set STT
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"B{rowIndex + 9}" ), rowIndex + 1, false, string.Empty, backgroundColorCheck, null );
          // set staff ID
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"C{rowIndex + 9}" ), user.StaffId, false, string.Empty, backgroundColorCheck, null );
          // set fullname
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"D{rowIndex + 9}" ), user.FullName, false, string.Empty, backgroundColorCheck, null );
          // set team code
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"E{rowIndex + 9}" ), string.Join( ",", user.TeamUsers.Select( x => x.Team.Code ) ), false, string.Empty, backgroundColorCheck, null );
          // get data timesheet 
          var timesheets = await _timesheetRepository.GetTimesheetDetails( t => t.UserId == user.Id && t.Date >= startFilter && t.Date <= endFilter );
          var tsDetail = timesheets.SelectMany( t => t.TimesheetDetails ).GroupBy( t => t.Type ).ToDictionary( key => key.Key, val => val.Sum( t => t.Hours ) );

          //ngày công tiêu chuẩn
          var dayWorking = totalWorkingLogs;

          //chấm công
          var fg = timesheets.Select( x => x.FingerPrinter ).Where( x => x.DateIn.TimeOfDay != TimeSpan.Zero && x.DateOut.TimeOfDay != TimeSpan.Zero );
          var fgLate = timesheets.Select( x => x.FingerPrinter ).Where( x => x.DateIn.Hour >= 8 && x.DateIn.Minute > Enums.MAXIUM_FINGURE_LATE );


          //leave
          var leaveRequests = await _leaveRequestRepository.GetLeaveRequestDetails( l => l.CreatedBy == user.Id &&
            l.Status == ( int ) Enums.LeaveRequestStatus.Approve &&
            ( l.StartDate.Date >= startFilter.Date && l.StartDate.Date <= endFilter.Date ) );

          var leaveRequestDetailsByType = leaveRequests
            .SelectMany( l => l.LeaveRequestDetails.Select( d => new { l.Type, Detail = d } ) )
            .GroupBy( x => x.Type )
            .ToDictionary(
                g => g.Key,
                g => Math.Round( g.Sum( x => x.Detail.Hours ) / 8, 1 )
            );
          var leaveAnnual = Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.Annual );
          var leaveBereavement = Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.FamilyBereavement )
                                + Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.RelativeBereavement );
          var leaveWedding = Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.SelfWedding )
                                + Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.FamilyWedding );
          var leaveMaternity = Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.SelfMaternity )
                                + Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.FamilyMaternity );
          var totalLeave = leaveAnnual + leaveBereavement + leaveWedding + leaveMaternity;

          var leavUnpaid = Math.Round( tsDetail.ContainsKey( ( int ) Enums.TimesheetDetailType.UnpaidLeave ) ? tsDetail [ ( int ) Enums.TimesheetDetailType.UnpaidLeave ] / 8 : 0, 3 );

          //ngày công thực tế
          var dayReal = CalWorkingLog( timesheets, startFilter, endFilter );

          //trạng thái 
          var sttFg = fg.Count() >= dayWorking ? "OK" : "NG";
          var sttDayWorking = CheckStatusTimeSheet( timesheets, startFilter, endFilter ) ? "OK" : "NG";

          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"F{rowIndex + 9}" ), dayWorking != 0 ? dayWorking : string.Empty, false, string.Empty, backgroundColorCheck, null );

          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"G{rowIndex + 9}" ), fg.Count() != 0 ? fg.Count() : string.Empty, false, string.Empty, backgroundColorCheck, null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"H{rowIndex + 9}" ), fgLate.Count() != 0 ? fgLate.Count() : string.Empty, false, string.Empty, backgroundColorCheck, null );

          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"I{rowIndex + 9}" ), leaveAnnual != 0 ? leaveAnnual : string.Empty, false, string.Empty, backgroundColorCheck, null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"J{rowIndex + 9}" ), leaveBereavement != 0 ? leaveBereavement : string.Empty, false, string.Empty, backgroundColorCheck, null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"K{rowIndex + 9}" ), leaveWedding != 0 ? leaveWedding : string.Empty, false, string.Empty, backgroundColorCheck, null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"L{rowIndex + 9}" ), leaveMaternity != 0 ? leaveMaternity : string.Empty, false, string.Empty, backgroundColorCheck, null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"M{rowIndex + 9}" ), totalLeave != 0 ? totalLeave : string.Empty, false, string.Empty, backgroundColorCheck, null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"N{rowIndex + 9}" ), leavUnpaid != 0 ? leavUnpaid : string.Empty, false, string.Empty, backgroundColorCheck, null );

          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"O{rowIndex + 9}" ), dayReal != 0 ? dayReal : string.Empty, false, string.Empty, backgroundColorCheck, null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"P{rowIndex + 9}" ), fg.Count() != 0 ? sttFg : string.Empty, false, string.Empty, backgroundColorCheck, null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"Q{rowIndex + 9}" ), sttDayWorking, false, string.Empty, backgroundColorCheck, null );
          var rangeG = workSheet.Range( "G5:G100" );
          rangeG.AddConditionalFormat()
                .WhenIsTrue( "G5<F5" )
                .Font.SetFontColor( XLColor.Red );

          // Ví dụ: P và Q = "NG" (tô đỏ nền)
          var rangePQ = workSheet.Range( "P5:Q100" );
          rangePQ.AddConditionalFormat()
                .WhenEquals( "NG" )
                .Font.SetFontColor( XLColor.Red );

          rowIndex++;
        }
        workBook.Save();

        if ( teamIds != null && teamIds.Length > 0 ) {
          int index = 1;
          foreach ( var id in teamIds ) {
            users = await _userRepository.GetUsersDataByCondition( id );
            users = users.Where( u => u.IsActive && u.Id != Enums.SYSTEM_ID ).OrderBy( u => u.UserName ).ToArray();
            workSheet = workBook.AddWorksheet( $"VCI.HRM{index + 1}" );
            // set header
            header = $"BẢNG TỔNG KẾT NGÀY/GIỜ CÔNG TỪ {startFilter.ToString( Enums.DATE_FORMAT )} ĐẾN {endFilter.ToString( Enums.DATE_FORMAT )}";
            FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"B2" ), header, false, string.Empty, string.Empty, null );
            //FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"E3" ), $"{totalWorkingLogs}/{DateTime.DaysInMonth( startFilter.Year, startFilter.Month )}", false, string.Empty, string.Empty, null );
            workSheet.Cell( $"E3" ).Value = $"{totalWorkingLogs}/{DateTime.DaysInMonth( startFilter.Year, startFilter.Month )}";
            // set cell value
            rowIndex = 0;
            teamIdCheck = 0;
            backgroundColorCheck = "#DBE5E0";
            users = await SortedUserByTeam( users.ToList() );
            foreach ( var user in users ) {
              if ( teamIdCheck == 0 || user.TeamUsers.Any( x => x.TeamId != teamIdCheck ) ) {
                teamIdCheck = user.TeamUsers.First()!.TeamId;
                backgroundColorCheck = backgroundColorCheck == "#FFFFFF" ? "#DBE5E0" : "#FFFFFF";
              }
              // set STT
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"B{rowIndex + 9}" ), rowIndex + 1, false, string.Empty, backgroundColorCheck, null );
              // set staff ID
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"C{rowIndex + 9}" ), user.StaffId, false, string.Empty, backgroundColorCheck, null );
              // set fullname
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"D{rowIndex + 9}" ), user.FullName, false, string.Empty, backgroundColorCheck, null );
              // set team code
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"E{rowIndex + 9}" ), string.Join( ",", user.TeamUsers.Select( x => x.Team.Code ) ), false, string.Empty, backgroundColorCheck, null );
              // get data timesheet 
              var timesheets = await _timesheetRepository.GetTimesheetDetails( t => t.UserId == user.Id && t.Date >= startFilter && t.Date <= endFilter );
              var tsDetail = timesheets.SelectMany( t => t.TimesheetDetails ).GroupBy( t => t.Type ).ToDictionary( key => key.Key, val => val.Sum( t => t.Hours ) );

              //ngày công tiêu chuẩn
              var dayWorking = totalWorkingLogs;

              //chấm công
              var fg = timesheets.Select( x => x.FingerPrinter ).Where( x => x.DateIn.TimeOfDay != TimeSpan.Zero && x.DateOut.TimeOfDay != TimeSpan.Zero );
              var fgLate = timesheets.Select( x => x.FingerPrinter ).Where( x => x.DateIn.Hour >= 8 && x.DateIn.Minute > Enums.MAXIUM_FINGURE_LATE );


              //leave
              var leaveRequests = await _leaveRequestRepository.GetLeaveRequestDetails( l => l.CreatedBy == user.Id &&
                l.Status == ( int ) Enums.LeaveRequestStatus.Approve &&
                ( l.StartDate.Date >= startFilter.Date && l.StartDate.Date <= endFilter.Date ) );

              var leaveRequestDetailsByType = leaveRequests
                .SelectMany( l => l.LeaveRequestDetails.Select( d => new { l.Type, Detail = d } ) )
                .GroupBy( x => x.Type )
                .ToDictionary(
                    g => g.Key,
                    g => Math.Round( g.Sum( x => x.Detail.Hours ) / 8, 1 )
                );
              var leaveAnnual = Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.Annual );
              var leaveBereavement = Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.FamilyBereavement )
                                    + Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.RelativeBereavement );
              var leaveWedding = Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.SelfWedding )
                                    + Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.FamilyWedding );
              var leaveMaternity = Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.SelfMaternity )
                                    + Common.GetValue( leaveRequestDetailsByType, ( int ) Enums.LeaveType.FamilyMaternity );
              var totalLeave = leaveAnnual + leaveBereavement + leaveWedding + leaveMaternity;

              var leavUnpaid = Math.Round( tsDetail.ContainsKey( ( int ) Enums.TimesheetDetailType.UnpaidLeave ) ? tsDetail [ ( int ) Enums.TimesheetDetailType.UnpaidLeave ] / 8 : 0, 3 );

              //ngày công thực tế
              var dayReal = CalWorkingLog( timesheets, startFilter, endFilter );

              //trạng thái 
              var sttFg = fg.Count() > dayWorking ? "OK" : "NG";
              var sttDayWorking = CheckStatusTimeSheet( timesheets, startFilter, endFilter ) ? "OK" : "NG";

              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"F{rowIndex + 9}" ), dayWorking != 0 ? dayWorking : string.Empty, false, string.Empty, backgroundColorCheck, null );

              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"G{rowIndex + 9}" ), fg.Count() != 0 ? fg.Count() : string.Empty, false, string.Empty, backgroundColorCheck, null );
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"H{rowIndex + 9}" ), fgLate.Count() != 0 ? fgLate.Count() : string.Empty, false, string.Empty, backgroundColorCheck, null );

              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"I{rowIndex + 9}" ), leaveAnnual != 0 ? leaveAnnual : string.Empty, false, string.Empty, backgroundColorCheck, null );
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"J{rowIndex + 9}" ), leaveBereavement != 0 ? leaveBereavement : string.Empty, false, string.Empty, backgroundColorCheck, null );
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"K{rowIndex + 9}" ), leaveWedding != 0 ? leaveWedding : string.Empty, false, string.Empty, backgroundColorCheck, null );
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"L{rowIndex + 9}" ), leaveMaternity != 0 ? leaveMaternity : string.Empty, false, string.Empty, backgroundColorCheck, null );
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"M{rowIndex + 9}" ), totalLeave != 0 ? totalLeave : string.Empty, false, string.Empty, backgroundColorCheck, null );
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"N{rowIndex + 9}" ), leavUnpaid != 0 ? leavUnpaid : string.Empty, false, string.Empty, backgroundColorCheck, null );

              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"O{rowIndex + 9}" ), dayReal != 0 ? dayReal : string.Empty, false, string.Empty, backgroundColorCheck, null );
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"P{rowIndex + 9}" ), fg.Count() != 0 ? sttFg : string.Empty, false, string.Empty, backgroundColorCheck, null );
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( $"Q{rowIndex + 9}" ), sttDayWorking, false, string.Empty, backgroundColorCheck, null );
              var rangeG = workSheet.Range( "G5:G100" );
              rangeG.AddConditionalFormat()
                    .WhenIsTrue( "G5<F5" )
                    .Font.SetFontColor( XLColor.Red );

              // Ví dụ: P và Q = "NG" (tô đỏ nền)
              var rangePQ = workSheet.Range( "P5:Q100" );
              rangePQ.AddConditionalFormat()
                    .WhenEquals( "NG" )
                    .Font.SetFontColor( XLColor.Red );

              rowIndex++;

            }
            workBook.Save();
          }
        }
        workbookBytes = templateStream.ToArray();
      }
      return workbookBytes;
    }

    private double CalWorkingLog( IEnumerable<Timesheet> timesheets, DateTime startDay, DateTime endDay )
    {
      double totalWorkingLog = 0.0;
      if ( endDay > DateTime.UtcNow )
        endDay = DateTime.UtcNow;
      for ( int i = startDay.Day; i <= endDay.Day; i++ ) {
        var workingLog = 0.0;
        var date = new DateTime( startDay.Year, startDay.Month, i );
        var timeSheet = timesheets.Where( t => t.Date == date );
        var tsDetails = timeSheet.SelectMany( t => t.TimesheetDetails ).GroupBy( t => t.Type ).ToDictionary( key => key.Key, val => val.Sum( t => t.Hours ) );
        var timeSheetUnPaidLeave = Common.GetValue( tsDetails, ( int ) Enums.TimesheetDetailType.UnpaidLeave );
        var timeSheetPaidLeave = Common.GetValue( tsDetails, ( int ) Enums.TimesheetDetailType.PaidLeave );
        var timeSheetProject = Common.GetValue( tsDetails, ( int ) Enums.TimesheetDetailType.Project );
        if ( timeSheetPaidLeave == 0.0 && timeSheetProject >= 4 ) {
          workingLog = 1.0 - timeSheetUnPaidLeave / 8.0;
        }
        else if ( timeSheetPaidLeave == 4.0 && timeSheetProject > 1 ) {
          workingLog = 0.5 - timeSheetUnPaidLeave / 8.0;
        }
        else if ( timeSheetPaidLeave == 0.0 && timeSheetProject > 1 ) {
          workingLog = 1 - timeSheetUnPaidLeave / 8.0;
        }
        else if ( timeSheetProject != 0 ) {
          workingLog = 1.0;
        }
        totalWorkingLog = Math.Round( totalWorkingLog + workingLog, 3 );
      }
      return totalWorkingLog;
    }

    private bool CheckStatusTimeSheet( IEnumerable<Timesheet> timesheets, DateTime startDay, DateTime endDay )
    {
      if ( endDay > DateTime.UtcNow )
        endDay = DateTime.UtcNow.AddDays( -1 );
      for ( int i = startDay.Day; i <= endDay.Day; i++ ) {
        var date = new DateTime( startDay.Year, startDay.Month, i );
        if ( date.Date.DayOfWeek == DayOfWeek.Sunday ) {
          continue;
        }
        var timeSheet = timesheets.Where( t => t.Date == date );
        var tsDetails = timeSheet.SelectMany( t => t.TimesheetDetails ).GroupBy( t => t.Type ).ToDictionary( key => key.Key, val => val.Sum( t => t.Hours ) );
        var timeSheetUnPaidLeave = Common.GetValue( tsDetails, ( int ) Enums.TimesheetDetailType.UnpaidLeave );
        var timeSheetPaidLeave = Common.GetValue( tsDetails, ( int ) Enums.TimesheetDetailType.PaidLeave );
        var timeSheetProject = Common.GetValue( tsDetails, ( int ) Enums.TimesheetDetailType.Project );

        if ( timeSheetUnPaidLeave == 0.0 && timeSheetPaidLeave == 0.0 && timeSheetProject == 0.0 ) {
          continue;
        }

        if ( timeSheetPaidLeave == 4.0 && timeSheetProject < 4 ) {
          return false;
        }
        else if ( timeSheetPaidLeave == 0.0 && ( timeSheetProject < 8 - timeSheetUnPaidLeave ) ) {
          return false;
        }
      }
      return true;
    }
    public async Task<byte []?> ExportSummaryTimesheetByTeam( long? teamId, DateTime startFilter, DateTime endFilter, long []? teamIds )
    {
      // get list user
      var users = await _userRepository.GetUsersDataByCondition( teamId );
      users = users.Where( u => u.IsActive && u.Id != Enums.SYSTEM_ID ).OrderBy( u => u.UserName ).ToArray();

      if ( teamIds != null && teamIds.Length > 0 ) {
        foreach ( var id in teamIds ) {
          var usersByTeam = await _userRepository.GetUsersDataByCondition( id );
          users.Union( usersByTeam );
        }
      }
      // get data timesheet 
      var timesheets = new List<Timesheet>();
      foreach ( var user in users ) {
        var timesheetSelect = await _timesheetRepository.GetTimesheetDetails( t => t.UserId == user.Id && t.Date >= startFilter && t.Date <= endFilter );
        timesheets.AddRange( timesheetSelect );
      }

      byte [] workbookBytes;
      var pathFileTemp = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Excel", "SummaryOfTimesheetByTeam.xlsx" );
      using ( MemoryStream templateStream = new() ) {
        using ( FileStream fileStream = new( pathFileTemp, FileMode.Open, FileAccess.Read ) ) {
          fileStream.CopyTo( templateStream );
          templateStream.Position = 0;
        }
        var workBook = new XLWorkbook( templateStream );
        var workSheetDetail = workBook.Worksheet( "Sheet1" );
        var workSheetTotal = workBook.Worksheet( "Sheet2" );
        var rowIndexSheetDetail = 1;
        var rowIndexSheetTotal = 1;
        var projects = new Dictionary<long, string>();

        foreach ( var element in ( await GroupByTeam( users ) ) ) {
          #region sheet detail
          // set header Project
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetDetail.Cell( $"A{rowIndexSheetDetail}" ), "Project", false, "#000000", "#BCD6ED", null );
          // set header Division
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetDetail.Cell( $"B{rowIndexSheetDetail}" ), "Division", false, "#000000", "#BCD6ED", null );
          // set header users
          string columnIndex = "C";
          foreach ( var user in element.Value ) {
            FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetDetail.Cell( $"{columnIndex}{rowIndexSheetDetail}" ), user.FullName, false, "#000000", "#BCD6ED", null );
            columnIndex = GetNextColumn( columnIndex );
          }
          // set header total
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetDetail.Cell( $"{columnIndex}{rowIndexSheetDetail}" ), "Total", false, "#000000", "#BCD6ED", null );
          #endregion

          // set row value
          var projectGroup = timesheets.Where( t => element.Value.Select( u => u.Id ).Contains( t.UserId ) ) // {key (project ID): value {key (user ID): value (hours)}}
            .SelectMany( t => t.TimesheetDetails ).Where( t => t.ProjectId > 0 ).Select( t => new { t.ProjectId, t.Hours, t.Timesheet.UserId } )
            .GroupBy( t => t.ProjectId ).ToDictionary( key => key.Key, val => val.GroupBy( t => t.UserId ).ToDictionary( k => k.Key, v => v.Sum( t => t.Hours ) ) ).OrderBy( t => t.Key );
          var teamCode = element.Value.First().TeamUsers.First().Team.Code;
          #region sheet total
          // set header No
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetTotal.Cell( $"A{rowIndexSheetTotal}" ), "No.", false, string.Empty, "#FFFF00", XLAlignmentHorizontalValues.Center );
          // set header team
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetTotal.Cell( $"B{rowIndexSheetTotal}" ), $"Project-{teamCode}", false, string.Empty, "#FFFF00", XLAlignmentHorizontalValues.Center );
          // set header WH
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetTotal.Cell( $"C{rowIndexSheetTotal}" ), "WH", false, string.Empty, "#FFFF00", XLAlignmentHorizontalValues.Center );
          #endregion
          rowIndexSheetDetail += 1;
          rowIndexSheetTotal += 1;
          var cellNo = 1;
          foreach ( var ts in projectGroup ) {
            columnIndex = "A";
            // set cell project name [Sheet Detail]
            if ( !projects.ContainsKey( ts.Key ) ) {
              var project = await _projectRepository.GetByConditionNoTrack( p => p.Id == ts.Key );
              projects.Add( ts.Key, Common.FormatProjectName( project! ) );
            }
            FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetDetail.Cell( $"{columnIndex}{rowIndexSheetDetail}" ), projects [ ts.Key ], false, "#000000", string.Empty, XLAlignmentHorizontalValues.Left );
            // set cell team [Sheet Detail]
            columnIndex = GetNextColumn( columnIndex );
            FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetDetail.Cell( $"{columnIndex}{rowIndexSheetDetail}" ), teamCode, false, "#000000", string.Empty, null );
            // set cell hour [Sheet Detail]
            columnIndex = GetNextColumn( columnIndex );
            foreach ( var user in element.Value ) {
              FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetDetail.Cell( $"{columnIndex}{rowIndexSheetDetail}" ), ts.Value.ContainsKey( user.Id ) ? ts.Value [ user.Id ] : string.Empty, false, string.Empty, string.Empty, null );
              columnIndex = GetNextColumn( columnIndex );
            }
            // set cell total [Sheet Detail]
            FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetDetail.Cell( $"{columnIndex}{rowIndexSheetDetail}" ), ts.Value.Values.Sum(), false, string.Empty, "#FFFF00", null );
            // set cell No [Sheet Total]
            FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetTotal.Cell( $"A{rowIndexSheetTotal}" ), cellNo, false, string.Empty, string.Empty, null );
            // set cell project [Sheet Total]
            FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetTotal.Cell( $"B{rowIndexSheetTotal}" ), projects [ ts.Key ], false, string.Empty, string.Empty, null );
            // set cell total [Sheet Total]
            FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheetTotal.Cell( $"C{rowIndexSheetTotal}" ), ts.Value.Values.Sum(), false, string.Empty, string.Empty, null );
            // next row [Sheet Detail]
            rowIndexSheetDetail += 1;
            rowIndexSheetTotal += 1;
            cellNo += 1;
          }
          rowIndexSheetDetail += 2;
          rowIndexSheetTotal += 1;
        }
        workSheetDetail.Name = "Detail";
        workSheetTotal.Name = "Total";
        workBook.Save();
        workbookBytes = templateStream.ToArray();
      }
      return workbookBytes;
    }

    private static string GetNextColumn( string previousName )
    {
      int endCharCode = 90;
      if ( previousName.Length == 1 ) {
        int previousCode = previousName [ 0 ];
        return previousCode < endCharCode ? $"{( char ) ( previousCode + 1 )}" : "AA";
      }
      else {
        int preCharCode1 = previousName [ 0 ];
        int preCharCode2 = previousName [ 1 ];
        return preCharCode2 < endCharCode ? $"{previousName [ 0 ]}{( char ) ( preCharCode2 + 1 )}" : $"{( char ) ( preCharCode1 + 1 )}A";
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cell">cell setup</param>
    /// <param name="value">giá trị hiển thị trong cell</param>
    /// <param name="isBold">true: in đậm</param>
    /// <param name="fontColor">string is not black, then setup</param>
    /// <param name="backgroundColor">string is not black, then setup</param>
    /// <param name="horizon">value is not null, then setup</param>
    private void FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( IXLCell cell, object value, bool isBold, string fontColor, string backgroundColor, XLAlignmentHorizontalValues? horizon )
    {
      cell.Value = value;
      cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
      if ( isBold )
        cell.Style.Font.Bold = isBold;
      if ( !string.IsNullOrEmpty( fontColor ) )
        cell.Style.Font.FontColor = XLColor.FromColor( ColorTranslator.FromHtml( fontColor ) );
      if ( !string.IsNullOrEmpty( backgroundColor ) )
        cell.Style.Fill.SetBackgroundColor( XLColor.FromColor( ColorTranslator.FromHtml( backgroundColor ) ) );
      if ( horizon is not null )
        cell.Style.Alignment.Horizontal = ( XLAlignmentHorizontalValues ) horizon;
    }

    public async Task<IEnumerable<SwapDayResponse>> SwapDayGetListView( int year )
    {
      var records = await _swapDayRepository.GetListData( s => s.FromDate.Year == year );
      var users = new Dictionary<long, string>();
      foreach ( var record in records ) {
        if ( users.ContainsKey( record.CreatedBy ) ) {
          record.CreatedByName = users [ record.CreatedBy ];
        }
        else {
          var user = await _userRepository.GetByConditionNoTrack( u => u.Id == record.CreatedBy );
          record.CreatedByName = user!.UserName;
          users.Add( record.CreatedBy, record.CreatedByName );
        }
      }
      return records.OrderBy( s => s.FromDate );
    }

    public async Task<long> SwapDayCreate( SwapDayResource resource, long creator )
    {
      var record = new SwapDay()
      {
        FromDate = resource.FromDate,
        ToDate = resource.ToDate,
        Description = resource.Description,
        CreatedBy = creator
      };
      if ( resource.Users != null && resource.Users.Any() ) {
        record.SwapDayUsers = resource.Users;
      }
      await _swapDayRepository.CreateAsync( record );
      // apply for timesheet
      await ApplySwapDayToTimesheet( record, true );
      return record.Id;
    }

    public async Task SwapDayUpdate( SwapDayResource resource, long modifier )
    {
      // get record SwapDay
      var record = await _swapDayRepository.GetData( s => s.Id == resource.Id );
      // remove apply for timesheet
      await ApplySwapDayToTimesheet( record!, false );
      // xóa hết quan hệ trong SwapDayUser
      await _swapDayUserRepository.DeleteByCondition( s => s.SwapDayId == resource.Id );

      // update SwapDay
      record!.FromDate = resource.FromDate;
      record.ToDate = resource.ToDate;
      record.Description = resource.Description;
      if ( resource.Users != null && resource.Users.Any() ) {
        record.SwapDayUsers = resource.Users;
        foreach ( var user in resource.Users ) {
          user.SwapDayId = ( long ) resource.Id!;
        }
      }
      _swapDayRepository.Update( record );
      // re-apply for timesheet
      await ApplySwapDayToTimesheet( record, true );
    }

    public async Task SwapDayDelete( long id )
    {
      var record = await _swapDayRepository.GetData( s => s.Id == id );
      if ( record is null )
        return;
      // apply for timesheet
      await ApplySwapDayToTimesheet( record, false );
      // xóa hết quan hệ trong SwapDayUser
      await _swapDayUserRepository.DeleteByCondition( s => s.SwapDayId == id );
      // xóa record SwapDay
      await _swapDayRepository.DeleteByCondition( s => s.Id == id );
    }

    public async Task<bool> ExistSwapDay( DateTime date, long userId )
    {
      return await _swapDayRepository.Exist( s => s.ToDate == date && !s.SwapDayUsers.Any() ) || await _swapDayUserRepository.Exist( s => s.UserId == userId && s.SwapDay.ToDate == date );
    }

    /// <summary>
    /// chuyển dữ liệu record hoán đổi ngày vào timesheet
    /// </summary>
    /// <param name="swapDay">record SwapDay, bao gồm dữ liệu SwapDayUser</param>
    /// <param name="isApply">true: áp dụng, false: thu hồi/xóa áp dụng</param>
    private async Task ApplySwapDayToTimesheet( SwapDay swapDay, bool isApply )
    {
      // get list timesheet
      var userIds = swapDay.SwapDayUsers == null || !swapDay.SwapDayUsers.Any() ? null : swapDay.SwapDayUsers.Select( s => s.UserId ).ToList();
      var timesheets = userIds == null ?
        await _timesheetRepository.GetTimesheets( t => t.Date == swapDay.FromDate ) :
        await _timesheetRepository.GetTimesheets( t => t.Date == swapDay.FromDate && userIds.Contains( t.UserId ) );
      var froms = timesheets.ToDictionary( k => k.UserId, v => v.FingerPrinter.Id ); // key: UserId, value: Finger Printer ID
      timesheets = userIds == null ?
        await _timesheetRepository.GetTimesheets( t => t.Date == swapDay.ToDate ) :
        await _timesheetRepository.GetTimesheets( t => t.Date == swapDay.ToDate && userIds.Contains( t.UserId ) );
      var tos = timesheets.ToDictionary( k => k.UserId, v => v ); // key: UserId, value: Timesheet record

      var refers = new HashSet<SwapDayRefer>();
      foreach ( var ts in tos ) {
        ts.Value.SwapDay = isApply ? swapDay.FromDate : null;
        if ( isApply ) {
          // tạo dữ liệu SwapDayRefer
          refers.Add( new SwapDayRefer()
          {
            DateIn = swapDay.FromDate.AddHours( ts.Value.FingerPrinter.DateIn.Hour ).AddMinutes( ts.Value.FingerPrinter.DateIn.Minute ),
            DateOut = swapDay.FromDate.AddHours( ts.Value.FingerPrinter.DateOut.Hour ).AddMinutes( ts.Value.FingerPrinter.DateOut.Minute ),
            SwapDayId = swapDay.Id,
            FingerPrinterId = froms [ ts.Key ],
            HourTotal = ts.Value.FingerPrinter.HourTotal,
          } );
        }
        else {
          refers.Add( ( await _swapDayReferRepository.GetByCondition( t => t.FingerPrinterId == froms [ ts.Key ] ) )! );
        }
      }
      // apply changer
      _timesheetRepository.Updates( tos.Values.ToArray() );
      if ( isApply ) {
        await _swapDayReferRepository.CreatesAsync( refers );
      }
      else {
        _swapDayReferRepository.Deletes( refers );
      }
    }

    public async Task<long []> GetTeamId( long id )
    {
      return await _timesheetRepository.GetTeamId( id );
    }

    public async Task<byte []?> ExportTimesheetByMonth( long year, long month )
    {
      byte [] workbookBytes = Array.Empty<byte>();
      var users = await _userRepository.GetUsersDataByCondition( null );
      users = users.Where( u => u.IsActive && u.Id != Enums.SYSTEM_ID ).OrderBy( u => u.UserName ).ToArray();
      var pathFileTemp = Path.Combine( _env.WebRootPath, "Excel", "TimesheetTemplate.xlsx" );
      var sources = new List<TimesheetByMonthResource>();

      foreach ( var user in users.OrderBy( u => u.StaffId ) ) {

        // danh sách timesheet trong tháng
        var timesheets = await _timesheetRepository.GetTimesheetDetails( t => t.UserId == user.Id && t.Date.Year == year && t.Date.Month == month );
        // danh sách leve request trong tháng
        var leaveRequests = await _leaveRequestRepository.GetLeaveRequestDetails( l => l.CreatedBy == user.Id &&
           l.Status == ( int ) Enums.LeaveRequestStatus.Approve &&
           ( l.StartDate.Year == year || l.EndDate.Year == year ) &&
           ( l.StartDate.Month == month && l.EndDate.Month == month ) );

        var leaveRequestDetails = leaveRequests
              .SelectMany( l => l.LeaveRequestDetails.Select( d => new { l.Type, Detail = d } ) )
              .GroupBy( x => x.Detail.Date.Month )
              .ToDictionary(
                  g => g.Key,
                  g => g.GroupBy( x => x.Type )
                        .ToDictionary(
                            tg => tg.Key,
                            tg => tg.Sum( x => x.Detail.Hours ) )
              );

        var leaveByMonth = await CalLeaveDayByMonth( ( int ) month, leaveRequestDetails );

        var projectId = timesheets
            .SelectMany( t => t.TimesheetDetails )
            .Where( td => td.ProjectId > 0 )
            .Select( td => td.ProjectId );

        var projects = await _projectRepository.GetListByConditionTrack( p => projectId.Contains( p.Id ) );
        var resource = new TimesheetByMonthResource
        {
          FullName = user.FullName,
          Projects = projects.Select( p => Common.FormatProjectName( p ) ).Distinct().ToList(),
          PaidLeave = leaveByMonth.Where( x => x.Type == Enum.GetName( typeof( Enums.LeaveType ), ( int ) Enums.LeaveType.Annual ) )
                  .Sum( x => x.Day ).ToString( "F1" ),
        };
        sources.Add( resource );
      }

      var projectAll = await _projectRepository.GetAll();
      var timeSheetData = new List<TimesheetWorkingByMonthResource>();
      foreach ( var project in projectAll.OrderBy( x => x.Id ) ) {
        var timesheets = await _timesheetRepository.GetTimesheetDetails( t => t.Date.Year == year && t.Date.Month == month );
        var usersInProject = timesheets
           .SelectMany( t => t.TimesheetDetails )
           .Where( td => td.ProjectId == project.Id )
           .Select( td => td.Timesheet.UserId )
           .Distinct()
           .ToList();

        foreach ( var userId in usersInProject ) {
          var user = await _userRepository.GetByConditionNoTrack( u => u.Id == userId );
          var workLogInProject = timesheets
              .Where( t => t.UserId == userId )
              .SelectMany( t => t.TimesheetDetails )
              .Where( td => td.ProjectId == project.Id )
              .Sum( td => td.Hours );

          timeSheetData.Add( new TimesheetWorkingByMonthResource
          {
            ProjectId = project.Id,
            FullName = user!.FullName,
            UserId = user.Id,
            ProjecName = Common.FormatProjectName( project ),
            WorkLog = workLogInProject.ToString( "F2" )
          } );
        }
      }

      using ( MemoryStream templateStream = new() ) {
        using ( FileStream fileStream = new( pathFileTemp, FileMode.Open, FileAccess.Read ) ) {
          fileStream.CopyTo( templateStream );
          templateStream.Position = 0;
        }

        var workBook = new XLWorkbook( templateStream );

        var workSheet = workBook.Worksheet( "Sheet1" );

        workSheet.Cell( 1, 1 ).Value = "Tên nhân viên";
        workSheet.Cell( 1, 2 ).Value = "Số ngày nghỉ (PaidLeave)";
        workSheet.Cell( 1, 3 ).Value = "Tên dự án";

        var headerRange2 = workSheet.Range( "A1:C1" );
        headerRange2.Style.Font.Bold = true;
        headerRange2.Style.Fill.BackgroundColor = XLColor.LightGreen;

        int rowIndex = 2;
        foreach ( var resource in sources ) {
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( rowIndex, 1 ), resource.FullName, false, string.Empty, "#FFFFFF", null );
          workSheet.Cell( rowIndex, 2 ).Style.NumberFormat.Format = "@";
          workSheet.Cell( rowIndex, 2 ).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( rowIndex, 2 ), resource.PaidLeave, false, string.Empty, "#FFFFFF", null );
          int index = 2;
          foreach ( var project in resource.Projects ) {
            FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet.Cell( rowIndex, ++index ), project, false, string.Empty, "#FFFFFF", null );
          }
          rowIndex++;
        }
        workSheet.Name = $"Leave_{month}_{year}";

        var workSheet2 = workBook.Worksheet( "Sheet2" );

        workSheet2.Cell( 1, 1 ).Value = "ID dự án";
        workSheet2.Cell( 1, 2 ).Value = "Tên dự án";
        workSheet2.Cell( 1, 3 ).Value = "ID nhân viên";
        workSheet2.Cell( 1, 4 ).Value = "Tên nhân viên";
        workSheet2.Cell( 1, 5 ).Value = "Giờ làm việc";

        var headerRange = workSheet2.Range( "A1:E1" );
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
        rowIndex = 2;
        foreach ( var item in timeSheetData ) {
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet2.Cell( rowIndex, 1 ), item.ProjectId, false, string.Empty, "#FFFFFF", null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet2.Cell( rowIndex, 2 ), item.ProjecName, false, string.Empty, "#FFFFFF", null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet2.Cell( rowIndex, 3 ), item.UserId, false, string.Empty, "#FFFFFF", null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet2.Cell( rowIndex, 4 ), item.FullName, false, string.Empty, "#FFFFFF", null );
          FileReportSummaryTimesheetByTeamGenerateHeaderTalbe( workSheet2.Cell( rowIndex, 5 ), item.WorkLog, false, string.Empty, "#FFFFFF", null );
          rowIndex++;
        }
        workSheet2.Name = $"WorkLog_{month}_{year}";

        workBook.Save();
        workbookBytes = templateStream.ToArray();
      }
      return workbookBytes;
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
    private async Task<List<User>> SortedUserByTeam( List<User> users )
    {
      var usersSorted = new List<User>();
      var teams = await _teamRepository.GetAll();
      teams = teams.OrderBy( t => t.Id ).ToList();
      foreach ( var team in teams ) {
        var usersInTeam = users.Where( u => u.TeamUsers.Any( tu => tu.TeamId == team.Id ) ).ToList();
        if ( usersInTeam.Any() ) {
          usersSorted.AddRange( usersInTeam.OrderBy( u => u.UserName ) );
        }
      }
      return usersSorted;
    }

    private async Task<Dictionary<long, List<User>>> GroupByTeam( IEnumerable<User> users )
    {
      var result = new Dictionary<long, List<User>>();
      var teams = await _teamRepository.GetAll();
      foreach ( var team in teams ) {
        var usersInTeam = users.Where( u => u.TeamUsers.Any( tu => tu.TeamId == team.Id ) ).ToList();
        if ( usersInTeam.Any() ) {
          result.Add( team.Id, usersInTeam );
        }
      }
      return result;
    }
    #endregion
  }
}
