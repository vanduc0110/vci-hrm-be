using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TTDesign.API.Domain.Services;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers;
/// <summary>
/// APIs của Client Controller
/// </summary>
[Route( "api/[controller]" )]
[ApiController]
[Authorize]
[Consumes( "application/json" )]
[Produces( "application/json" )]
public class ClientController : ControllerBase
{
  private readonly IClientService _clientService;
  public ClientController( IClientService clientService )
  {
    _clientService = clientService;
  }
  /// <summary>
  /// [Admin - List] lấy danh sách client kèm thông tin
  /// </summary>
  /// <returns></returns>
  [HttpGet( "GetListView" )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<ClientResponse> ) )]
  public async Task<IEnumerable<ClientResponse>> GetListView()
  {
    return await _clientService.GetList( new BaseFilter() );
  }
  /// <summary>
  /// [Admin - Create] Tạo mới Client
  /// </summary>
  /// <param name="resource"></param>
  /// <returns></returns>
  [HttpPost( "Create" )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  public async Task<IActionResult> Create( [FromBody] ClientResource resource )
  {
    if ( !ModelState.IsValid )
      return BadRequest( ModelState );
    resource.Id = null;
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    // create
    var teamId = await _clientService.Create( resource, userLogin );
    return Ok( teamId );
  }
  /// <summary>
  /// [Admin - Update] Cập nhật thông tin Client
  /// </summary>
  /// <param name="resource"></param>
  /// <returns></returns>
  [HttpPut( "Update" )]
  [SwaggerResponse( StatusCodes.Status200OK )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  public async Task<IActionResult> Update( [FromBody] ClientResource resource )
  {
    if ( !ModelState.IsValid )
      return BadRequest( ModelState );
    if ( resource.Id is null ) {
      return BadRequest( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Team ) );
    }
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    // update
    await _clientService.Update( resource, userLogin );
    return Ok();
  }
  /// <summary>
  /// [Admin - Delete] Xóa Client
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpDelete( "Delete/{id}" )]
  [SwaggerResponse( StatusCodes.Status200OK )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  [SwaggerResponse( StatusCodes.Status404NotFound )]
  public async Task<IActionResult> Delete( long id )
  {
    // check client id
    if ( !await _clientService.Exist( t => t.Id == id ) ) {
      return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Client ) );
    }
    // check client in project
    if ( !await _clientService.CheckClientBeforeDelete( id ) ) {
      return BadRequest( string.Format( ErrorMessageResource.TeamHadUserInclude, DisplayNameResource.Client ) );
    }
    // delete
    await _clientService.DeleteByCondition( t => t.Id == id );
    return Ok();
  }
}
