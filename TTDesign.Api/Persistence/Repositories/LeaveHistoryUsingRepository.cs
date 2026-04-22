using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class LeaveHistoryUsingRepository : GenericRepository<LeaveHistoryUsing>, ILeaveHistoryUsingRepository
  {
    public LeaveHistoryUsingRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
