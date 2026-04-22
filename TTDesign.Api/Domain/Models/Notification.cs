namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// Thông báo từ hệ thống
  /// </summary>
  public partial class Notification : BaseEntity
  {
    public Notification()
    {
      NotificationAssigns = new HashSet<NotificationAssign>();
    }

    /// <summary>
    /// tiêu đề
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// nội dung
    /// </summary>
    public string Content { get; set; } = null!;
    /// <summary>
    /// loại notification (trạng thái hiện tại của request): 0 Information, 1 Approved, 2 Rejected
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// đối tượng liên quan: 0 Notification, 1 LeaveRequest, 2 WfhRequest,3 NoticeInvite, 4 AssetInvite
    /// </summary>
    public int ObjectType { get; set; }
    /// <summary>
    /// ID đối tượng liên quan
    /// </summary>
    public long ObjectId { get; set; }
    /// <summary>
    /// tên user request object
    /// </summary>
    public string UserName { get; set; } = null!;

    public virtual ICollection<NotificationAssign> NotificationAssigns { get; set; }
  }
}
