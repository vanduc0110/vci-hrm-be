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
  public class AssetController : ControllerBase
  {
    private readonly IAssetService _assetService;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
    public AssetController( IAssetService assetService, IOptions<ApiBehaviorOptions> options )
    {
      _assetService = assetService;
      _apiBehaviorOptions = options;
    }
    [HttpGet( "GetListView" )]
    public async Task<IActionResult> GetAssets()
    {
      return Ok( await _assetService.GetList( new BaseFilter() ) );
    }
    [HttpGet( "computers" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<AssetResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetComputers()
    {
      return Ok( await _assetService.GetList( new BaseFilter()
      {
        AssetCategoryId = 5
      } ) );
    }
    [HttpGet( "screen" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<AssetResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetScreens()
    {
      return Ok( await _assetService.GetList( new BaseFilter()
      {
        AssetCategoryId = 6
      } ) );
    }
    [HttpGet( "allcomponents" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<AssetResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetAllCompoments()
    {
      return Ok( await _assetService.GetList( new BaseFilter()
      {
        AssetCategoryId = 7
      } ) );
    }
    [HttpGet( "components" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<AssetResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetComponents()
    {
      return Ok( await _assetService.GetList( new BaseFilter()
      {
        AssetCategoryId = 7,
        Status = ( int ) Enums.AssetStatus.InStorage
      } ) );
    }
    [HttpGet( "GetDetail/{id}" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( AssetDetailResponse ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetAsset( long id )
    {
      var asset = await _assetService.GetDetail( id );
      return Ok( asset );
    }
    [HttpPost( "{assetId}/components" )]
    public async Task<IActionResult> AddComponentToAsset( long assetId, [FromBody] ComponentToAsset dto )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _assetService.AddComponentToAsset( assetId, dto.ComponentIds, userLogin );
      return NoContent();
    }
    [HttpPost( "{assetId}/components/{componentId}" )]
    public async Task<IActionResult> RemoveComponentToAsset( long assetId, long componentId )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _assetService.RemoveComponentToAsset( assetId, componentId, userLogin );
      return NoContent();
    }
    [HttpPost( "create" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] AssetResource assetResource )
    {
      var errors = await ValidateBeforeCreate( assetResource );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return Ok( await _assetService.Create( assetResource, userLogin ) );
    }
    [HttpPut( "update" )]
    [SwaggerResponse( StatusCodes.Status204NoContent )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Update( [FromBody] AssetResource assetResource )
    {
      //var errors = await ValidateBeforeCreate( assetResource );
      //if ( errors.Count > 0 ) {
      //  foreach ( var error in errors ) {
      //    ModelState.AddModelError( error.Key, error.Value );
      //  }
      //  return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      //}
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _assetService.Update( assetResource, userLogin );
      return NoContent();
    }
    [HttpPut( "allocate" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Allocate( long id, [FromBody] AllocateAssetResource resoure )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _assetService.AllocateAsset( resoure, userLogin );
      return NoContent();
    }
    [HttpPut( "dispose" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Dispose( long id, [FromBody] DisposeAssetResource resoure )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _assetService.DisposeAsset( resoure, userLogin );
      return NoContent();
    }
    [HttpPut( "transfer" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Tranfer( long id, [FromBody] TransferAssetResource resoure )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _assetService.TranferAsset( resoure, userLogin );
      return NoContent();
    }
    [HttpPut( "return" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Return( long id, [FromBody] ReturnAssetResource resoure )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _assetService.ReturnAsset( resoure, userLogin );
      return NoContent();
    }
    [HttpPut( "destroy" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Destroy( long id, [FromBody] DestroyAssetResource resoure )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _assetService.DestroyAsset( resoure, userLogin );
      return NoContent();
    }
    [HttpGet( "assignment" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<AssetAllocationResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetAssignment()
    {
      return Ok( await _assetService.GetAssignment() );
    }
    [HttpGet( "asset-user" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<AssetAllocationResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetAssetUser()
    {
      return Ok( await _assetService.GetAssetsUser() );
    }
    [HttpGet( "asset-user/{id}" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<AssetAllocationResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetAssetUser( long id )
    {
      return Ok( await _assetService.GetAssetsUserDetail( id ) );
    }

    [HttpGet( "asset-storage" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<AssetStorageResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetAssetStorage()
    {
      return Ok( await _assetService.GetAssetStorage() );
    }
    [HttpGet( "asset-storage-option" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<AssetStorageResponse> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetAssetStorageOption()
    {
      return Ok( await _assetService.GetAssetStorageOption() );
    }
    private async Task<Dictionary<string, string>> ValidateBeforeCreate( AssetResource resource )
    {
      var errors = new Dictionary<string, string>();
      return errors;
    }
    [HttpDelete( "{id}" )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Delete( long id )
    {
      var errors = await ValidateBeforeDelete( id );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      // delete
      await _assetService.DeleteByCondition( t => t.Id == id );
      return Ok();
    }
    private async Task<Dictionary<string, string>> ValidateBeforeDelete( long id )
    {
      var errors = new Dictionary<string, string>();
      if ( !await _assetService.Exist( t => t.Id == id ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ) );
      }
      else if ( !await _assetService.CheckAssetBeforeDelete( id ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.HadUsing, DisplayNameResource.Asset ) );
      }
      return errors;
    }
    /// <summary>
    ///  //1: Computer, 2: Screen, 3: Accessory, 4: Electronics, 5: Furniture, 6: Other
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [HttpPost( "{type}/export" )]
    public async Task<IActionResult> Export( [FromRoute] int type )
    {
      var bytes = await _assetService.ExportAssets( type );
      var name = type switch
      {
        1 => "Computers",
        2 => "Screens",
        3 => "Accessories",
        4 => "Electronics",
        5 => "Furnitures",
        6 => "Others",
        _ => "Assets"
      };
      var fileName = $"{name}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
      if ( bytes == null || bytes.Length == 1 ) {
        return NoContent();
      }
      return File( bytes, "application/xlsx", fileName );
    }

    [HttpPost( "{type}/import" )]
    [Consumes( "multipart/form-data" )]
    public async Task<IActionResult> Import( [FromRoute] int type, IFormFile file )
    {
      var result = await _assetService.ImportAssets( file, type );
      return Ok( result );
    }

    [HttpGet( "Users/GetOption" )]
    [Authorize( Policy = Policies.ASSET_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<UserOption> ) )]
    public async Task<IEnumerable<UserOption>> GetOption()
    {
      return await _assetService.GetUserOptions();
    }
  }
}

