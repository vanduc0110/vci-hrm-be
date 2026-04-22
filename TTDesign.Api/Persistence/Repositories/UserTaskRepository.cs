using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class UserTaskRepository : GenericRepository<UserTask>, IUserTaskRepository
  {
    public UserTaskRepository( AppDbContext context ) : base( context )
    {

    }

  }
}
