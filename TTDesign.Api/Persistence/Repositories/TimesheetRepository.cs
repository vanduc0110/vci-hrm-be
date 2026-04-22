using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class TimesheetRepository : GenericRepository<Timesheet>, ITimesheetRepository
  {
    public TimesheetRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<IEnumerable<Timesheet>> GetList( long user, long year, long month )
    {
      var start = new DateTime( ( int ) year, ( int ) month, 1 );
      var end = start.AddMonths( 1 );
      return await _context.Timesheets.Include( t => t.TimesheetReports ).Include( t => t.FingerPrinter ).ThenInclude( f => f.SwapDayRefer )
        .Where( t => t.UserId == user && t.Date >= start && t.Date < end ).OrderBy( t => t.Date ).AsNoTracking().ToListAsync();
    }

    public async Task<Timesheet?> GetTimesheet( Expression<Func<Timesheet, bool>> predicate )
    {
      return await _context.Timesheets.Include( t => t.FingerPrinter ).Where( predicate ).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Timesheet>> GetTimesheets( Expression<Func<Timesheet, bool>> predicate )
    {
      return await _context.Timesheets.Include( t => t.FingerPrinter ).Include( t => t.WfhRequest ).Where( predicate ).ToArrayAsync();
    }

    public async Task<Timesheet?> GetTimesheetDetailById( long id )
    {
      return await _context.Timesheets.Include( t => t.TimesheetDetails ).Include( t => t.FingerPrinter ).ThenInclude( f => f.SwapDayRefer ).Include( x => x.User ).ThenInclude( x => x.TeamUsers )
        .Where( t => t.Id == id ).FirstOrDefaultAsync();
    }

    public async Task<Timesheet?> GetTimesheetDetailByCondition( Expression<Func<Timesheet, bool>> predicate )
    {
      return await _context.Timesheets.Include( t => t.WfhRequest ).Include( t => t.TimesheetDetails ).Include( t => t.FingerPrinter ).ThenInclude( f => f.SwapDayRefer )
        .Where( predicate ).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Timesheet>> GetTimesheetDetails( Expression<Func<Timesheet, bool>> predicate )
    {
      return await _context.Timesheets.Include( t => t.TimesheetDetails ).Include( x => x.FingerPrinter ).Include( x => x.User ).ThenInclude( x => x.TeamUsers ).Where( predicate ).ToListAsync();
    }

    public async Task<Timesheet?> GetTimesheetDetail( Expression<Func<Timesheet, bool>> predicate )
    {
      return await _context.Timesheets.Include( t => t.TimesheetDetails ).Where( predicate ).FirstOrDefaultAsync();
    }

    public async Task<long []> GetTeamId( long id )
    {
      return await _context.Timesheets
          .Include( t => t.User )
          .ThenInclude( u => u.TeamUsers )
          .Where( t => t.Id == id )
          .SelectMany( t => t.User.TeamUsers.Select( x => x.TeamId ) )
          .ToArrayAsync();
    }
  }
}
