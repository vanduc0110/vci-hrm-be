using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Repositories
{
  public interface ISalaryRepository : IGenericRepository<Salary>
  {
    Task<IEnumerable<SalaryResponse>> GetActiveSalaries();
    Task<SalaryResponse?> GetActiveByUser( long userId );
  }
}
