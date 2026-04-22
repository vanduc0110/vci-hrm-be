using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IFingerPrinterService : IGenericService<FingerPrinter>,
    BaseServiceResource<FingerPrinterResource>, BaseServiceList<FingerPrinterResponse>
  {
    /// <summary>
    /// valid kiểm tra timesheet đã bị lock hay chưa
    /// </summary>
    /// <remarks>truyền input 1 trong 2 giá trị</remarks>
    /// <param name="date"></param>
    /// <param name="id"></param>
    /// <returns>true: ts đã lock, không được thực hiện tiếp</returns>
    Task<bool> CheckTimesheetHadLock( long id, DateTime? date = null );
    /// <summary>
    /// reset thông tin chấm công
    /// </summary>
    /// <param name="id"></param>
    /// <param name="editor"></param>
    /// <returns></returns>
    Task Delete( long id, long editor );

    Task UpdateFingureExcel( IFormFile file, long editor );
  }
}
