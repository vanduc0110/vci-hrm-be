using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
  {
    protected readonly AppDbContext _context;

    public GenericRepository( AppDbContext context )
    {
      _context = context;
    }

    public async Task CreateAsync( TEntity obj )
    {
      await _context.Set<TEntity>().AddAsync( obj );
      _context.SaveChanges();
    }

    public async Task CreatesAsync( IEnumerable<TEntity> obj )
    {
      await _context.Set<TEntity>().AddRangeAsync( obj );
      _context.SaveChanges();
    }

    public void Update( TEntity obj )
    {
      _context.Set<TEntity>().Update( obj );
      _context.SaveChanges();
    }

    public void Updates( TEntity [] obj )
    {
      _context.Set<TEntity>().UpdateRange( obj );
      _context.SaveChanges();
    }

    public async Task DeleteByCondition( Expression<Func<TEntity, bool>> predicate )
    {
      var entity = await _context.Set<TEntity>().Where( predicate ).ToListAsync();
      if ( entity != null && entity.Count() > 0 ) {
        _context.Set<TEntity>().RemoveRange( entity );
        _context.SaveChanges();
      }
    }

    public void Delete( TEntity obj )
    {
      _context.Set<TEntity>().Remove( obj );
      _context.SaveChanges();
    }

    public void Deletes( IEnumerable<TEntity> obj )
    {
      _context.Set<TEntity>().RemoveRange( obj );
      _context.SaveChanges();
    }

    public async Task<TEntity?> GetByConditionNoTrack( Expression<Func<TEntity, bool>> predicate )
    {
      return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync( predicate );
    }

    public async Task<TEntity?> GetByCondition( Expression<Func<TEntity, bool>> predicate )
    {
      return await _context.Set<TEntity>().FirstOrDefaultAsync( predicate );
    }

    public async Task<IEnumerable<TEntity>> GetListByCondition( Expression<Func<TEntity, bool>> predicate )
    {
      return await _context.Set<TEntity>().AsNoTracking().Where( predicate ).ToListAsync();
    }
    
    public async Task<IEnumerable<TEntity>> GetListByConditionTrack( Expression<Func<TEntity, bool>> predicate )
    {
      return await _context.Set<TEntity>().Where( predicate ).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
      return await _context.Set<TEntity>().AsNoTracking().ToArrayAsync();
    }

    public async Task<bool> Exist( Expression<Func<TEntity, bool>> predicate )
    {
      return await _context.Set<TEntity>().AnyAsync( predicate );
    }
  }
}
