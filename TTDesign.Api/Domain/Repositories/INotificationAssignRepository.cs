using System.Linq.Expressions;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface INotificationAssignRepository : IGenericRepository<NotificationAssign>
  {
    /// <summary>
    /// lấy danh sách notification assign, bao gồm cả thông tin notification
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<NotificationAssign>> GetListNotification( Expression<Func<NotificationAssign, bool>> predicate );
  }
}
