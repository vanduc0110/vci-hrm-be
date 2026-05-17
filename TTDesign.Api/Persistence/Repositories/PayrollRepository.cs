using Microsoft.EntityFrameworkCore;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class PayrollRepository : GenericRepository<Payroll>, IPayrollRepository
  {
    public PayrollRepository( AppDbContext context ) : base( context ) { }

    public async Task<Payroll?> GetWithDetails( long id )
    {
      return await _context.Payrolls
        .Include( p => p.User )
        .Include( p => p.Details )
        .FirstOrDefaultAsync( p => p.Id == id );
    }

    public async Task<IEnumerable<Payroll>> GetListByMonthYear( int month, int year )
    {
      return await _context.Payrolls
        .AsNoTracking()
        .Include( p => p.User )
        .Where( p => p.Month == month && p.Year == year )
        .OrderBy( p => p.User.FullName )
        .ToListAsync();
    }
  }
}
