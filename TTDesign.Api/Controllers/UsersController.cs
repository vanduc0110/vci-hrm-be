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
  /// APIs của User Controller
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

    public UsersController( IUserService userService,
      ILogger<UsersController> logger,
      IOptions<ApiBehaviorOptions> apiBehaviorOptions )
    {
      _userService = userService;
      _logger = logger;
      _apiBehaviorOptions = apiBehaviorOptions;
    }

    /// <summary>
    /// [Personal - Other] Lấy thông tin khởi tạo dashboard: users
    /// </summary>
    /// <returns></returns>
    [HttpGet( "GetDashboardUser" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<DashboardUser> ) )]
    public async Task<IEnumerable<DashboardUser>> GetDashboardUser()
    {
      return await _userService.GetDashboardUser();
    }

    /// <summary>
    /// [Personal - Detail] Xem thông tin User khác từ view của User Staff
    /// </summary>
    /// <param name="id" example="1">User ID</param>
    /// <returns></returns>
    [HttpGet( "GetDetailOtherUser/{id}" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( DetailOtherUser ) )]
    [SwaggerResponse( StatusCodes.Status404NotFound )]
    public async Task<IActionResult> GetDetailOtherUser( long id )
    {
      if ( id == Enums.SYSTEM_ID ) {
        return NotFound( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.User ) );
      }
      var resource = await _userService.GetDetailOtherUser( id );
      if ( resource == null ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ) );
      }
      return Ok( resource );
    }

    /// <summary>
    /// [Admin - Option] Lấy danh sách thu gọn User, theo team
    /// </summary>
    /// <remarks>
    /// Dùng cho các TH:
    ///     - Chọn User khi tạo Group
    ///     - filter view FingerPrinter (chú ý user ko là admin thì chỉ xem được user trong team mình)
    ///     - filter trong timesheet other (chú ý user ko là admin thì chỉ xem được user trong team mình)
    /// </remarks>
    /// <returns></returns>
    /// <response code="200">Danh sách option</response>
    [HttpGet( "GetOption" )]
    [Authorize( Policy = Policies.USER_GET_OPTION )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<UserOption> ) )]
    public async Task<IEnumerable<UserOption>> GetOption()
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamStr = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )?.Value ?? "";
      var teamUserLogins = teamStr.Length > 0 ? teamStr.Split( "," ).Select( long.Parse ).ToArray() : Array.Empty<long>();
      bool hasPayrollFull = HttpContext.User.HasClaim( c => c.Type == Roles.ROLE_ADMIN_PAYROLL && c.Value == Roles.PERMISSION_CREATE );
      bool isLeadRole = positionUserLogin >= ( int ) Enums.UserPosition.TeamLead && positionUserLogin <= ( int ) Enums.UserPosition.PM;
      var list = new List<UserOption>();
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && ( isLeadRole || ( !teamUserLogins.Contains( Enums.TEAM_HR ) && !hasPayrollFull ) ) ) {
        foreach ( var teamId in teamUserLogins ) {
          var res = ( await _userService.GetOption() ).Where( u => u.TeamIds.Contains( teamId ) );
          list.AddRange( res );
        }
        return list.GroupBy( u => u.Id ).Select( g => g.First() );
      }
      else {
        return await _userService.GetOption();
      }
    }

    /// <summary>
    /// [Admin - List] Lấy danh sách User
    /// </summary>
    /// <remarks>Danh sách tất cả User với tất cả trạng thái</remarks>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView" )]
    [Authorize( Policy = Policies.USER_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<UserResponse> ) )]
    public async Task<IEnumerable<UserResponse>> GetListView()
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamStr = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )?.Value ?? "";
      var teamUserLogins = teamStr.Length > 0 ? teamStr.Split( "," ).Select( long.Parse ).ToArray() : Array.Empty<long>();
      bool isLeadRole = positionUserLogin >= ( int ) Enums.UserPosition.TeamLead && positionUserLogin <= ( int ) Enums.UserPosition.PM;
      var list = new List<UserResponse>();
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && ( isLeadRole || !teamUserLogins.Contains( Enums.TEAM_HR ) ) ) {
        foreach ( var teamId in teamUserLogins ) {
          var res = await _userService.GetList( new BaseFilter() { TeamId = teamId } );
          list.AddRange( res );
        }
        return list.GroupBy( u => u.Id ).Select( g => g.First() );
      }
      else {
        return await _userService.GetList( new BaseFilter() { TeamId = null } );
      }
    }

    /// <summary>
    /// [Admin - List] Lấy danh sách User
    /// </summary>
    /// <remarks>Danh sách tất cả User với tất cả trạng thái</remarks>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListUserProject" )]
    [Authorize( Policy = Policies.USER_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<UserResponse> ) )]
    public async Task<IEnumerable<UserResponse>> GetListUserProject()
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      return await _userService.GetList( new BaseFilter() );
    }

    /// <summary>
    /// [Admin - Detail] Lấy thông tin chi tiết của User
    /// </summary>
    /// <param name="id" example="1">User ID</param>
    /// <returns></returns>
    [HttpGet( "GetDetail/{id}" )]
    [Authorize( Policy = Policies.USER_GET_DETAIL )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( UserDetailResponse ) )]
    [SwaggerResponse( StatusCodes.Status404NotFound )]
    public async Task<IActionResult> GetDetail( long id )
    {
      if ( id == Enums.SYSTEM_ID ) {
        return NotFound( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.User ) );
      }
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLoginStrs = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLoginStrs.Split( "," );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      var resource = await _userService.GetDetail( id );
      // không tìm thấy user hoặc user ko là admin và khác team với user lấy thông tin
      if ( resource == null || ( !Common.ValidRoleAdmin( positionUserLogin ) && !teamUserLogins.Contains( Enums.TEAM_HR ) && !new HashSet<long>( resource.Teams.Select( x => x.TeamId!.Value ) ).SetEquals( teamUserLogins ) ) ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ) );
      }
      return Ok( resource );
    }

    /// <summary>
    /// [Other] Lấy thông tin UserLogin
    /// </summary>
    /// <returns></returns>
    [HttpGet( "GetCredential" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( UserDetailResponse ) )]
    public async Task<IActionResult> GetCredential()
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var resource = await _userService.GetDetail( userLogin );
      if ( resource == null ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ) );
      }
      return Ok( resource );
    }

    /// <summary>
    /// [Admin - Create] Tạo User mới
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="200">ID của new User</response>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// - Dữ liệu nhập không tồn tại
    /// </response>
    [HttpPost( "Create" )]
    [Authorize( Policy = Policies.USER_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] UserResource resource )
    {
      // valid
      var errors = await ValidateBeforeCreate( resource );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      // user ko là admin thì phạm vi ảnh hưởng trong team của mình
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamStr      = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var teamUserIds  = teamStr.Split( ',' ).Select( long.Parse ).ToArray();
      var teamUserLogin = teamUserIds [ 0 ];
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && !teamUserIds.Contains( Enums.TEAM_HR ) ) {
        resource.TeamIds = teamUserIds;
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // create
      var userId = await _userService.Create( resource, userLogin );
      return Ok( userId );

    }

    /// <summary>
    /// [Admin - Update] Cập nhật thông tin User
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// - Dữ liệu nhập không tồn tại
    /// </response>
    [HttpPut( "Update" )]
    [Authorize( Policy = Policies.USER_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Update( [FromBody] UserResource resource )
    {
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
      await _userService.Update( resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Personal - Update] Cập nhật thông tin của chính mình
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="400"></response>
    [HttpPut( "SelfUpdate" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> SelfUpdate( [FromBody] YourSelfResource resource )
    {
      resource.Id = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // valid: không được sửa user system
      if ( resource.Id == Enums.SYSTEM_ID ) {
        ModelState.AddModelError( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.User ) );
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      // update
      await _userService.SelfUpdate( resource );
      return Ok();
    }

    /// <summary>
    /// [Admin - Other] Chuyển trạng thái của User (Active/Inactive)
    /// </summary>
    /// <param name="id" example="1">User ID</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// </response>
    [HttpPut( "ChangeStatus/{id}" )]
    [Authorize( Policy = Policies.USER_CHANGE_STATUS )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> ChangeStatus( long id )
    {
      // valid: không được sửa user system
      if ( id == Enums.SYSTEM_ID ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ) );
      }
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
      // không tìm thấy user hoặc user ko là admin và khác team với user lấy thông tin
      if ( !await _userService.Exist( u => u.Id == id ) || ( !Common.ValidRoleAdmin( positionUserLogin ) && teamUserLogin != Enums.TEAM_HR && await _userService.Exist( u => u.Id == id && !u.TeamUsers.Select( x => x.TeamId ).Contains( teamUserLogin ) ) ) ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ) );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // change status
      await _userService.ChangeStatus( id, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Admin - Other] Reset password
    /// </summary>
    /// <param name="id" example="1">User ID</param>
    /// <remark>User có quyền, thực hiện reset password về default</remark>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// </response>
    [HttpPut( "ResetPassword/{id}" )]
    [Authorize( Policy = Policies.USER_RESET_PASS )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> ResetPassword( long id )
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
      var resource = await _userService.GetDetail( id );
      // không tìm thấy user hoặc user ko là admin và khác team với user lấy thông tin
      if ( resource == null || ( !Common.ValidRoleAdmin( positionUserLogin ) && teamUserLogin != Enums.TEAM_HR && !resource.Teams.Select( x => x.TeamId ).Contains( teamUserLogin ) ) ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ) );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // reset password
      await _userService.ResetPassword( id, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Personal - Other] Tự mình đổi password
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    [HttpPut( "ChangePassword" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> ChangePassword( [FromBody] UserAndPassResource resource )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      if ( !await _userService.Exist( u => u.Id == userLogin ) ) {
        return BadRequest( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ) );
      }
      // change password
      if ( !await _userService.ChangePassword( resource, userLogin ) ) {
        return BadRequest( ErrorMessageResource.ChangePasswordFail );
      }
      return Ok();
    }

    /// <summary>
    /// [Personal - Other] Upload avatar
    /// </summary>
    /// <remarks>File upload type: png, img</remarks>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <response code="200">Url Avatar</response>
    [HttpPost( "UploadAvatar" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( string ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    [Consumes( "multipart/form-data" )]
    //[Produces( "multipart/form-data" )]
    public async Task<IActionResult> UploadAvatar( IFormFile file )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // valid file
      if ( file is null || file.Length == 0 ) {
        return BadRequest( ErrorMessageResource.AvatarNull );
      }
      if ( file.Length > Enums.MAX_AVATAR_SIZE ) {
        return BadRequest( ErrorMessageResource.AvatarOverSize );
      }
      if ( !Enums.AVATAR_EXTENSION.Contains( Path.GetExtension( file.FileName ) ) ) {
        return BadRequest( ErrorMessageResource.AvatarExtension );
      }
      var fileName = $"{userLogin}{Path.GetExtension( file.FileName )}";
      var path = Path.GetFullPath( Path.Combine( Directory.GetCurrentDirectory(), "Upload", "Images" ) );
      // create folder storage
      if ( !Directory.Exists( path ) ) {
        Directory.CreateDirectory( path );
      }
      // delete exist old image
      if ( System.IO.File.Exists( Path.Combine( path, fileName ) ) ) {
        System.IO.File.Delete( Path.Combine( path, fileName ) );
      }
      // save new avatar
      using ( var fileStream = new FileStream( Path.Combine( path, fileName ), FileMode.Create ) ) {
        await file.CopyToAsync( fileStream );
      }
      // update DB
      await _userService.UpdateAvatar( userLogin, $"/Upload/Images/{fileName}" );
      return Ok( $"/Upload/Images/{fileName}" );
    }

    /// <summary>
    /// [Personal - Other] Cập nhật trạng thái User
    /// </summary>
    /// <param name="state" example="Available"></param>
    /// <returns></returns>
    [HttpPut( "ChangeState" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK )]
    public async Task<IActionResult> ChangeState( [FromBody] UserChangeState state )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      if ( !Enum.IsDefined( typeof( Enums.UserState ), state.State ) ) {
        return BadRequest( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.UserState ) );
      }
      await _userService.ChangeState( userLogin, ( int ) Enum.Parse( typeof( Enums.UserState ), state.State ) );
      return Ok();
    }

    /// <summary>
    /// [Personal - Update] Edit Setting 
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    [HttpPut( "EditSetting" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK )]
    public async Task<IActionResult> EditSetting( [FromBody] UserSettingResource resource )
    {
      if ( !ModelState.IsValid ) {
        return BadRequest( ModelState.GetErrorMessages() );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      resource.Id = userLogin;
      await _userService.UpdateSetting( resource );
      return Ok();
    }

    /// <summary>
    /// [Personal - Detail] Get Setting 
    /// </summary>
    /// <returns></returns>
    [HttpGet( "GetSetting" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( UserSettingResponse ) )]
    public async Task<UserSettingResponse> GetSetting()
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return await _userService.GetSetting( userLogin );
    }

    /// <summary>
    /// [GetOption] Danh sách chung đối tượng chọn: user, team
    /// </summary>
    /// <returns></returns>
    [HttpGet( "GetDynamicOption" )]
    [Authorize( Policy = Policies.USER_GET_DYNAMIC_OPTION )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    public async Task<List<DynamicOption>> GetDynamicOption()
    {
      return await _userService.GetDynamicOption( null );
    }

    /// <summary>
    /// lấy danh sách user có position PM
    /// </summary>
    /// <returns></returns>
    [HttpGet( "GetProjectManager" )]
    [Authorize( Policy = Policies.USER_GET_PM_OPTION )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<UserOption> ) )]
    public async Task<IEnumerable<UserOption>> GetListPm()
    {
      return await _userService.GetUserByPosition( ( long ) Enums.UserPosition.PM );
    }

    /// <summary>
    /// valid resource trước khi create
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeCreate( UserResource resource )
    {
      var errors = new Dictionary<string, string>();
      return errors;
    }

    /// <summary>
    /// valid resource trước khi update
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeUpdate( UserResource resource )
    {
      var errors = new Dictionary<string, string>();
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamStr     = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var teamUserIds = teamStr.Split( ',' ).Select( long.Parse ).ToArray();
      // valid: không được sửa user system
      if ( resource.Id == Enums.SYSTEM_ID ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.User ) );
      }
      // valid: khi update, bắt buộc phải truyền User ID lên
      else if ( resource.Id is null ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ) );
      }
      // Valid: User login không là admin, User login không được sửa User team khác
      else if ( !Common.ValidRoleAdmin( positionUserLogin ) && !teamUserIds.Contains( Enums.TEAM_HR ) && !resource.TeamIds.Any( t => teamUserIds.Contains( t ) ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.User ) );
      }
      return errors;
    }
  }
}