using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class HolidayRepository : GenericRepository<Holiday>, IHolidayRepository
  {
    public HolidayRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<IEnumerable<Holiday>> GetDataByCondition( Expression<Func<Holiday, bool>> predicate )
    {
      return await _context.Holidays.Include( h => h.HolidayApplys ).Where( predicate ).ToListAsync();
    }
  }
}
