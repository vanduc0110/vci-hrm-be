using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class AssetAllocateRepository : GenericRepository<AssetAllocation>, IAssetAllocateRepository
  {
    public AssetAllocateRepository( AppDbContext context ) : base( context )
    {
    }

    public async Task<IEnumerable<AssetAllocation>> GetDataListByCondition( Expression<Func<AssetAllocation, bool>> predicate )
    {
      return await _context.Set<AssetAllocation>().Include( x => x.Asset ).ThenInclude( x => x.AssetCategory ).AsNoTracking().Where( predicate ).ToListAsync();
    }
  }
}
