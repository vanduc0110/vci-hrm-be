using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class ClientRepository : GenericRepository<Client>, IClientRepository
  {
    public ClientRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
