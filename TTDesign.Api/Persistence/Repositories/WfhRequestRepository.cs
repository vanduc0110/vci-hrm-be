using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class WfhRequestRepository : GenericRepository<WfhRequest>, IWfhRequestRepository
  {
    public WfhRequestRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
