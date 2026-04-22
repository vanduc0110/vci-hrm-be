using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class RoleRepository : GenericRepository<Role>, IRoleRepository
  {
    public RoleRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
