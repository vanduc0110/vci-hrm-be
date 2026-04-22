using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class AssetCategoryRepository : GenericRepository<AssetCategory>, IAssetCategoryRepository
  {
    public AssetCategoryRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
