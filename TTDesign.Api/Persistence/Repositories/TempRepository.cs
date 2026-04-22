using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class TempRepository : GenericRepository<User>, ITempRepository
  {
    public TempRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
