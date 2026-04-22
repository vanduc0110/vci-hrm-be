using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class SystemRequestRepository : GenericRepository<SystemRequest>, ISystemRequestRepository
  {
    public SystemRequestRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
