using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IHolidayService : IGenericService<Holiday>,
    BaseServiceResource<HolidayResource>, BaseServiceList<HolidayResponse>
  {
    /// <summary>
    /// danh sách holiday information
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<IEnumerable<HolidayResponse>> GetListInfor( BaseFilter filter );
  }
}
