using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class SwapDayUserRepository : GenericRepository<SwapDayUser>, ISwapDayUserRepository
  {
    public SwapDayUserRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
