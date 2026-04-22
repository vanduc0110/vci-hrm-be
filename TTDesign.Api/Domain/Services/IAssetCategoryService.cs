using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IAssetCategoryService : IGenericService<AssetCategory>, BaseServiceResource<AssetCategoryResource>, BaseServiceList<AssetCategoryResponse>
  {
    Task<bool> CheckHadUsingBeforeDelete( long id );
  }
}
