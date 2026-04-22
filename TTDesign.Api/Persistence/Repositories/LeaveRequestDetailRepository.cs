using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class LeaveRequestDetailRepository : GenericRepository<LeaveRequestDetail>, ILeaveRequestDetailRepository
  {
    public LeaveRequestDetailRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
