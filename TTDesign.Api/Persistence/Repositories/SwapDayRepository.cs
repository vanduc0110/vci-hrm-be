using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;
using TTDesign.API.Resources;

namespace TTDesign.API.Persistence.Repositories
{
  public class SwapDayRepository : GenericRepository<SwapDay>, ISwapDayRepository
  {
    public SwapDayRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<IEnumerable<SwapDayResponse>> GetListData( Expression<Func<SwapDay, bool>> predicate )
    {
      return await _context.SwapDays.Include( s => s.SwapDayUsers ).Where( predicate ).Select( s => new SwapDayResponse()
      {
        Id = s.Id,
        Description = s.Description,
        FromDate = s.FromDate,
        ToDate = s.ToDate,
        CreatedBy = s.CreatedBy,
        CreatedDate = s.CreatedDate,
        Users = s.SwapDayUsers == null || !s.SwapDayUsers.Any() ? "All" : string.Join( ", ", s.SwapDayUsers.Select( su => su.UserName ) ),
        UserIds = s.SwapDayUsers == null || !s.SwapDayUsers.Any() ? _context.Users.Where( x => x.IsActive).Select( x => x.Id ).ToArray() : s.SwapDayUsers.Select( su => su.Id ).ToArray()
      } ).ToListAsync();
    }

    public async Task<SwapDay?> GetData( Expression<Func<SwapDay, bool>> predicate )
    {
      return await _context.SwapDays.Include( s => s.SwapDayUsers ).Where( predicate ).FirstOrDefaultAsync();
    }
  }
}
