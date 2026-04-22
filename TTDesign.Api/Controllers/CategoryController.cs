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
  /// APIs của Category Controller
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class CategoryController : ControllerBase
  {
    private readonly ICategoryService _categoryService;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

    public CategoryController( ICategoryService categorytService, IOptions<ApiBehaviorOptions> apiBehaviorOptions )
    {
      _categoryService = categorytService;
      _apiBehaviorOptions = apiBehaviorOptions;
    }

    /// <summary>
    /// [Personal - Option] Lấy danh sách thu gọn Category
    /// </summary>
    /// <remarks>
    /// - Danh sách trả về phụ thuộc vào team của User Login
    /// - Dùng cho các TH:
    ///     - view timesheet detail, khi thực hiện work log, chọn category
    /// </remarks>
    /// <returns></returns>
    /// <response code="200">Danh sách option</response>
    [HttpGet( "GetOption" )]
    [Authorize( Policy = Policies.CATEGORY_GET_OPTION )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<CategoryOption> ) )]
    public async Task<IEnumerable<CategoryOption>> GetOption()
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var teamUserLogin = ( long ) 0;
      var result = new List<CategoryOption>();
      if ( parts.Length > 1 && parts.Any( x => x != Enums.TEAM_HR.ToString() ) ) {
        foreach ( var item in parts ) {
          var teamId = long.Parse( item );
          var teamUserOptions = await _categoryService.GetOption( !Common.ValidRoleAdmin( positionUserLogin ) ? teamId : null );
          result.AddRange( teamUserOptions );
        }
        return result;
      }
      else {
        teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
        return await _categoryService.GetOption( !Common.ValidRoleAdmin( positionUserLogin ) ? teamUserLogin : null );
      }
    }

    /// <summary>
    /// [Admin - List] Lấy danh sách Category
    /// </summary>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView" )]
    [Authorize( Policy = Policies.CATEGORY_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<CategoryResponse> ) )]
    public async Task<IEnumerable<CategoryResponse>> GetListView()
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var teamUserLogin = ( long ) 0;
      var result = new List<CategoryResponse>();
      if ( parts.Length > 1 && parts.Any( x => x != Enums.TEAM_HR.ToString() ) ) {
        foreach ( var item in parts ) {
          var teamId = long.Parse( item );
          var categories = await _categoryService.GetList( new BaseFilter() { TeamId = !Common.ValidRoleAdmin( positionUserLogin ) ? teamId : null } );
          result.AddRange( categories );
        }
        return result;

      }
      else {
        teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
        return await _categoryService.GetList( new BaseFilter() { TeamId = ( !Common.ValidRoleAdmin( positionUserLogin ) && teamUserLogin != Enums.TEAM_HR ) ? teamUserLogin : null } );
      }

    }

    // [Note]: Category Controller không có api Detail

    /// <summary>
    /// [Admin - Create] Tạo Category mới
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="200">ID của new Category</response>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPost( "Create" )]
    [Authorize( Policy = Policies.CATEGORY_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] CategoryResource resource )
    {
      //if ( !ModelState.IsValid ) {
      //  return BadRequest( ModelState.GetErrorMessages() );
      //}
      // valid
      var errors = await ValidateBeforeCreate( resource );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      // set
      resource.Id = null;
      if ( resource.TeamId == 0 ) {
        resource.TeamId = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // create
      var categoryId = await _categoryService.Create( resource, userLogin );
      return Ok( categoryId );
    }

    /// <summary>
    /// [Admin - Update] Cập nhật thông tin Category
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - ID không tồn tại
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPut( "Update" )]
    [Authorize( Policy = Policies.CATEGORY_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Update( [FromBody] CategoryResource resource )
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
      // set
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // update
      await _categoryService.Update( resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Admin - Delete] Xóa Category
    /// </summary>
    /// <param name="id" example="1">Category ID muốn xóa</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - ID không tồn tại
    /// - Category vẫn còn member
    /// </response>
    [HttpDelete( "Delete/{id}" )]
    [Authorize( Policy = Policies.CATEGORY_DELETE )]
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
      // delete
      await _categoryService.DeleteByCondition( t => t.Id == id );
      return Ok();
    }
    /// <summary>
    /// valid resource trước khi create
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeCreate( CategoryResource resource )
    {
      var errors = new Dictionary<string, string>();
      // valid permission
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();

      if ( await _categoryService.Exist( c => teamUserLogin.Any( x => x == c.TeamId ) && c.Name == resource.Name ) ) {
        errors.Add( nameof( CategoryResource.Name ), string.Format( ErrorMessageResource.HadExistError, DisplayNameResource.Category ) );
      }
      else if ( !teamUserLogin.Any( x => x == resource.TeamId ) && !Common.ValidRoleAdmin( positionUserLogin ) ) {
        errors.Add( nameof( CategoryResource.TeamId ), string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Category ) );
      }
      return errors;
    }

    /// <summary>
    /// valid resource trước khi update
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeUpdate( CategoryResource resource )
    {
      var errors = new Dictionary<string, string>();
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      // valid exist?
      if ( resource.Id is null ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Category ) );
      }
      // valid: check role team with category
      //else if ( Common.ValidRoleAdmin( positionUserLogin ) || await _categoryService.Exist( c => c.Id == resource.Id && c.TeamId != teamUserLogin ) ) {
      //  errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Category ) );
      //}
      else if ( await _categoryService.Exist( c => teamUserLogin.Any( x => x == c.TeamId ) && c.Id != resource.Id && c.Name == resource.Name ) ) {
        errors.Add( nameof( CategoryResource.Name ), string.Format( ErrorMessageResource.HadExistError, DisplayNameResource.Category ) );
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
      // var teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
      // check category id
      if ( !await _categoryService.Exist( t => t.Id == id ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Category ) );
      }
      // valid: check role team with category
      //else if ( Common.ValidRoleAdmin( positionUserLogin ) || await _categoryService.Exist( c => c.Id == id && c.TeamId != teamUserLogin ) ) {
      //  errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Category ) );
      //}
      // check field isUsing true or check DB timesheet had using category
      else if ( await _categoryService.CheckCategoryBeforeDelete( id ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.HadUsing, DisplayNameResource.Category ) );
      }
      return errors;
    }
  }
}
