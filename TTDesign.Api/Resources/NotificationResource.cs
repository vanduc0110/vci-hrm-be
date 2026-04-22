using TTDesign.API.Constants;

namespace TTDesign.API.Resources
{
  public class NotificationResource
  {
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class NotificationResponse
  {
    /// <summary>
    /// ID notification
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// tiêu đề
    /// </summary>
    /// <example>tiêu đề</example>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// nội dung chi tiết
    /// </summary>
    /// <example>nội dung</example>
    public string Content { get; set; } = string.Empty;
    /// <summary>
    /// loại thông báo: notification/ approved/ reject
    /// </summary>
    /// <example>Notification</example>
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// false: thông báo mới, true: đã đọc
    /// </summary>
    /// <example>false</example>
    public bool Status { get; set; }
    /// <summary>
    /// thời gian khởi tạo
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string CreatedDateText => CreatedDate.ToString( Enums.DATETIME_FORMAT );
    /// <summary>
    /// chỉ dẫn path điều hướng khi click Noti
    /// </summary>
    public NotificationPath Path { get; set; } = null!;
  }

  public class NotificationPath
  {
    /// <summary>
    /// ID đối tượng liên quan
    /// </summary>
    public long ObjectId { get; set; }
    /// <summary>
    /// path url view
    /// </summary>
    public string PathUrl { get; set; } = string.Empty;
    /// <summary>
    /// index tab in view
    /// </summary>
    public int TabIndex { get; set; }
  }
}
