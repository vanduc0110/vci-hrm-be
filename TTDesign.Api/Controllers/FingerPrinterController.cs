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
  /// APIs của FingerPrinter Controller
  /// 
  /// - Timesheet và FingerPrinter là một cặp thông tin đi cùng nhau
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class FingerPrinterController : ControllerBase
  {
    private readonly IFingerPrinterService _fingerPrinterService;
    private readonly ITimesheetService _timesheetService;
    private readonly ILogger<FingerPrinterController> _logger;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

    public FingerPrinterController( IFingerPrinterService fingerPrinterService,
      ITimesheetService timesheetService,
      ILogger<FingerPrinterController> logger, IOptions<ApiBehaviorOptions> apiBehaviorOptions )
    {
      _fingerPrinterService = fingerPrinterService;
      _logger = logger;
      _apiBehaviorOptions = apiBehaviorOptions;
      _timesheetService = timesheetService;
    }

    /// <summary>
    /// [Admin - List] Lấy danh sách FingerPrinter (theo Team)
    /// </summary>
    /// <param name="from" example="2024-01-01">định dạng: yyyy-MM-dd</param>
    /// <param name="to" example="2024-01-01">định dạng: yyyy-MM-đd</param>
    /// <param name="userId" example="1">user ID chỉ định tìm kiếm</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView/{from}/{to}/{userId}" )]
    [Authorize( Policy = Policies.FINGERPRINTER_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<FingerPrinterResponse> ) )]
    public async Task<IEnumerable<FingerPrinterResponse>> GetListView( DateTime from, DateTime to, long userId )
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var teamUserLogin = ( long ) 0;
      var result = new List<FingerPrinterResponse>();
      if ( parts.Length > 1 ) {
        foreach ( var p in parts ) {
          var teamId = long.Parse( p );
          var res1 = await _fingerPrinterService.GetList( new BaseFilter()
          {
            Start = from,
            End = to,
            UserId = userId,
            TeamId = Common.ValidRoleAdmin( positionUserLogin ) || teamId == Enums.TEAM_HR ? null : teamUserLogin
          } );
          result.AddRange( res1.ToList() );
        }
        return result;
      }
      else {
        teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
        return await _fingerPrinterService.GetList( new BaseFilter()
        {
          Start = from,
          End = to,
          UserId = userId,
          TeamId = Common.ValidRoleAdmin( positionUserLogin ) || teamUserLogin == Enums.TEAM_HR ? null : teamUserLogin
        } );
      }

    }

    /// <summary>
    /// [Admin - Create] Tạo Timesheet mới (theo Team)
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <remarks>Backup cho TH hệ thống không tự động tạo timesheet + finger printer cho user</remarks>
    /// <response code="200"></response>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPost( "Create" )]
    [Authorize( Policy = Policies.FINGERPRINTER_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] FingerPrinterResource resource )
    {
      //if ( !ModelState.IsValid ) {
      //  return BadRequest( ModelState.GetErrorMessages() );
      //}
      resource.Id = null;
      // valid
      var errors = await ValidateBeforeCreate( resource );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // create
      await _fingerPrinterService.Create( resource, userLogin );
      return Ok();
      //}
    }

    /// <summary>
    /// [Admin - Update] Cập nhật thông tin FingerPrinter
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
    [Authorize( Policy = Policies.FINGERPRINTER_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Update( [FromBody] FingerPrinterResource resource )
    {
      //// valid
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
      await _fingerPrinterService.Update( resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Admin - Delete] Xóa dữ liệu chấm công
    /// </summary>
    /// <param name="id" example="1">ID Finger Printer muốn xóa</param>
    /// <remarks>thực tế xóa chỉ là xóa value field chứ ko xóa record DB</remarks>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// - Had member belong Team
    /// </response>
    [HttpDelete( "Delete/{id}" )]
    [Authorize( Policy = Policies.FINGERPRINTER_DELETE )]
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

      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _fingerPrinterService.Delete( id, userLogin );
      return Ok();
    }
    [HttpPost]
    [Route( "update-excel" )]
    [Consumes( "multipart/form-data" )]
    [Authorize( Policy = Policies.FINGERPRINTER_CREATE )]
    public async Task<IActionResult> UpdateFingureExcel( IFormFile file )
    {
      if ( file == null || file.Length == 0 ) {
        return BadRequest();
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _fingerPrinterService.UpdateFingureExcel( file, userLogin );
      return Ok();
    }
    /// <summary>
    /// valid resource trước khi create
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeCreate( FingerPrinterResource resource )
    {
      var errors = new Dictionary<string, string>();
      var todayVn        = TimeZoneInfo.ConvertTimeFromUtc( DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById( "SE Asia Standard Time" ) ).Date;
      var dateValidMinimum = todayVn.AddMonths( -1 );
      dateValidMinimum = dateValidMinimum.AddDays( 1 - dateValidMinimum.Day );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      // valid
      if ( resource.DateSelect is null ) {
        errors.Add( nameof( FingerPrinterResource.DateSelect ), string.Format( ErrorMessageResource.DateWrongFormat, DisplayNameResource.FingerPrinterDate ) );
      }
      // check record DB had exist
      else if ( await _timesheetService.Exist( t => t.UserId == resource.UserId && t.Date == resource.DateSelect ) ) {
        errors.Add( nameof( FingerPrinterResource.DateSelect ), string.Format( ErrorMessageResource.HadExistError, DisplayNameResource.FingerPrinterDate ) );
      }
      // check locked date
      else if ( await _fingerPrinterService.CheckTimesheetHadLock( 0, resource.DateSelect ) ) {
        errors.Add( nameof( FingerPrinterResource.DateSelect ), string.Format( ErrorMessageResource.HadLocked, DisplayNameResource.FingerPrinterDate ) );
      }
      else if ( resource.DateSelect < dateValidMinimum || resource.DateSelect > todayVn ) {
        errors.Add( nameof( FingerPrinterResource.DateSelect ), string.Format( ErrorMessageResource.DateRangeError, DisplayNameResource.FingerPrinterDate ) );
      }
      // kiểm tra user login ko là admin/HR thì phải tạo/sửa đúng team của mình thôi
      else if ( !Common.ValidRoleAdmin( positionUserLogin ) && !teamUserLogin.Contains( Enums.TEAM_HR ) && !new HashSet<long>( resource.TeamIds ).SetEquals( teamUserLogin ) ) {
        errors.Add( nameof( FingerPrinterResource.UserId ), string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.FingerPrinterUser ) );
      }
      else {
        var date = ( ( DateTime ) resource.DateSelect! ).ToString( Enums.DATE_FORMAT );
        DateTime dateTimeTry;
        // valid time in
        if ( string.IsNullOrEmpty( resource.TimeIn ) || !DateTime.TryParse( date + " " + resource.TimeIn, out dateTimeTry ) ) {
          resource.DateTimeIn = DateTime.SpecifyKind( ( DateTime ) resource.DateSelect, DateTimeKind.Utc );
        }
        else {
          resource.DateTimeIn = DateTime.SpecifyKind( dateTimeTry, DateTimeKind.Utc );
        }
        // valid time out
        if ( string.IsNullOrEmpty( resource.TimeIn ) || string.IsNullOrEmpty( resource.TimeOut ) || !DateTime.TryParse( date + " " + resource.TimeOut, out dateTimeTry ) ) {
          resource.DateTimeOut = DateTime.SpecifyKind( ( DateTime ) resource.DateSelect, DateTimeKind.Utc );
        }
        else {
          resource.DateTimeOut = DateTime.SpecifyKind( dateTimeTry, DateTimeKind.Utc );
        }
        if ( resource.DateTimeOut < resource.DateTimeIn ) {
          errors.Add( nameof( FingerPrinterResource.DateSelect ), string.Format( ErrorMessageResource.DatePastError, DisplayNameResource.FingerPrinterDate ) );
        }
      }
      return errors;
    }

    /// <summary>
    /// valid resource trước khi update
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeUpdate( FingerPrinterResource resource )
    {
      var errors = new Dictionary<string, string>();
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      if ( resource.Id is null ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.FingerPrinter ) );
      }
      else if ( await _fingerPrinterService.CheckTimesheetHadLock( ( long ) resource.Id ) ) {
        errors.Add( nameof( FingerPrinterResource.DateSelect ), string.Format( ErrorMessageResource.HadLocked, DisplayNameResource.FingerPrinterDate ) );
      }
      else {
        var old = await _fingerPrinterService.GetByCondition( f => f.Id == resource.Id );
        var date = old!.DateIn.Date.ToString( Enums.DATE_FORMAT );
        var todayVnUpd       = TimeZoneInfo.ConvertTimeFromUtc( DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById( "SE Asia Standard Time" ) ).Date;
        var dateValidMinimum = todayVnUpd.AddMonths( -1 );
        dateValidMinimum = dateValidMinimum.AddDays( 1 - dateValidMinimum.Day );
        // check record DB had exist
        if ( !await _timesheetService.Exist( t => t.UserId == resource.UserId && t.Date == old!.DateIn.Date ) ) {
          errors.Add( nameof( FingerPrinterResource.DateSelect ), string.Format( ErrorMessageResource.HadExistError, DisplayNameResource.FingerPrinterDate ) );
        }
        else if ( old!.DateIn.Date < dateValidMinimum || old!.DateIn.Date > todayVnUpd ) {
          errors.Add( nameof( FingerPrinterResource.DateSelect ), string.Format( ErrorMessageResource.DateRangeError, DisplayNameResource.FingerPrinterDate ) );
        }
        // kiểm tra user login ko là admin/HR thì phải tạo/sửa đúng team của mình thôi
        else if ( !Common.ValidRoleAdmin( positionUserLogin ) && !teamUserLogin.Contains( Enums.TEAM_HR ) && !new HashSet<long>( resource.TeamIds ).SetEquals( teamUserLogin ) ) {
          errors.Add( nameof( FingerPrinterResource.UserId ), string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.FingerPrinterUser ) );
        }
        else {
          DateTime dateTimeTry;
          // valid time in
          if ( string.IsNullOrEmpty( resource.TimeIn ) || !DateTime.TryParse( date + " " + resource.TimeIn, out dateTimeTry ) ) {
            errors.Add( nameof( FingerPrinterResource.TimeIn ), string.Format( ErrorMessageResource.DateWrongFormat, DisplayNameResource.FingerPrinterTimeIn ) );
          }
          else {
            resource.DateTimeIn = DateTime.SpecifyKind( dateTimeTry, DateTimeKind.Utc );
          }
          // valid time out
          if ( string.IsNullOrEmpty( resource.TimeOut ) || !DateTime.TryParse( date + " " + resource.TimeOut, out dateTimeTry ) ) {
            errors.Add( nameof( FingerPrinterResource.TimeOut ), string.Format( ErrorMessageResource.DateWrongFormat, DisplayNameResource.FingerPrinterTimeOut ) );
          }
          else {
            resource.DateTimeOut = DateTime.SpecifyKind( dateTimeTry, DateTimeKind.Utc );
          }
          if ( resource.DateTimeOut < resource.DateTimeIn ) {
            errors.Add( nameof( FingerPrinterResource.DateSelect ), string.Format( ErrorMessageResource.DatePastError, DisplayNameResource.FingerPrinterDate ) );
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
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      // valid exist
      if ( !await _fingerPrinterService.Exist( g => g.Id == id ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.FingerPrinter ) );
      }
      // valid ts locked
      else if ( await _fingerPrinterService.CheckTimesheetHadLock( id ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.HadLocked, DisplayNameResource.FingerPrinterDate ) );
      }
      else {
        var old = await _fingerPrinterService.GetByCondition( f => f.Id == id );
        var teamIds = await _timesheetService.GetTeamId( old!.TimesheetId );
        // kiểm tra user login ko là admin/HR thì phải tạo/sửa đúng team của mình thôi
        if ( !Common.ValidRoleAdmin( positionUserLogin ) && !teamUserLogin.Contains( Enums.TEAM_HR ) && !new HashSet<long>( teamIds ).SetEquals( teamUserLogin ) ) {
          errors.Add( nameof( FingerPrinterResource.UserId ), string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.FingerPrinterUser ) );
        }
        var dateValidMinimum = DateTime.UtcNow.AddMonths( -1 ).Date;
        dateValidMinimum = dateValidMinimum.AddDays( 1 - dateValidMinimum.Day );
        if ( old!.DateIn.Date < dateValidMinimum || old!.DateIn.Date >= DateTime.UtcNow.Date ) {
          errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.DateRangeError, DisplayNameResource.FingerPrinterDate ) );
        }
      }
      return errors;
    }
  }
}
