using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class FingerPrinterLogRepository : GenericRepository<FingerPrinterLog>, IFingerPrinterLogRepository
  {
    public FingerPrinterLogRepository( AppDbContext context ) : base( context )
    {
    }
  }
}
