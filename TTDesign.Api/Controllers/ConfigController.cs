using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Services;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers;
/// <summary>
/// APIs của Config Controller
/// </summary>
[Route( "api/[controller]" )]
[ApiController]
[Authorize]
[Consumes( "application/json" )]
[Produces( "application/json" )]
public class ConfigController : ControllerBase
{
  private readonly IConfigService _configService;
  private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
  public ConfigController( IConfigService configService, IOptions<ApiBehaviorOptions> apiBehaviorOptions )
  {
    _configService = configService;
    _apiBehaviorOptions = apiBehaviorOptions;
  }
  /// <summary>
  /// [Admin - List] lấy danh sách Config kèm thông tin
  /// </summary>
  /// <returns></returns>
  [HttpGet( "GetListView" )]
  [Authorize( Policy = Policies.CONFIG_GET_LIST )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<ConfigResponse> ) )]
  public async Task<IEnumerable<ConfigResponse>> GetListView()
  {
    return await _configService.GetList( new BaseFilter() );
  }
  /// <summary>
  /// [Admin - List] lấy danh sách Config kèm thông tin theo type
  /// </summary>
  /// <returns></returns>
  [HttpGet( "GetOption/{type}" )]
  [Authorize( Policy = Policies.CONFIG_GET_LIST )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<ConfigResponse> ) )]
  public async Task<IEnumerable<ConfigResponse>> GetOption( string type )
  {
    return await _configService.GetOption( type );
  }
  /// <summary>
  /// [Admin - Create] Tạo mới Config
  /// </summary>
  /// <param name="resource"></param>
  /// <returns></returns>
  [HttpPost( "Create" )]
  [Authorize( Policy = Policies.CONFIG_CREATE )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  public async Task<IActionResult> Create( [FromBody] ConfigResource resource )
  {
    if ( !ModelState.IsValid )
      return BadRequest( ModelState );
    resource.Id = null;
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    // create
    var configId = await _configService.Create( resource, userLogin );
    return Ok( configId );
  }
  /// <summary>
  /// [Admin - Update] Cập nhật thông tin Config
  /// </summary>
  /// <param name="resource"></param>
  /// <returns></returns>
  [HttpPut( "Update" )]
  [Authorize( Policy = Policies.CONFIG_UPDATE )]
  [SwaggerResponse( StatusCodes.Status200OK )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  public async Task<IActionResult> Update( [FromBody] ConfigResource resource )
  {
    if ( !ModelState.IsValid )
      return BadRequest( ModelState );
    if ( resource.Id is null ) {
      return BadRequest( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Team ) );
    }
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    // update
    await _configService.Update( resource, userLogin );
    return Ok();
  }
  /// <summary>
  /// [Admin - Delete] Xóa Config
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpDelete( "Delete/{id}" )]
  [Authorize( Policy = Policies.CONFIG_DELETE )]
  [SwaggerResponse( StatusCodes.Status200OK )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  [SwaggerResponse( StatusCodes.Status404NotFound )]
  public async Task<IActionResult> Delete( long id )
  {
    // check Config id
    if ( !await _configService.Exist( t => t.Id == id ) ) {
      return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Config ) );
    }
    var errors = await ValidateBeforeDelete( id );
    if ( errors.Count > 0 ) {
      foreach ( var error in errors ) {
        ModelState.AddModelError( error.Key, error.Value );
      }
      return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
    }
    // delete
    await _configService.DeleteByCondition( t => t.Id == id );
    return Ok();
  }
  private async Task<Dictionary<string, string>> ValidateBeforeDelete( long id )
  {
    var errors = new Dictionary<string, string>();
    if ( await _configService.CheckHadUsingBeforeDelete( id ) ) {
      errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.HadUsing, DisplayNameResource.Category ) );
    }
    return errors;
  }
  [HttpPost( "total-workday" )]
  public async Task<IActionResult> CreateTotalWorkday( double day )
  {
    var month = DateTime.Now.Month;
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );

    var config = new ConfigResource
    {
      Name = "TotalWorkday",
      Code = day.ToString(),
      Description = month.ToString(),
      Type = "Day"
    };
    var old = await _configService.GetByCondition( x => x.Description == month.ToString() );
    if ( old != null ) {
      config.Id = old.Id;
      await _configService.Update( config, userLogin );
      return Ok( old.Id );
    }
    var configs = await _configService.Create( config, userLogin );
    return Ok( configs );
  }
  [HttpGet( "total-workday" )]
  public async Task<IActionResult> GetTotalWorkday()
  {
    var totalWorkday = await _configService.GetTotalWorking( "Day" );
    return Ok( totalWorkday );
  }
}
