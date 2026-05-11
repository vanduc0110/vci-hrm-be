using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers
{
  /// <summary>
  /// APIs của Leave
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  public class LeaveController : ControllerBase
  {
    private readonly ILeaveService _leaveService;
    private readonly IHolidayService _holidayService;
    private readonly ITeamService _teamService;
    private readonly IMapper _mapper;
    private readonly ILogger<LeaveController> _logger;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

    public LeaveController( ILeaveService leaveService,
      IHolidayService holidayService,
      ITeamService teamService,
      IMapper mapper, ILogger<LeaveController> logger,
      IOptions<ApiBehaviorOptions> apiBehaviorOptions )
    {
      _leaveService = leaveService;
      _holidayService = holidayService;
      _teamService = teamService;
      _mapper = mapper;
      _logger = logger;
      _apiBehaviorOptions = apiBehaviorOptions;
    }

    /// <summary>
    /// [Personal - List] Lấy danh sách leave request
    /// </summary>
    /// <param name="year" example="2024">năm tìm kiếm</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetRequestListView/{year}" )]
    [Authorize( Policy = Policies.LEAVE_GET_REQUEST_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<LeaveRequestResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetRequestListView( long year )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return Ok( await _leaveService.GetRequestList( userLogin, year ) );
    }

    /// <summary>
    /// [Admin - List] Lấy danh sách request leave mà user login quản lý team
    /// </summary>
    /// <param name="year" example="2024">năm tìm kiếm</param>
    /// <param name="month" example="1">tháng tìm kiếm</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView/{year}/{month}" )]
    [Authorize( Policy = Policies.LEAVE_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<LeaveRequestResponse> ) )]
    public async Task<IActionResult> GetListView( long year, long month )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var userLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var result = new List<LeaveRequestResponse>();
      if ( parts.Length > 1 && parts.Any( x => x != Enums.TEAM_HR.ToString() ) ) {
        foreach ( var p in parts ) {
          var teamId = long.Parse( p );
          var res1 = await _leaveService.GetList( new BaseFilter()
          {
            TeamId = !Common.ValidRoleAdmin( positionUserLogin ) ? teamId : null,
            Year = year,
            Month = month,
            UserId = userLogin
          } );
          result.AddRange( res1.ToList() );
        }
        return Ok( result );
      }
      else {
        var teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
        return Ok( await _leaveService.GetList( new BaseFilter()
        {
          TeamId = ( !Common.ValidRoleAdmin( positionUserLogin ) && teamUserLogin != Enums.TEAM_HR ) ? teamUserLogin : null,
          Year = year,
          Month = month,
          UserId = userLogin
        } ) );
      }

    }

    /// <summary>
    /// [Personal - Create] Tạo request leave mới
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="200">ID của new request leave</response>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPost( "Create" )]
    [Authorize( Policy = Policies.LEAVE_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] LeaveRequestResource resource )
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

      // valid
      var errors = await ValidateBeforeCreate( resource, userLogin );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      resource.Id = null;
      // create
      var leaveId = await _leaveService.Create( resource, userLogin );
      return Ok( leaveId );
    }

    /// <summary>
    /// [Personal - Update] Cập nhật request leave
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPut( "Update" )]
    [Authorize( Policy = Policies.LEAVE_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Update( [FromBody] LeaveRequestResource resource )
    {
      //if ( !ModelState.IsValid )
      //  return BadRequest( ModelState.GetErrorMessages() );
      // valid
      var errors = await ValidateBeforeUpdate( resource );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // update
      await _leaveService.Update( resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Personal - Delete] Xóa request leave
    /// </summary>
    /// <param name="id" example="1">ID request leave muốn xóa</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// </response>
    [HttpDelete( "Delete/{id}" )]
    [Authorize( Policy = Policies.LEAVE_DELETE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Delete( long id )
    {
      // valid
      var errors = await ValidateBeforeDelete( id );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      await _leaveService.Delete( id );
      return Ok();
    }

    /// <summary>
    /// [Admin - Other] Approve/Reject request leave
    /// </summary>
    /// <param name="id" example="1">request leave ID</param>
    /// <param name="isApprove" example="true">true: approve, false: reject</param>
    /// <returns></returns>
    [HttpPost( "Approve/{id}/{isApprove}" )]
    [Authorize( Policy = Policies.LEAVE_APPROVE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    public async Task<IActionResult> Approve( long id, bool isApprove )
    {
      // valid
      var errors = await ValidateBeforeApprove( id, isApprove );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _leaveService.Approve( id, isApprove, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Personal - Other] danh sách lịch sử leave và các thông số
    /// </summary>
    /// <param name="year" example="2024">năm tìm kiếm</param>
    /// <returns></returns>
    [HttpGet( "GetHistories/{year}" )]
    [Authorize]
    public async Task<IActionResult> GetHistories( long year )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/01/01", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return Ok( await _leaveService.GetLeaveHistories( userLogin, year ) );
    }

    [HttpPost( "UpdateAnualLeave" )]
    [Authorize( Policy = Policies.LEAVE_REPORT )]
    public async Task<IActionResult> UpdateAnualLeave( [FromBody] AnnualLeaveResource resource )
    {
      await _leaveService.UpdateAnunalLeave( resource.userId, resource.annualLeave, resource.notes );
      return NoContent();
    }

    /// <summary>
    /// [Personal - Other] lấy số liệu ngày nghỉ còn lại 
    /// </summary>
    /// <returns></returns>
    [HttpGet( "GetStateRemainLeave" )]
    [Authorize]
    public async Task<RemainLeave> GetStateRemainLeave()
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return await _leaveService.GetRemainLeave( userLogin );
    }

    /// <summary>
    /// [Admin - Report] lấy tổng hợp số liệu ngày nghỉ theo năm
    /// </summary>
    /// <param name="year" example="2024">năm tìm kiếm</param>
    /// <returns></returns>
    [HttpGet( "GetReport/{year}" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( LeaveReport ) )]
    public async Task<LeaveReport> GetReport( long year )
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var userLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var result = new LeaveReport();
      if ( parts.Length >= 1 && parts.Any( x => x != Enums.TEAM_HR.ToString() ) && userLogin != 1 ) {
        foreach ( var p in parts ) {
          var teamId = long.Parse( p );
          var res1 = await _leaveService.GetReport( teamId, year );
          result.AnnualLeaves.AddRange( res1.AnnualLeaves );
        }
        return result;
      }
      else {
        return await _leaveService.GetReport( null, year );
      }

    }

    /// <summary>
    /// [Admin - Report] xuất báo cáo thống kê leave request theo năm
    /// </summary>
    /// <param name="year" example="2024">năm tìm kiếm</param>
    /// <returns></returns>
    [HttpGet( "ExportReport/{year}" )]
    [Authorize( Policy = Policies.LEAVE_REPORT )]
    public async Task<IActionResult> ExportReport( long year )
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      string fileName = $"LeaveDayStaffs_{DateTime.UtcNow.ToString( "yyyy_MM_dd-HH_mm_ss" )}.xlsx";
      if ( !Common.ValidRoleAdmin( positionUserLogin ) ) {
        return File( ( await _leaveService.ExportLeaveDayStaff( teamUserLogin, year ) )!, "application/xlsx", fileName );
      }
      return File( ( await _leaveService.ExportLeaveDayStaff( null, year ) )!, "application/xlsx", fileName );
    }

    /// <summary>
    /// [Personal - Other] lấy danh sách các loại leave quy định sẵn
    /// </summary>
    /// <returns></returns>
    [HttpGet( "GetLeaveInformation" )]
    [Authorize]
    public async Task<IEnumerable<LeaveInformationResponse>> GetLeaveInformation()
      {
      return await _leaveService.GetLeaveInformationResponses();
    }
    [HttpGet( "GetTotalLeavePending" )]
    [Authorize( Policy = Policies.LEAVE_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    public async Task<IActionResult> GetTotalListRequest()
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      var totalLeaveRequests = 0;
      if ( !Common.ValidRoleAdmin( positionUserLogin ) ) {
        foreach ( var teamId in teamUserLogin ) {
          totalLeaveRequests += await _leaveService.GetTotalLeaveRequest( teamId );
        }
      }
      else {
        totalLeaveRequests = await _leaveService.GetTotalLeaveRequest( null );
      }
      return Ok( totalLeaveRequests );
    }
    /// <summary>
    /// valid resource trước khi create
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeCreate( LeaveRequestResource resource, long id )
    {
      var errors = new Dictionary<string, string>();
      // valid
      var dateValidMinimum = DateTime.UtcNow.Date.AddDays( 1 - DateTime.UtcNow.Day ).AddMonths( -1 ); // ngày mồng 1 của tháng trước, hôm nay 15/06/23 thì đây là 01/05/23
      var dateValidMaximum = DateTime.UtcNow.Date.AddDays( 1 - DateTime.UtcNow.Day ).AddMonths( 2 ); // ngày mồng 1 của 2 tháng sau, hôm nay 15/06/23 thì đây là 01/08/23
      if ( resource.StartDate < dateValidMinimum || resource.StartDate >= dateValidMaximum ||
        resource.EndDate < dateValidMinimum || resource.EndDate >= dateValidMaximum ) {
        errors.Add( nameof( LeaveRequestResource.StartDate ), string.Format( ErrorMessageResource.DateRangeError, DisplayNameResource.LeaveRequestStartDate ) );
      }
      DateTime dateTimeTry;
      // valid start time
      if ( string.IsNullOrEmpty( resource.StartTime ) || !DateTime.TryParse( resource.StartDate.ToString( Enums.DATE_FORMAT ) + " " + resource.StartTime, out dateTimeTry ) ) {
        errors.Add( nameof( LeaveRequestResource.StartTime ), string.Format( ErrorMessageResource.DateWrongFormat, DisplayNameResource.LeaveRequestStartDate ) );
      }
      else {
        resource.StartDate = dateTimeTry;
      }
      // valid end time
      if ( string.IsNullOrEmpty( resource.EndTime ) || !DateTime.TryParse( resource.EndDate.ToString( Enums.DATE_FORMAT ) + " " + resource.EndTime, out dateTimeTry ) ) {
        errors.Add( nameof( LeaveRequestResource.EndTime ), string.Format( ErrorMessageResource.DateWrongFormat, DisplayNameResource.LeaveRequestEndDate ) );
      }
      else {
        resource.EndDate = dateTimeTry;
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // valid
      if ( !errors.ContainsKey( nameof( LeaveRequestResource.StartDate ) ) && !errors.ContainsKey( nameof( LeaveRequestResource.EndDate ) ) ) {
        if ( resource.EndDate <= resource.StartDate ) {
          errors.Add( nameof( LeaveRequestResource.StartDate ), string.Format( ErrorMessageResource.DatePastError, DisplayNameResource.LeaveRequestStartDate ) );
        }
        // valid ngày request đã bị lock timesheet chưa
        else if ( await _leaveService.CheckTimesheetHadLock( resource.StartDate, resource.EndDate ) ) {
          errors.Add( nameof( LeaveRequestResource.StartDate ), string.Format( ErrorMessageResource.HadLocked, DisplayNameResource.Timesheet ) );
        }
        // valid exist leave request
        else if ( await _leaveService.Exist( o => o.CreatedBy == userLogin && o.Id != resource.Id &&
          o.Status != ( int ) Enums.LeaveRequestStatus.Reject && o.StartDate < resource.EndDate && resource.StartDate < o.EndDate ) ) {
          errors.Add( nameof( LeaveRequestResource.StartDate ), string.Format( ErrorMessageResource.HadExistError, DisplayNameResource.LeaveRequestStartDate ) );
        }
        else if ( resource.StartDate.DayOfWeek == DayOfWeek.Sunday ||
              resource.EndDate.DayOfWeek == DayOfWeek.Sunday ||
              resource.StartDate.Hour < Enums.TimeWorkStartHour || resource.StartDate.Hour > Enums.TimeWorkEndHour ||
              resource.EndDate.Hour < Enums.TimeWorkStartHour || resource.EndDate.Hour > Enums.TimeWorkEndHour ||
              await _holidayService.Exist( h => ( ( h.StartDate <= resource.StartDate && resource.StartDate <= h.EndDate ) ||
                ( h.StartDate <= resource.EndDate && resource.EndDate <= h.EndDate ) ) && h.Status == ( int ) Enums.HolidayStatus.Apply ) ) {
          errors.Add( nameof( LeaveRequestResource.StartDate ), string.Format( ErrorMessageResource.LeaveRequestTimeWorkError, DisplayNameResource.LeaveRequestStartDate ) );
        }
        else if ( !errors.ContainsKey( nameof( LeaveRequestResource.Type ) ) ) {
          await ValidLeaveType( resource, errors );
        }
      }
      return errors;
    }

    /// <summary>
    /// valid resource trước khi update
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeUpdate( LeaveRequestResource resource )
    {
      var errors = new Dictionary<string, string>();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      if ( resource.Id is null ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.LeaveRequest ) );
      }
      else {
        var old = await _leaveService.GetByCondition( g => g.Id == resource.Id );
        if ( old is null || old.CreatedBy != userLogin ) {
          errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.LeaveRequest ) );
        }
        else if ( old.Status == ( int ) Enums.LeaveRequestStatus.Approve ) {
          errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.LeaveRequest ) );
        }
        else {
          // valid
          var dateValidMinimum = DateTime.UtcNow.Date.AddDays( 1 - DateTime.UtcNow.Day ).AddMonths( -1 ); // ngày mồng 1 của tháng trước, hôm nay 15/06/23 thì đây là 01/05/23
          var dateValidMaximum = DateTime.UtcNow.Date.AddDays( 1 - DateTime.UtcNow.Day ).AddMonths( 2 ); // ngày mồng 1 của 2 tháng sau, hôm nay 15/06/23 thì đây là 01/08/23
          if ( resource.StartDate < dateValidMinimum || resource.StartDate >= dateValidMaximum ||
            resource.EndDate < dateValidMinimum || resource.EndDate >= dateValidMaximum ) {
            errors.Add( nameof( LeaveRequestResource.StartDate ), string.Format( ErrorMessageResource.DateRangeError, DisplayNameResource.LeaveRequestStartDate ) );
          }
          DateTime dateTimeTry;
          // valid start time
          if ( string.IsNullOrEmpty( resource.StartTime ) || !DateTime.TryParse( resource.StartDate.ToString( Enums.DATE_FORMAT ) + " " + resource.StartTime, out dateTimeTry ) ) {
            errors.Add( nameof( LeaveRequestResource.StartTime ), string.Format( ErrorMessageResource.DateWrongFormat, DisplayNameResource.LeaveRequestStartDate ) );
          }
          else {
            resource.StartDate = dateTimeTry;
          }
          // valid end time
          if ( string.IsNullOrEmpty( resource.EndTime ) || !DateTime.TryParse( resource.EndDate.ToString( Enums.DATE_FORMAT ) + " " + resource.EndTime, out dateTimeTry ) ) {
            errors.Add( nameof( LeaveRequestResource.EndTime ), string.Format( ErrorMessageResource.DateWrongFormat, DisplayNameResource.LeaveRequestEndDate ) );
          }
          else {
            resource.EndDate = dateTimeTry;
          }
          // valid
          if ( !errors.ContainsKey( nameof( LeaveRequestResource.StartDate ) ) || !errors.ContainsKey( nameof( LeaveRequestResource.StartDate ) ) ) {
            if ( resource.EndDate < resource.StartDate ) {
              errors.Add( nameof( LeaveRequestResource.StartDate ), string.Format( ErrorMessageResource.DatePastError, DisplayNameResource.LeaveRequestStartDate ) );
            }
            // valid ngày request đã bị lock timesheet chưa
            else if ( await _leaveService.CheckTimesheetHadLock( resource.StartDate, resource.EndDate ) ) {
              errors.Add( nameof( LeaveRequestResource.StartDate ), string.Format( ErrorMessageResource.HadLocked, DisplayNameResource.Timesheet ) );
            }
            // valid exist OT request
            else if ( await _leaveService.Exist( o => o.CreatedBy == userLogin && o.Id != resource.Id &&
              o.Status != ( int ) Enums.LeaveRequestStatus.Reject && o.StartDate < resource.EndDate && resource.StartDate < o.EndDate ) ) {
              errors.Add( nameof( LeaveRequestResource.StartDate ), string.Format( ErrorMessageResource.HadExistError, DisplayNameResource.LeaveRequestStartDate ) );
            }
            else if ( !errors.ContainsKey( nameof( LeaveRequestResource.StartTime ) ) && resource.StartDate.DayOfWeek == DayOfWeek.Sunday ||
              resource.EndDate.DayOfWeek == DayOfWeek.Sunday ||
              resource.StartDate.Hour < Enums.TimeWorkStartHour || resource.StartDate.Hour > Enums.TimeWorkEndHour ||
              resource.EndDate.Hour < Enums.TimeWorkStartHour || resource.EndDate.Hour > Enums.TimeWorkEndHour ||
              await _holidayService.Exist( h => ( ( h.StartDate <= resource.StartDate && resource.StartDate <= h.EndDate ) ||
                ( h.StartDate <= resource.EndDate && resource.EndDate <= h.EndDate ) ) && h.Status == ( int ) Enums.HolidayStatus.Apply ) ) {
              errors.Add( nameof( LeaveRequestResource.StartDate ), string.Format( ErrorMessageResource.LeaveRequestTimeWorkError, DisplayNameResource.LeaveRequestStartDate ) );
            }
            else if ( !errors.ContainsKey( nameof( LeaveRequestResource.Type ) ) ) {
              await ValidLeaveType( resource, errors );
            }
          }
        }
      }
      return errors;
    }

    /// <summary>
    /// valid object trước khi delete
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeDelete( long id )
    {
      var errors = new Dictionary<string, string>();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // valid
      if ( !await _leaveService.Exist( g => g.Id == id && g.CreatedBy == userLogin ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Leave ) );
      }
      // valid status 
      else if ( await _leaveService.Exist( g => g.Id == id && g.Status == ( int ) Enums.LeaveRequestStatus.Approve ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Leave ) );
      }
      return errors;
    }

    private async Task ValidLeaveType( LeaveRequestResource resource, Dictionary<string, string> errors )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // get list holiday
      var holiday = await _holidayService.GetByCondition( h => ( ( h.StartDate >= resource.StartDate && resource.EndDate >= h.StartDate ) ||
                ( h.EndDate >= resource.StartDate && resource.EndDate >= h.EndDate ) ) && h.Status == ( int ) Enums.HolidayStatus.Apply );
      // valid type
      switch ( resource.Type ) {
        case "SelfWedding":
        case "FamilyWedding":
        case "FamilyBereavement":
        case "RelativeBereavement":
        case "FamilyMaternity":
          // valid leave theo block 8h-12h 13h-17h
          if ( !( ( resource.StartDate.Hour == Enums.TimeWorkStartHour && resource.StartDate.Minute == Enums.TimeWorkStartMinute ) ||
          ( resource.StartDate.Hour == Enums.TimeWorkEndHourMorning && resource.StartDate.Minute == Enums.TimeWorkEndMinuteMorning ) ||
          ( resource.StartDate.Hour == Enums.TimeWorkStartHourAfternoon && resource.StartDate.Minute == Enums.TimeWorkStartMinuteAfternoon ) ) ||
          !( ( resource.EndDate.Hour == Enums.TimeWorkEndHourMorning && resource.EndDate.Minute == Enums.TimeWorkEndMinuteMorning ) ||
          ( resource.EndDate.Hour == Enums.TimeWorkStartHourAfternoon && resource.EndDate.Minute == Enums.TimeWorkStartMinuteAfternoon ) ||
          ( resource.EndDate.Hour == Enums.TimeWorkEndHour && resource.EndDate.Minute == Enums.TimeWorkEndMinute ) ) ) {
            if ( resource.Type == "SelfWedding" )
              errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.LeaveRequestBlockError, DisplayNameResource.LeaveRequest, resource.Type, $"{Enums.MINIMUM_PERIOD_LEAVE_SELF_WEDDING} {DisplayNameResource.Hours}" ) );
            else if ( resource.Type == "FamilyWedding" )
              errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.LeaveRequestBlockError, DisplayNameResource.LeaveRequest, resource.Type, $"{Enums.MINIMUM_PERIOD_LEAVE_FAMILY_WEDDING} {DisplayNameResource.Hours}" ) );
            else if ( resource.Type == "FamilyBereavement" )
              errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.LeaveRequestBlockError, DisplayNameResource.LeaveRequest, resource.Type, $"{Enums.MINIMUM_PERIOD_LEAVE_FAMILY_BEREAVEMENT} {DisplayNameResource.Hours}" ) );
            else if ( resource.Type == "RelativeBereavement" )
              errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.LeaveRequestBlockError, DisplayNameResource.LeaveRequest, resource.Type, $"{Enums.MINIMUM_PERIOD_LEAVE_RELATIVE_BEREAVEMENT} {DisplayNameResource.Hours}" ) );
            else if ( resource.Type == "FamilyMaternity" )
              errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.LeaveRequestBlockError, DisplayNameResource.LeaveRequest, resource.Type, $"{Enums.MINIMUM_PERIOD_LEAVE_FAMILY_MATERNITY} {DisplayNameResource.Hours}" ) );
          }
          else { // valid tối thiểu/tối đa thời gian được nghỉ theo loại leave
            var timeLeaves = Common.TotalTimeWorkExcludeWeekend( resource.StartDate, resource.EndDate, "hour", holiday );
            resource.Hours = timeLeaves.Sum( t => t.Hours );
            resource.LeaveDetail = timeLeaves;
            if ( resource.Type == "SelfWedding" ) {
              if ( resource.Hours < Enums.MINIMUM_PERIOD_LEAVE_SELF_WEDDING ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MinimumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MINIMUM_PERIOD_LEAVE_SELF_WEDDING, DisplayNameResource.Hours ) );
              }
              else if ( resource.Hours / 8 > Enums.MAXIMUM_PERIOD_LEAVE_SELF_WEDDING ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MaximumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MAXIMUM_PERIOD_LEAVE_SELF_WEDDING, DisplayNameResource.Days ) );
              }
              else {
                var weddingLeaves = await _leaveService.GetAll();
                var lastWeddingLeaves = weddingLeaves.Where( w => w.CreatedBy == userLogin && w.Type == ( int ) Enums.LeaveType.SelfWedding ).OrderByDescending( x => x.CreatedDate ).First();
                var dateCheck = lastWeddingLeaves.StartDate.AddDays( 30 );
                if ( resource.StartDate < dateCheck ) {
                  errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.LeaveSelfWeddingError, resource.Type ) );
                }
              }
            }
            else if ( resource.Type == "FamilyWedding" ) {
              if ( resource.Hours < Enums.MINIMUM_PERIOD_LEAVE_FAMILY_WEDDING ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MinimumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MINIMUM_PERIOD_LEAVE_FAMILY_WEDDING, DisplayNameResource.Hours ) );
              }
              else if ( resource.Hours / 8 > Enums.MAXIMUM_PERIOD_LEAVE_FAMILY_WEDDING ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MaximumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MAXIMUM_PERIOD_LEAVE_FAMILY_WEDDING, DisplayNameResource.Days ) );
              }
            }
            else if ( resource.Type == "FamilyBereavement" ) {
              if ( resource.Hours < Enums.MINIMUM_PERIOD_LEAVE_FAMILY_BEREAVEMENT ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MinimumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MINIMUM_PERIOD_LEAVE_FAMILY_BEREAVEMENT, DisplayNameResource.Hours ) );
              }
              else if ( resource.Hours / 8 > Enums.MAXIMUM_PERIOD_LEAVE_FAMILY_BEREAVEMENT ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MaximumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MAXIMUM_PERIOD_LEAVE_FAMILY_BEREAVEMENT, DisplayNameResource.Days ) );
              }
            }
            else if ( resource.Type == "RelativeBereavement" ) {
              if ( resource.Hours < Enums.MINIMUM_PERIOD_LEAVE_RELATIVE_BEREAVEMENT ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MinimumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MINIMUM_PERIOD_LEAVE_RELATIVE_BEREAVEMENT, DisplayNameResource.Hours ) );
              }
              else if ( resource.Hours / 8 > Enums.MAXIMUM_PERIOD_LEAVE_RELATIVE_BEREAVEMENT ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MaximumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MAXIMUM_PERIOD_LEAVE_RELATIVE_BEREAVEMENT, DisplayNameResource.Days ) );
              }
            }
            else if ( resource.Type == "FamilyMaternity" ) {
              if ( resource.Hours < Enums.MINIMUM_PERIOD_LEAVE_FAMILY_MATERNITY ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MinimumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MINIMUM_PERIOD_LEAVE_FAMILY_MATERNITY, DisplayNameResource.Hours ) );
              }
              else if ( resource.Hours / 8 > Enums.MAXIMUM_PERIOD_LEAVE_FAMILY_MATERNITY ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MaximumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MAXIMUM_PERIOD_LEAVE_FAMILY_MATERNITY, DisplayNameResource.Days ) );
              }
            }
          }
          break;
        case "SelfMaternity": // trường hợp đặc biệt nghỉ sinh, đơn vị lưu sẽ là month
          resource.EndDate = resource.StartDate.AddMonths( 6 );
          resource.Hours = 8;
          break;
        case "Annual":
          // valid leave theo block 8h 12h 13h 17h
          if ( !( ( resource.StartDate.Hour == Enums.TimeWorkStartHour && resource.StartDate.Minute == Enums.TimeWorkStartMinute ) ||
          ( resource.StartDate.Hour == Enums.TimeWorkEndHourMorning && resource.StartDate.Minute == Enums.TimeWorkEndMinuteMorning ) ||
          ( resource.StartDate.Hour == Enums.TimeWorkStartHourAfternoon && resource.StartDate.Minute == Enums.TimeWorkStartMinuteAfternoon ) ) ||
          !( ( resource.EndDate.Hour == Enums.TimeWorkEndHourMorning && resource.EndDate.Minute == Enums.TimeWorkEndMinuteMorning ) ||
          ( resource.EndDate.Hour == Enums.TimeWorkStartHourAfternoon && resource.EndDate.Minute == Enums.TimeWorkStartMinuteAfternoon ) ||
          ( resource.EndDate.Hour == Enums.TimeWorkEndHour && resource.EndDate.Minute == Enums.TimeWorkEndMinute ) ) ) {
            if ( resource.Type == "Annual" )
              errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.LeaveRequestBlockError, DisplayNameResource.LeaveRequest, resource.Type, $"{Enums.MINIMUM_PERIOD_LEAVE_ANNUAL} {DisplayNameResource.Hours}" ) );
          }
          else { // valid tối thiểu/tối đa thời gian được nghỉ theo loại leave
            var timeLeaves = Common.TotalTimeWorkExcludeWeekend( resource.StartDate, resource.EndDate, "hour", holiday );
            resource.Hours = timeLeaves.Sum( t => t.Hours );
            resource.LeaveDetail = timeLeaves;
            var remainLeave = await _leaveService.GetRemainLeave( userLogin );
            if ( resource.Type == "Annual" ) {
              if ( resource.Hours < Enums.MINIMUM_PERIOD_LEAVE_ANNUAL ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.MinimumPeriodLeaveError, DisplayNameResource.LeaveRequest, resource.Type, Enums.MINIMUM_PERIOD_LEAVE_ANNUAL, DisplayNameResource.Hours ) );
              }
              else if ( resource.Hours > remainLeave.Annual * 8 ) {
                errors.Add( nameof( LeaveRequestResource.Type ), string.Format( ErrorMessageResource.OverRemainLeaveError, DisplayNameResource.LeaveRequest, resource.Type, remainLeave.Annual, DisplayNameResource.Days ) );
              }
            }
          }
          break;
        case "Unpaid":
          var timeLeave = Common.TotalTimeWorkExcludeWeekend( resource.StartDate, resource.EndDate, "hour", holiday );
          resource.Hours = timeLeave.Sum( t => t.Hours );
          resource.LeaveDetail = timeLeave;
          if ( !errors.ContainsKey( nameof( LeaveRequestResource.StartTime ) ) && resource.Hours * 60 < Enums.MINIMUM_PERIOD_LEAVE_UNPAID ) {
            errors.Add( nameof( LeaveRequestResource.StartTime ), string.Format( ErrorMessageResource.MinimumPeriodLeaveError, DisplayNameResource.LeaveRequestStartDate, resource.Type, Enums.MINIMUM_PERIOD_LEAVE_UNPAID, DisplayNameResource.Minutes ) );
          }
          break;
      }
      return;
    }

    /// <summary>
    /// valid object trước khi approve
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isApprove"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeApprove( long id, bool isApprove )
    {
      var errors = new Dictionary<string, string>();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      var leaveRequest = await _leaveService.GetByCondition( o => o.Id == id );
      // valid
      if ( leaveRequest is null ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.LeaveRequest ) );
      }
      // valid status 
      else if ( !( isApprove && leaveRequest.Status == ( int ) Enums.LeaveRequestStatus.Pending ) && !( !isApprove && leaveRequest.Status != ( int ) Enums.LeaveRequestStatus.Reject ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ChangeStatusError, DisplayNameResource.Overtime ) );
      }
      // valid ngày request đã bị lock timesheet chưa
      else if ( await _leaveService.CheckTimesheetHadLock( leaveRequest.StartDate, leaveRequest.EndDate ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.HadLocked, DisplayNameResource.LeaveRequest ) );
      }
      // valid permission
      else {
        var teamUser = await _teamService.GetTeamUserByUserId( leaveRequest.CreatedBy );
        if ( !Common.ValidRoleAdmin( positionUserLogin ) &&
        !( ( positionUserLogin == ( int ) Enums.UserPosition.TeamLead || positionUserLogin == ( int ) Enums.UserPosition.SubLead ) && ( teamUser != null && new HashSet<long>( teamUser.Select( x => x.TeamId ) ).SetEquals( teamUserLogin ) ) ) ) {
          errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.LeaveRequest ) );
        }
        else { // valid còn quỹ leave còn để nghỉ ko
          var remainLeave = await _leaveService.GetRemainLeave( leaveRequest.CreatedBy );
          if ( isApprove && leaveRequest.Type == ( int ) Enums.LeaveType.Annual && leaveRequest.Hour > remainLeave.Annual * 8 ) {
            errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.OverRemainLeaveError, DisplayNameResource.LeaveRequest, Enum.GetName( Enums.LeaveType.Annual ), remainLeave.Annual, DisplayNameResource.Hours ) );
          }
        }
      }
      return errors;
    }
  }
}
