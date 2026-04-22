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
  /// APIs của Overtime Controller
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class WfhController : ControllerBase
  {
    private readonly IWfhService _wfhService;
    private readonly ITeamService _teamService;
    private readonly ITimesheetService _timesheetService;
    private readonly ILogger<WfhController> _logger;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

    public WfhController( IWfhService wfhService, ITeamService teamService, ITimesheetService timesheetService,
      ILogger<WfhController> logger,
    IOptions<ApiBehaviorOptions> apiBehaviorOptions )
    {
      _wfhService = wfhService;
      _logger = logger;
      _apiBehaviorOptions = apiBehaviorOptions;
      _teamService = teamService;
      _timesheetService = timesheetService;
    }

    /// <summary>
    /// [Personal - List] Lấy danh sách WFH request
    /// </summary>
    /// <param name="year" example="2024">năm chỉ định</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetRequestListView/{year}" )]
    [Authorize( Policy = Policies.WFH_GET_REQUEST_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<WfhResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetRequestListView( long year )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return Ok( await _wfhService.GetRequestList( userLogin, year ) );
    }

    /// <summary>
    /// [Admin - List] Lấy danh sách request wfh mà user login quản lý team
    /// </summary>
    /// <param name="year" example="2024">năm chỉ định</param>
    /// <param name="month" example="1">tháng chỉ định</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView/{year}/{month}" )]
    [Authorize( Policy = Policies.WFH_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<WfhResponse> ) )]
    public async Task<IActionResult> GetListView( long year, long month )
    {
      DateTime datetimeTry;
      if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
        return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
      }
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var teamUserLoginStrs = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLoginStrs.Split( "," );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      var list = new List<WfhResponse>();
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && parts.Any( x => x != Enums.TEAM_HR.ToString() ) ) {
        foreach ( var teamId in teamUserLogins ) {
          var res = await _wfhService.GetList( new BaseFilter()
          {
            TeamId = teamId,
            UserId = userLogin,
            Year = year,
            Month = month,
          } );
          list.AddRange( res );
        }
        return Ok( list );
      }
      else {
        return Ok( await _wfhService.GetList( new BaseFilter()
        {
          TeamId = null,
          UserId = userLogin,
          Year = year,
          Month = month,
        } ) );
      }

    }

    /// <summary>
    /// [Personal - Create] Tạo request WFH mới
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="200">ID của new request WFH</response>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPost( "Create" )]
    [Authorize( Policy = Policies.WFH_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] WfhResource resource )
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      if ( Common.ValidRoleAdmin( positionUserLogin ) ) {
        return BadRequest( ErrorMessageResource.UserNotPermission );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      resource.Id = null;
      // create
      var overtimeId = await _wfhService.Create( resource, userLogin );
      return Ok( overtimeId );
    }

    /// <summary>
    /// [Personal - Update] Cập nhật request WFH
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
    [Authorize( Policy = Policies.WFH_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Update( [FromBody] WfhResource resource )
    {
      if ( resource.Id is null ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Wfh ) );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // update
      await _wfhService.Update( resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Personal - Delete] Xóa request WFH
    /// </summary>
    /// <param name="id" example="1">ID request WFH muốn xóa</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// </response>
    [HttpDelete( "Delete/{id}" )]
    [Authorize( Policy = Policies.WFH_DELETE )]
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
      await _wfhService.DeleteByCondition( g => g.Id == id );
      return Ok();
    }

    /// <summary>
    /// [Admin - Other] Approve/Reject request WFH
    /// </summary>
    /// <param name="id" example="1">request Wfh ID</param>
    /// <param name="isApprove" example="true">true: approve, false: reject</param>
    /// <returns></returns>
    [HttpPost( "Approve/{id}/{isApprove}" )]
    [Authorize( Policy = Policies.WFH_APPROVE )]
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
      await _wfhService.Approve( id, isApprove, userLogin );
      return Ok();
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
      if ( !await _wfhService.Exist( g => g.Id == id && g.CreatedBy == userLogin ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Wfh ) );
      }
      // valid status 
      else if ( await _wfhService.Exist( g => g.Id == id && g.Status == ( int ) Enums.WfhRequestStatus.Approve ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Wfh ) );
      }
      return errors;
    }
    [HttpGet( "GetTotalWfhPending" )]
    [Authorize( Policy = Policies.WFH_GET_LIST )]
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
          totalLeaveRequests += await _wfhService.GetTotalWfhRequest( teamId );
        }
      }
      else {
        totalLeaveRequests = await _wfhService.GetTotalWfhRequest( null );
      }

      return Ok( totalLeaveRequests );
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
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      var wfhRequest = await _wfhService.GetByCondition( o => o.Id == id );
      // valid
      if ( wfhRequest is null ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Wfh ) );
      }
      // valid status 
      else if ( !( isApprove && wfhRequest.Status == ( int ) Enums.WfhRequestStatus.Pending ) && !( !isApprove && wfhRequest.Status != ( int ) Enums.WfhRequestStatus.Reject ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ChangeStatusError, DisplayNameResource.Wfh ) );
      }
      // valid ngày request đã bị lock timesheet chưa
      else if ( await _timesheetService.CheckTimesheetHadLock( wfhRequest.StartTime, wfhRequest.EndTime ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.HadLocked, DisplayNameResource.Wfh ) );
      }
      // valid permission
      else {
        var teamUser = await _teamService.GetTeamUserByUserId( wfhRequest.CreatedBy );
        if ( !Common.ValidRoleAdmin( positionUserLogin ) &&
          !( ( positionUserLogin == ( int ) Enums.UserPosition.TeamLead || positionUserLogin == ( int ) Enums.UserPosition.SubLead ) && new HashSet<long>( teamUser!.Select( x => x.TeamId ) ).SetEquals( teamUserLogin ) ) ) {
          errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Wfh ) );
        }
      }
      return errors;
    }
  }
}
