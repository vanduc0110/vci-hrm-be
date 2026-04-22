using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Services
{
  public interface IOtherService
  {
    Task<IEnumerable<Client>> GetClients();
    Task<int> GetNewProjectNumber();
  }
}
