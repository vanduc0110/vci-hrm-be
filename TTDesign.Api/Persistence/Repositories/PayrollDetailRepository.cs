using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class PayrollDetailRepository : GenericRepository<PayrollDetail>, IPayrollDetailRepository
  {
    public PayrollDetailRepository( AppDbContext context ) : base( context ) { }
  }
}
