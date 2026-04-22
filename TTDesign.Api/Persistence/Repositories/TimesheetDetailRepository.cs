using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class TimesheetDetailRepository : GenericRepository<TimesheetDetail>, ITimesheetDetailRepository
  {
    public TimesheetDetailRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<IEnumerable<TimesheetDetail>> GetDataTimesheet( Expression<Func<TimesheetDetail, bool>> predicate )
    {
      return await _context.TimesheetDetails.Include( t => t.Timesheet ).Where( predicate ).ToListAsync();
    }
  }
}
