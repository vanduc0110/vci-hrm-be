using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class ConfigRepository : GenericRepository<Config>, IConfigRepository
  {
    public ConfigRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
