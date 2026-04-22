namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// lịch sử sử dụng ngày nghỉ
  /// </summary>
  public partial class LeaveHistory : BaseEntity
  {
    /// <summary>
    /// leave id tương ứng (có giá trị trong TH tạo leave/ thu hồi leave)
    /// </summary>
    public long? LeaveId { get; set; }
    /// <summary>
    /// request leave id tương ứng (có giá trị trong TH approve leave request)
    /// </summary>
    public long? LeaveRequestId { get; set; }
    /// <summary>
    /// 0 AddAnnualLeave,  1 TakeBackAnnualLeave,  2 UsingAnnualLeave
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// mô tả hành động: thu hồi leave, sử dụng leave...
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// tổng số ngày nghỉ phép còn lại (đơn vị ngày)
    /// </summary>
    public double AnnualLeave { get; set; }
    /// <summary>
    /// đơn vị thay đổi: thu hồi leave (-1), thêm ngày nghỉ phép (+1)
    /// </summary>
    public double? Unit { get; set; }
  }
}
