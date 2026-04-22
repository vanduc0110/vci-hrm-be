using System.Linq.Expressions;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;

namespace TTDesign.API.Services
{
  public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
  {
    private readonly IGenericRepository<TEntity> _genericRepository;

    public GenericService( IGenericRepository<TEntity> genericRepository )
    {
      _genericRepository = genericRepository;
    }

    public Task DeleteByCondition( Expression<Func<TEntity, bool>> predicate )
    {
      return _genericRepository.DeleteByCondition( predicate );
    }

    public Task<TEntity?> GetByCondition( Expression<Func<TEntity, bool>> predicate )
    {
      return _genericRepository.GetByConditionNoTrack( predicate );
    }

    public Task<IEnumerable<TEntity>> GetAll()
    {
      return _genericRepository.GetAll();
    }

    public async Task<bool> Exist( Expression<Func<TEntity, bool>> predicate )
    {
      return await _genericRepository.Exist( predicate );
    }
  }
}
