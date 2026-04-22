namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// danh sách ngày nghỉ của member
  /// </summary>
  public partial class Leave : BaseEntity
  {
    public Leave()
    {
      LeaveHistoryUsings = new HashSet<LeaveHistoryUsing>();
    }
    public long UserId { get; set; }
    /// <summary>
    /// loại: nghỉ phép/ nghỉ bù
    /// 6 Annual
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// ngày phát sinh ngày nghỉ: mặc định ngày 1, dùng tháng + năm để xác định thời điểm thu hồi ngày nghỉ hết hạn
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// tổng thời gian (đơn vị giờ)
    /// </summary>
    public double Hours { get; set; }
    /// <summary>
    /// đã sử dụng bao nhiêu (đơn vị giờ)
    /// </summary>
    public double Using { get; set; }
    public virtual ICollection<LeaveHistoryUsing> LeaveHistoryUsings { get; set; }
  }
}
