using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers
{
  /// <summary>
  /// APIs của Timesheet Controller
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class TimesheetController : ControllerBase
  {
    private readonly ITimesheetService _timesheetService;
    private readonly IProjectService _projectService;
    private readonly ITeamService _teamService;
    private readonly IUserService _userService;
    private readonly ICategoryService _categoryService;
    private readonly IHolidayService _holidayService;
    private readonly ILogger<TimesheetController> _logger;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

    public TimesheetController( ITimesheetService timesheetService,
      IProjectService projectService,
      ITeamService teamService,
      IUserService userService,
      ICategoryService categoryService,
      IHolidayService holidayService,
      ILogger<TimesheetController> logger,
      IOptions<ApiBehaviorOptions> apiBehaviorOptions )
    {
      _timesheetService = timesheetService;
      _projectService = projectService;
      _teamService = teamService;
      _userService = userService;
      _categoryService = categoryService;
      _holidayService = holidayService;
      _logger = logger;
      _apiBehaviorOptions = apiBehaviorOptions;
    }

    /// <summary>
    /// [Personal - List] Lấy danh sách timesheet trong tháng
    /// </summary>
    /// <remarks>
    /// đây là danh sách thu gọn theo đơn vị tháng
    /// </remarks>
    /// <param name="year" example="2024">năm chỉ định</param>
    /// <param name="month" example="1">tháng chỉ định</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView/{year}/{month}" )]
    [Authorize( Policy = Policies.TIMESHEET_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<TimesheetResponse> ) )]
    public async Task<IActionResult> GetListView( long year, long month )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return Ok( await _timesheetService.GetList( new BaseFilter()
      {
        UserId = userLogin,
        DateCheck = datetimeTry,
        Year = year,
        Month = month
      } ) );
    }

    /// <summary>
    /// [Admin - List] Lấy danh sách timesheet trong tháng
    /// </summary>
    /// <remarks>
    /// quản lý theo dõi
    /// </remarks>
    /// <param name="year" example="2024">năm chỉ định</param>
    /// <param name="month" example="1">tháng chỉ định</param>
    /// <param name="userId" example="1"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView/{userId}/{year}/{month}" )]
    [Authorize( Policy = Policies.TIMESHEET_VIEW_OTHER )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<TimesheetResponse> ) )]
    public async Task<IActionResult> GetListView( long userId, long year, long month )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var teamUserLogin = ( long ) 0;
      var result = new List<TimesheetResponse>();
      if ( parts.Length > 1 && parts.Any( x => x != Enums.TEAM_HR.ToString() ) ) {
        foreach ( var p in parts ) {
          var teamId = long.Parse( p );
          var res1 = await _timesheetService.GetList( new BaseFilter()
          {
            UserId = userId,
            TeamId = !Common.ValidRoleAdmin( positionUserLogin ) && teamUserLogin != Enums.TEAM_HR ? teamUserLogin : null,
            DateCheck = datetimeTry,
            Year = year,
            Month = month
          } );
          result.AddRange( res1.ToList() );
        }
        return Ok( result );
      }
      else {
        teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
        return Ok( await _timesheetService.GetList( new BaseFilter()
        {
          UserId = userId,
          TeamId = !Common.ValidRoleAdmin( positionUserLogin ) && teamUserLogin != Enums.TEAM_HR ? teamUserLogin : null,
          DateCheck = datetimeTry,
          Year = year,
          Month = month
        } ) );
      }


    }

    /// <summary>
    /// [Personal - Detail] Lấy thông tin chi tiết task trong ngày
    /// </summary>
    /// <param name="year" example="2024">Timesheet year</param>
    /// <param name="month" example="1">Timesheet month</param>
    /// <param name="day" example="1">Timesheet day</param>
    /// <returns></returns>
    [HttpGet( "GetDetail/{year}/{month}/{day}" )]
    [Authorize( Policy = Policies.TIMESHEET_GET_DETAIL )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( TimesheetDetailResponse ) )]
    public async Task<IActionResult> GetDetail( int year, int month, int day )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/{day}", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // valid timesheet ID có phải của user không
      var ts = await _timesheetService.GetByCondition( t => t.Date == datetimeTry && t.UserId == userLogin );
      if ( ts == null ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Timesheet ) );
      }
      var resource = await _timesheetService.GetDetail( ts.Id );
      if ( resource == null ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Timesheet ) );
      }
      return Ok( resource );
    }

    /// <summary>
    /// [Admin - Detail] Lấy thông tin chi tiết task trong ngày
    /// </summary>
    /// <remark>quản lý theo dõi</remark>
    /// <param name="userId" example="1">User ID</param>
    /// <param name="year" example="2024">Timesheet year</param>
    /// <param name="month" example="1">Timesheet month</param>
    /// <param name="day" example="1">Timesheet day</param>
    /// <returns></returns>
    [HttpGet( "GetDetail/{userId}/{year}/{month}/{day}" )]
    [Authorize( Policy = Policies.TIMESHEET_VIEW_OTHER )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( TimesheetDetailResponse ) )]
    public async Task<IActionResult> GetDetail( long userId, int year, int month, int day )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/{day}", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      // valid
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLoginStr = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLoginStr.Split( "," );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      var ts = await _timesheetService.GetByCondition( t => t.Date == datetimeTry && t.UserId == userId );
      var teams = await _timesheetService.GetTeamId( ts!.Id );
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && !teamUserLogins.Contains( Enums.TEAM_HR ) ) {
        ts = new HashSet<long>( teams ).SetEquals( teamUserLogins ) ? ts : null;
      }

      // valid timesheet ID có phải của user không
      if ( ts == null ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Timesheet ) );
      }
      var resource = await _timesheetService.GetDetail( ts.Id );
      if ( resource == null ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Timesheet ) );
      }
      return Ok( resource );
    }

    /// <summary>
    /// [Personal - Create] khai báo task trong ngày
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi được trả về theo model
    /// </response>
    [HttpPost( "Create" )]
    [Authorize( Policy = Policies.TIMESHEET_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] TimesheetResource resource )
    {
      // valid
      //if ( !ModelState.IsValid ) {
      //  return BadRequest( ModelState.GetErrorMessages() );
      //}
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      if ( Common.ValidRoleAdmin( positionUserLogin ) ) {
        return BadRequest( ErrorMessageResource.UserNotPermission );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // valid timesheet ID có phải của user không
      if ( !await _timesheetService.Exist( t => t.Id == resource.Id && t.UserId == userLogin ) ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Timesheet ) );
      }
      // valid
      if ( await ValidateBeforeCreate( resource ) )
        return BadRequest( resource );
      // create
      await _timesheetService.Update( resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Admin - Other] khóa timesheet
    /// </summary>
    /// <param name="requestLock"></param>
    /// <returns></returns>
    /// <response code="400">
    /// </response>
    [HttpPost( "Lock" )]
    [Authorize( Policy = Policies.TIMESHEET_LOCK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> LockTimesheet( TimesheetRequestLock requestLock )
    {
      // valid
      var errors = ValidateBeforeLock( requestLock );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _timesheetService.LockTimesheet( requestLock.Start, requestLock.End, userLogin, requestLock.IsLock );
      return Ok();
    }

    /// <summary>
    /// [Personal - Other] Lấy thông tin khởi tạo bashboard: checkin-checkout
    /// </summary>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetDashboardTimesheet" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( DashboardTimesheet ) )]
    public async Task<DashboardTimesheet> GetDashboardTimesheet()
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return await _timesheetService.GetDashboardTimesheet( userLogin );
    }

    /// <summary>
    /// [Personal - Other] Lấy thông tin khởi tạo bashboard: project
    /// </summary>
    /// <param name="year" example="2024">năm chỉ định</param>
    /// <param name="month" example="1">tháng chỉ định</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetDashboardProject/{year}/{month}" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( DashboardProject ) )]
    public async Task<IActionResult> GetDashboardProject( int year, int month )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return Ok( await _timesheetService.GetDashboardProject( userLogin, datetimeTry ) );
    }
    /// <summary>
    /// [Personal - Other] Lấy thông tin khởi tạo dashboard: time
    /// </summary>
    /// <param name="year" example="2024">năm chỉ định</param>
    /// <param name="month" example="1">tháng chỉ định</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetDashboardTime/{year}/{month}" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( DashboardTime ) )]
    public async Task<IActionResult> GetDashboardTime( int year, int month )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return Ok( await _timesheetService.GetDashboardTime( userLogin, datetimeTry ) );
    }
    /// <summary>
    /// [Admin - Report] lấy tổng hợp số liệu ngày làm việc theo tháng
    /// </summary>
    /// <param name="year" example="2024"></param>
    /// <param name="month" example="1"></param>
    /// <param name="type" example="0">0: option 20th~19th, 1: option 01st~30th</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetReport/{year}/{month}/{type}" )]
    [Authorize( Policy = Policies.TIMESHEET_REPORT )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<TimesheetReportDetail> ) )]
    public async Task<IActionResult> GetReport( long year, long month, int type )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      // nếu nhập sai type, tự động filter theo type = 0
      if ( type != 0 && type != 1 ) {
        type = 0;
      }
      DateTime start = type == 0 ? datetimeTry.AddMonths( -1 ).AddDays( 19 ) : datetimeTry;
      DateTime end = type == 0 ? datetimeTry.AddDays( 19 ) : datetimeTry.AddMonths( 1 );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var teamUserLogin = ( long ) 0;
      var result = new List<TimesheetReportDetail>();
      if ( parts.Length > 1 && parts.Any( x => x != Enums.TEAM_HR.ToString() ) ) {
        foreach ( var p in parts ) {
          var teamId = long.Parse( p );
          var res1 = await _timesheetService.GetReport( !Common.ValidRoleAdmin( positionUserLogin ) ? teamId : null, start, end );
          result.AddRange( res1.ToList() );
        }
        return Ok( result );
      }
      else {
        teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
        return Ok( await _timesheetService.GetReport( ( !Common.ValidRoleAdmin( positionUserLogin ) && teamUserLogin != Enums.TEAM_HR ) ? teamUserLogin : null, start, end ) );
      }

    }

    /// <summary>
    /// [Admin - Report] xuất báo cáo thống kê timesheet theo tháng
    /// </summary>
    /// <param name="year" example="2024"></param>
    /// <param name="month" example="1"></param>
    /// <param name="type" example="0">0: option 20th~19th, 1: option 01st~30th</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "ExportReport/{year}/{month}/{type}" )]
    [Authorize( Policy = Policies.TIMESHEET_REPORT )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( byte [] ) )]
    public async Task<IActionResult> ExportReport( long year, long month, int type )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      // nếu nhập sai type, tự động filter theo type = 0
      if ( type != 0 && type != 1 ) {
        type = 0;
      }
      DateTime start = type == 0 ? datetimeTry.AddMonths( -1 ).AddDays( 19 ) : datetimeTry;
      DateTime end = type == 0 ? datetimeTry.AddDays( 18 ) : datetimeTry.AddMonths( 1 ).AddDays( -1 );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLoginStrs = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLoginStrs.Split( "," );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      string fileName = $"{start.Day.ToString( "D2" )}{start.Month.ToString( "D2" )}.{end.Day.ToString( "D2" )}{end.Month.ToString( "D2" )}.VCI.HRM TIMESHEET.xlsx";
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && !teamUserLogins.Any( x => x == Enums.TEAM_HR ) ) {
        return File( ( await _timesheetService.ExportSummaryTimesheet( teamUserLogins.First(), start, end, teamUserLogins.Skip( 1 ).ToArray() ) )!, "application/xlsx", fileName );
      }
      return File( ( await _timesheetService.ExportSummaryTimesheet( null, start, end, null ) )!, "application/xlsx", fileName );
    }

    /// <summary>
    /// [Admin - Report] xuất báo cáo thống kê timesheet theo Team trong tháng
    /// </summary>
    /// <param name="year" example="2024"></param>
    /// <param name="month" example="1"></param>
    /// <param name="type" example="0">0: option 20th~19th, 1: option 01st~30th</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "ExportReportByTeam/{year}/{month}/{type}" )]
    [Authorize( Policy = Policies.TIMESHEET_REPORT )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( byte [] ) )]
    public async Task<IActionResult> ExportReportByTeam( long year, long month, int type )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      // nếu nhập sai type, tự động filter theo type = 0
      if ( type != 0 && type != 1 ) {
        type = 0;
      }
      DateTime start = type == 0 ? datetimeTry.AddMonths( -1 ).AddDays( 19 ) : datetimeTry;
      DateTime end = type == 0 ? datetimeTry.AddDays( 18 ) : datetimeTry.AddMonths( 1 ).AddDays( -1 );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLoginStrs = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLoginStrs.Split( "," );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      string fileName = $"SummaryTimesheetByTeam_{DateTime.Now.ToString( "yyyy_MM_dd-HH_mm_ss" )}.xlsx";
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && parts.Any( x => x != Enums.TEAM_HR.ToString() ) ) {
        return File( ( await _timesheetService.ExportSummaryTimesheetByTeam( teamUserLogins.First(), start, end, teamUserLogins.Skip( 1 ).ToArray() ) )!, "application/xlsx", fileName );
      }
      return File( ( await _timesheetService.ExportSummaryTimesheetByTeam( null, start, end, null ) )!, "application/xlsx", fileName );
    }

    /// <summary>
    /// lấy danh sách record ngày hoán đổi theo năm
    /// </summary>
    /// <param name="year" example="2024"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "SwapDay/GetListView/{year}" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( SwapDayResponse [] ) )]
    public async Task<IActionResult> SwapDayGetListView( int year )
    {
      return Ok( await _timesheetService.SwapDayGetListView( year ) );
    }

    /// <summary>
    /// tạo record hoán đổi ngày mới
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpPost( "SwapDay/Create" )]
    [Authorize( Policy = Policies.SWAPDAY_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    public async Task<IActionResult> SwapDayCreate( SwapDayResource resource )
    {
      // valid
      if ( !ModelState.IsValid ) {
        return BadRequest( ModelState.GetErrorMessages() );
      }
      resource.Id = null;
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var id = await _timesheetService.SwapDayCreate( resource, userLogin );
      return Ok( id );
    }

    /// <summary>
    /// cập nhật record hoán đổi ngày
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpPut( "SwapDay/Update" )]
    [Authorize( Policy = Policies.SWAPDAY_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( SwapDayResponse [] ) )]
    public async Task<IActionResult> SwapDayUpdate( SwapDayResource resource )
    {
      // valid
      if ( !ModelState.IsValid ) {
        return BadRequest( ModelState.GetErrorMessages() );
      }
      if ( resource.Id == null ) {
        ModelState.AddModelError( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.SwapDay ) );
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _timesheetService.SwapDayUpdate( resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// xóa record ngày hoán đổi
    /// </summary>
    /// <param name="id" example="1">ID swap day chỉ định</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpDelete( "SwapDay/Delete/{id}" )]
    [Authorize( Policy = Policies.SWAPDAY_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    public async Task<IActionResult> SwapDayDelete( long id )
    {
      await _timesheetService.SwapDayDelete( id );
      return Ok();
    }

    [HttpGet( "ExportReportTimeSheet/{year}/{month}" )]
    [AllowAnonymous]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( byte [] ) )]
    public async Task<IActionResult> ExportReportByMonth( long year, long month )
    {
      var fileName = $"Timesheet_{year}_{month.ToString( "D2" )}.xlsx";
      return File( ( await _timesheetService.ExportTimesheetByMonth( year, month ) )!, "application/xlsx", fileName );
    }

    [HttpGet( "ExportReport/week" )]
    [Authorize( Policy = Policies.TIMESHEET_REPORT )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( byte [] ) )]
    public async Task<IActionResult> ExportReportByWeek( string startDate, string endDate )
    {
      DateTime start = DateTime.Parse( startDate );
      DateTime end = DateTime.Parse( endDate );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLoginStrs = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLoginStrs.Split( "," );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      string fileName = $"{start.Day.ToString( "D2" )}{start.Month.ToString( "D2" )}.{end.Day.ToString( "D2" )}{end.Month.ToString( "D2" )}.VCI.HRM TIMESHEET.xlsx";
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && !teamUserLogins.Any( x => x == Enums.TEAM_HR ) ) {
        return File( ( await _timesheetService.ExportSummaryTimesheet( teamUserLogins.First(), start, end, teamUserLogins.Skip( 1 ).ToArray() ) )!, "application/xlsx", fileName );
      }
      return File( ( await _timesheetService.ExportSummaryTimesheet( null, start, end, null ) )!, "application/xlsx", fileName );
    }
    /// <summary>
    /// valid resource trước khi create
    /// </summary>
    /// <param name="resource"></param>
    /// <returns>true: invalid</returns>
    private async Task<bool> ValidateBeforeCreate( TimesheetResource resource )
    {
      bool invalid = false;
      resource.UserId = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var team = await _teamService.GetTeamUserByUserId( resource.UserId );
      var timesheet = await _timesheetService.GetTimesheet( resource.Id );
      if ( timesheet!.LockBy is not null ) {
        invalid = true;
        resource.Error = string.Format( ErrorMessageResource.HadLocked, DisplayNameResource.Timesheet );
      }
      else if ( !string.IsNullOrEmpty( timesheet.HolidayName ) || await _holidayService.Exist( h => h.StartDate <= timesheet.Date && h.EndDate >= timesheet.Date ) ) {
        invalid = true;
        resource.Error = string.Format( ErrorMessageResource.CanNotLogTimesheetInHoliday, DisplayNameResource.Timesheet );
      }
      else {
        if ( resource.Projects != null && resource.Projects.Count() > 0 ) {
          var projectWorking = await _projectService.GetOptionWorking( resource.UserId );
          double totalHour = 0;
          Regex regexHourInput = new Regex( "^[0-9]{2}:[0-9]{2}$" );
          var date = DateTime.Now.ToString( Enums.DATE_FORMAT );
          DateTime dateTimeTry;
          foreach ( var project in resource.Projects ) {
            // valid user working project
            if ( !projectWorking.Any( p => p.Id == project.ProjectId ) ) {
              project.ErrorProject = string.Format( ErrorMessageResource.UserNotWorkingProject, DisplayNameResource.TimesheetProject );
              invalid = true;
            }
            else if ( project.Tasks != null && project.Tasks.Count() > 0 ) {
              var taskTemp = new List<TimesheetResourceData>();
              foreach ( var task in project.Tasks ) {
                // valid category
                if ( !await _categoryService.Exist( c => c.Id == task.CategoryId ) ) {
                  task.ErrorCategory = string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.TimesheetCategory );
                  invalid = true;
                }
                // valid hour
                if ( regexHourInput.IsMatch( task.Hours ) ) {
                  // valid time in
                  if ( !DateTime.TryParse( date + " " + task.Hours, out dateTimeTry ) ) {
                    task.ErrorHour = string.Format( ErrorMessageResource.DateWrongFormat, DisplayNameResource.TimesheetHour );
                    invalid = true;
                  }
                  else if ( dateTimeTry.Hour == 0 && dateTimeTry.Minute == 0 ) {
                    continue;
                  }
                  else {
                    task.HourValid = Common.FormatHourInputToData( dateTimeTry.Hour + ( double ) dateTimeTry.Minute / 60 );
                  }
                }
                else {
                  task.ErrorHour = string.Format( ErrorMessageResource.DateWrongFormat, DisplayNameResource.TimesheetHour );
                  invalid = true;
                }
                taskTemp.Add( task );
                // task.Hours = Common.FormatHourInputToData( task.Hours );
                totalHour += task.HourValid;
              }
              project.Tasks = taskTemp;
            }
          }
        }
        // nếu ko xảy ra lỗi gì, thực hiện loại trừ các record thừa hoặc blank
        if ( !invalid && resource.Projects != null && resource.Projects.Count() > 0 ) {
          resource.Projects = resource.Projects.Where( p => p.Tasks != null && p.Tasks.Count() > 0 ).ToList();
        }
      }
      return invalid;
    }

    /// <summary>
    /// valid object trước khi lock
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private Dictionary<string, string> ValidateBeforeLock( TimesheetRequestLock request )
    {
      var errors = new Dictionary<string, string>();
      request.Start = request.Start.Date;
      request.End = request.End.Date;
      if ( request.End < request.Start ) {
        errors.Add( nameof( TimesheetRequestLock.Start ), ErrorMessageResource.DatePastError );
      }
      else if ( request.End >= DateTime.Now.Date ) {
        errors.Add( nameof( TimesheetRequestLock.End ), ErrorMessageResource.DateFutureError );
      }
      return errors;
    }
  }
}
