using Microsoft.EntityFrameworkCore;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;
using TTDesign.API.Resources;

namespace TTDesign.API.Persistence.Repositories
{
  public class BonusRepository : GenericRepository<Bonus>, IBonusRepository
  {
    public BonusRepository( AppDbContext context ) : base( context ) { }

    public async Task<IEnumerable<BonusResponse>> GetListWithUser( int month, int year )
    {
      return await _context.Bonuses
        .AsNoTracking()
        .Include( b => b.User )
        .Where( b => b.Month == month && b.Year == year )
        .OrderBy( b => b.User.FullName )
        .Select( b => new BonusResponse
        {
          Id = b.Id,
          UserId = b.UserId,
          FullName = b.User.FullName,
          Month = b.Month,
          Year = b.Year,
          Amount = b.Amount,
          Reason = b.Reason,
          ApprovedBy = b.ApprovedBy,
          ApprovedDate = b.ApprovedDate,
          CreatedDate = b.CreatedDate,
        } )
        .ToListAsync();
    }
  }
}
