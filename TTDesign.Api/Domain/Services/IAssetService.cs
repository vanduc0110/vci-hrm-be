using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IAssetService : IGenericService<Asset>, BaseServiceResource<AssetResource>, BaseServiceList<AssetResponse>, BaseServiceDetail<AssetDetailResponse>
  {
    Task AddComponentToAsset( long assetId, List<AssetToComponent> objs, long creator );
    Task RemoveComponentToAsset( long assetId, long componentId, long editor );
    Task AllocateAsset( AllocateAssetResource obj, long userId );
    Task DisposeAsset( DisposeAssetResource obj, long userId );
    Task TranferAsset( TransferAssetResource obj, long userId );
    Task ReturnAsset( ReturnAssetResource obj, long userId );
    Task DestroyAsset( DestroyAssetResource obj, long userId );
    Task<IEnumerable<AssetAllocationResponse>> GetAssignment();
    Task<IEnumerable<AssetUserResponse>> GetAssetsUser();
    Task<AssetByUserResponse> GetAssetsUserDetail( long userId );
    Task<IEnumerable<AssetStorageResponse>> GetAssetStorage();
    Task<bool> CheckAssetBeforeDelete( long id );
    Task<IEnumerable<AssetStorageOption>> GetAssetStorageOption();
    Task<byte []> ExportAssets( int type );
    Task<AssetImportReponse> ImportAssets( IFormFile file, int type );
    Task<IEnumerable<UserOption>> GetUserOptions();
  }
}
