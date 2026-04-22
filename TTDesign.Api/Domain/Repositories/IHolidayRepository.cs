using System.Linq.Expressions;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface IHolidayRepository : IGenericRepository<Holiday>
  {
    /// <summary>
    /// lấy danh sách holiday kèm thông tin apply
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<Holiday>> GetDataByCondition( Expression<Func<Holiday, bool>> predicate );
  }
}
