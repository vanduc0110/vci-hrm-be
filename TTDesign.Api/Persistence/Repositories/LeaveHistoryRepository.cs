using Microsoft.EntityFrameworkCore;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class LeaveHistoryRepository : GenericRepository<LeaveHistory>, ILeaveHistoryRepository
  {
    public LeaveHistoryRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<LeaveHistory?> GetLast( long userId )
    {
      return await _context.LeaveHistorys.Where( l => l.CreatedBy == userId ).OrderBy( l => l.Id ).LastOrDefaultAsync();
    }
  }
}
