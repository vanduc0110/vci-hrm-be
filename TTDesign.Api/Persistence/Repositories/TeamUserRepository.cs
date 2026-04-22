using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class TeamUserRepository : GenericRepository<TeamUser>, ITeamUserRepository
  {
    public TeamUserRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
