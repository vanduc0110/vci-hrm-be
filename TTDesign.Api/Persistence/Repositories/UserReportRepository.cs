using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class UserReportRepository : GenericRepository<UserReport>, IUserReportRepository
  {
    public UserReportRepository( AppDbContext context ) : base( context )
    {
    }
  }

}
