using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface ISalaryService : IGenericService<Salary>
  {
    Task<IEnumerable<SalaryResponse>> GetList();
    Task<SalaryResponse?> GetByUser( long userId );
    Task<long> Create( SalaryResource resource, long creator );
    Task Update( SalaryResource resource, long editor );
    Task Approve( long id, long approvedBy );
    Task Deactivate( long id, long editor );
  }
}
