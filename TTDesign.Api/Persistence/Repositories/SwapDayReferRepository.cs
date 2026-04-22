using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class SwapDayReferRepository : GenericRepository<SwapDayRefer>, ISwapDayReferRepository
  {
    public SwapDayReferRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
