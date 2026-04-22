using System.Linq.Expressions;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface IAssetRepository : IGenericRepository<Asset>
  {
    Task<Asset?> GetDetailByConditionNoTrack( Expression<Func<Asset, bool>> predicate );
    Task<IEnumerable<Asset>> GetListAssetByCondition( Expression<Func<Asset, bool>> predicate );

  }
}
