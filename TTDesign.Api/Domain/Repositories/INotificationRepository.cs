using System.Linq.Expressions;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface INotificationRepository : IGenericRepository<Notification>
  {
    /// <summary>
    /// lấy thông tin notification kèm danh sách assign
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<Notification?> getNotification( Expression<Func<Notification, bool>> predicate );
  }
}
