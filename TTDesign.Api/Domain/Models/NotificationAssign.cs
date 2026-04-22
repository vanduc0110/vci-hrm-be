namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// Thông báo từ hệ thống
  /// </summary>
  public partial class NotificationAssign : BaseSimpleEntity
  {
    public long NotificationId { get; set; }
    /// <summary>
    /// 0 unread, 1 read
    /// </summary>
    public bool Status { get; set; }
    /// <summary>
    /// user ID
    /// </summary>
    public long UserId { get; set; }

    public virtual Notification Notification { get; set; } = null!;
  }
}
