using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class AssetRepository : GenericRepository<Asset>, IAssetRepository
  {
    public AssetRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<Asset?> GetDetailByConditionNoTrack( Expression<Func<Asset, bool>> predicate )
    {
      return await _context.Assets.Include( p => p.AssetCategory ).Include( x => x.AssetAllocations ).Where( predicate ).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Asset>> GetListAssetByCondition( Expression<Func<Asset, bool>> predicate )
    {
      return await _context.Assets.Include( p => p.AssetCategory ).Include( x => x.AssetAllocations ).Where( predicate ).AsNoTracking().ToListAsync();
    }
  }
}
