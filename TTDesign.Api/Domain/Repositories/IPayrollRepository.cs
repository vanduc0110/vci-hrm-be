using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface IPayrollRepository : IGenericRepository<Payroll>
  {
    Task<Payroll?> GetWithDetails( long id );
    Task<IEnumerable<Payroll>> GetListByMonthYear( int month, int year );
  }
}
