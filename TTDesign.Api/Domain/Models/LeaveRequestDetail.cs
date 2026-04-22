
namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// chi tiết phân chia thời gian nghỉ trong các ngày của leave request
  /// </summary>
  public partial class LeaveRequestDetail : IComparer<LeaveRequestDetail>
  {
    public long Id { get; set; }
    /// <summary>
    /// leave request id quan hệ
    /// </summary>
    public long LeaveRequestId { get; set; }
    /// <summary>
    /// ngày đăng ký nghỉ
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// khoảng thời gian nghỉ (đơn vị giờ)
    /// </summary>
    public double Hours { get; set; }

    public virtual LeaveRequest LeaveRequest { get; set; } = null!;

    public int Compare( LeaveRequestDetail x, LeaveRequestDetail y )
    {
      // TODO: Handle x or y being null, or them not having names
      return x.Id.CompareTo( y.Id );
    }
  }
}
