using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface INotificationService : IGenericService<Notification>,
    BaseServiceList<NotificationResponse>
  {
    /// <summary>
    /// tạo notification mới
    /// </summary>
    /// <param name="objectType">0 Notification, 1 LeaveRequest, 2 WfhRequest, 3 NoticeInvite, 4 AssetInvite</param>
    /// <param name="obj">đối tượng 
    /// - nếu objectType = 0, obj type {Title: text, Content: text, UserName: text, To: text, ObjectId: text, CreatedBy: text}
    /// - nếu objectType = 1, obj type LeaveRequest
    /// - nếu objectType = 2, obj type WfhRequest
    /// - nếu objectType = 3, obj type Calendar + CalendarInvited
    /// </param>
    Task Create( int objectType, object obj );
    /// <summary>
    /// đánh dấu đã đọc tất cả notification
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task MarkReadAll( long userId );
    /// <summary>
    /// xóa tất cả notification
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteAll( long userId );
    /// <summary>
    /// đánh dấu đã đọc notification
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task MarkRead( long id, long userId );
    /// <summary>
    /// xóa notification assign cho mình
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId">nếu ko truyền user id thì xóa hết assign</param>
    /// <returns></returns>
    Task Delete( long id, long? userId );
    /// <summary>
    /// khi leave/OT/WFH chuyển trạng thái pending->approve/reject, approve->reject
    /// </summary>
    /// <param name="objectType"></param>
    /// <param name="objectId"></param>
    /// <param name="type">NotificationType: 0 Information, 1 Approve, 2 Reject</param>
    /// <param name="creator">user create request</param>
    /// <returns></returns>
    Task ChangeUserAssign( int objectType, long objectId, int type, long creator );
  }
}
