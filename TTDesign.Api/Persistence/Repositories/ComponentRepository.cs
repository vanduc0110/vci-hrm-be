using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class ComponentRepository : GenericRepository<Component>, IComponentRepository
  {
    public ComponentRepository( AppDbContext context ) : base( context )
    {
    }
  }
}
