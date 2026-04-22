using System.Linq.Expressions;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface ITimesheetDetailRepository : IGenericRepository<TimesheetDetail>
  {
    /// <summary>
    /// danh sách timesheet detail có kèm thông tin timesheet
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<TimesheetDetail>> GetDataTimesheet( Expression<Func<TimesheetDetail, bool>> predicate );
  }
}
