using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Repositories
{
  public interface IFingerPrinterRepository : IGenericRepository<FingerPrinter>
  {
    Task<IEnumerable<FingerPrinterResponse>> GetFingerPrinterByFilter( BaseFilter filter );
  }
}
