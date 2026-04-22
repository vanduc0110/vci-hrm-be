using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class GoogleTimesheetKeyRepository : GenericRepository<GoogleTimesheetKey>, IGoogleTimesheetKeyRepository
  {
    public GoogleTimesheetKeyRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
