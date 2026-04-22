using System.Linq.Expressions;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface IAssetAllocateRepository : IGenericRepository<AssetAllocation>
  {
    Task<IEnumerable<AssetAllocation>> GetDataListByCondition( Expression<Func<AssetAllocation, bool>> predicate );
  }
}
