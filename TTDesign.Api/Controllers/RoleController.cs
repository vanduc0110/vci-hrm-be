using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers
{
  /// <summary>
  /// APIs của Role Controller
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class RoleController : ControllerBase
  {
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;
    private readonly ILogger<RoleController> _logger;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public RoleController( IRoleService roleService,
      IMapper mapper, ILogger<RoleController> logger,
      IOptions<ApiBehaviorOptions> apiBehaviorOptions,
      UserManager<User> userManager, RoleManager<Role> roleManager )
    {
      _roleService = roleService;
      _mapper = mapper;
      _logger = logger;
      _apiBehaviorOptions = apiBehaviorOptions;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    /// <summary>
    /// [Admin - Option] Lấy danh sách thu gọn Role
    /// </summary>
    /// Dùng cho các TH:
    ///     - Tạo/sửa User
    /// <returns></returns>
    [HttpGet( "GetOption" )]
    [Authorize( Policy = Policies.ROLE_GET_OPTION )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<RoleOption> ) )]
    public async Task<IEnumerable<RoleOption>> GetOption()
    {
      return await _roleService.GetOption();
    }

    /// <summary>
    /// [Admin - List] Lấy danh sách Role
    /// </summary>
    /// <returns></returns>
    [HttpGet( "GetListView" )]
    [Authorize( Policy = Policies.ROLE_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<RoleResponse> ) )]
    public async Task<IEnumerable<RoleResponse>> GetListView()
    {
      return await _roleService.GetList( new BaseFilter() );
    }

    /// <summary>
    /// [Admin - Detail] Lấy thông tin chi tiết của Role
    /// </summary>
    /// <param name="id" example="1">Role ID</param>
    /// <returns></returns>
    [HttpGet( "GetDetail/{id}" )]
    [Authorize( Policy = Policies.ROLE_GET_DETAIL )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( RoleDetailResponse ) )]
    [SwaggerResponse( StatusCodes.Status404NotFound )]
    public async Task<IActionResult> GetDetail( long id )
    {
      var resource = await _roleService.GetDetail( id );
      if ( resource == null ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Role ) );
      }
      return Ok( resource );
    }

    /// <summary>
    /// [Admin - Other] Liệt kê danh sách tất cả các Role phân quyền trong hệ thống
    /// </summary>
    /// <returns></returns>
    [HttpGet( "GetCollectionRole" )]
    [Authorize( Policy = Policies.ROLE_GET_COLLECTION )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( Dictionary<string, RoleModelView []> ) )]
    public IActionResult GetCollectionRole()
    {
      return Ok( _mapper.Map<Dictionary<string, RoleModelView []>>( Common.CollectionRole() ) );
    }

    /// <summary>
    /// [Admin - Create] Tạo Role mới
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="200">ID của new Role</response>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// </response>
    [HttpPost( "Create" )]
    [Authorize( Policy = Policies.ROLE_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] RoleResource resource )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      resource.Id = null;
      // create
      var teamId = await _roleService.Create( resource, userLogin );
      return Ok( teamId );
    }

    /// <summary>
    /// [Update] Cập nhật thông tin Role
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// - Require
    /// </response>
    [HttpPut( "Update" )]
    [Authorize( Policy = Policies.ROLE_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Update( [FromBody] RoleResource resource )
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
      await _roleService.Update( resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Delete] Xóa Role
    /// </summary>
    /// <param name="id" example="1">ID Role muốn xóa</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// </response>
    [HttpDelete( "Delete/{id}" )]
    [Authorize( Policy = Policies.ROLE_DELETE )]
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
      var role = await _roleService.GetByCondition( r => r.Id == id );
      role = await _roleManager.FindByNameAsync( role!.Name );
      await _roleManager.DeleteAsync( role );
      return Ok();
    }

    /// <summary>
    /// valid resource trước khi update
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeUpdate( RoleResource resource )
    {
      var errors = new Dictionary<string, string>();
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      // valid
      if ( resource.Id is null ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Role ) );
      }
      // valid exist
      var old = await _roleService.GetByCondition( r => r.Id == resource.Id );
      if ( old == null ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Role ) );
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
      // validate role exist
      var role = await _roleService.GetByCondition( r => r.Id == id );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      if ( role == null ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Role ) );
      }
      else if ( role.Type == ( int ) Enums.RoleType.Default ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Role ) );
      }
      else if ( ( await _userManager.GetUsersInRoleAsync( role.Name ) ).ToList().Count > 0 ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.HadUsing, DisplayNameResource.Role ) );
      }
      return errors;
    }
  }
}
