using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Services;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class AssetCategoryController : ControllerBase
  {
    private readonly IAssetCategoryService _assetCategoryService;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
    public AssetCategoryController( IAssetCategoryService assetCategoryService, IOptions<ApiBehaviorOptions> apiBehaviorOptions )
    {
      _apiBehaviorOptions = apiBehaviorOptions;
      _assetCategoryService = assetCategoryService;
    }
    [HttpGet( "GetListView" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<AssetCategoryResponse> ) )]
    public async Task<IEnumerable<AssetCategoryResponse>> GetListView()
    {
      return await _assetCategoryService.GetList( new BaseFilter() );
    }
    [HttpPost( "Create" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] AssetCategoryResource resource )
    {
      if ( !ModelState.IsValid )
        return BadRequest( ModelState );
      resource.Id = null;
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // create
      var productTypeId = await _assetCategoryService.Create( resource, userLogin );
      return Ok( productTypeId );
    }
    [HttpPut( "Update" )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Update( [FromBody] AssetCategoryResource resource )
    {
      if ( !ModelState.IsValid )
        return BadRequest( ModelState );
      if ( resource.Id is null ) {
        return BadRequest( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Team ) );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // update
      await _assetCategoryService.Update( resource, userLogin );
      return Ok();
    }

    [HttpDelete( "Delete/{id}" )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    [SwaggerResponse( StatusCodes.Status404NotFound )]
    public async Task<IActionResult> Delete( long id )
    {

      if ( !await _assetCategoryService.Exist( t => t.Id == id ) ) {
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
      await _assetCategoryService.DeleteByCondition( t => t.Id == id );
      return Ok();
    }
    private async Task<Dictionary<string, string>> ValidateBeforeDelete( long id )
    {
      var errors = new Dictionary<string, string>();
      if ( await _assetCategoryService.CheckHadUsingBeforeDelete( id ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.HadUsing, DisplayNameResource.AssetCategory ) );
      }
      return errors;
    }
  }
}
