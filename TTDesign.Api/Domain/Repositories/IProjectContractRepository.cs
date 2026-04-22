using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface IProjectContractRepository : IGenericRepository<ProjectContract>
  {
    Task<List<ProjectContract>?> GetListDetail( long? id );
  }
}
