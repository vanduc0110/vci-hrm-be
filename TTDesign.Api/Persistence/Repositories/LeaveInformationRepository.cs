using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class LeaveInformationRepository : GenericRepository<LeaveInformation>, ILeaveInformationRepository
  {
    public LeaveInformationRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
