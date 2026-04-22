using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class AssetComponentRepository : GenericRepository<AssetComponentHistory>, IAssetComponentRepository
  {
    public AssetComponentRepository( AppDbContext context ) : base( context )
    {
    }
  }
}
