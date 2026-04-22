namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// request nghỉ
  /// </summary>
  public partial class LeaveRequest : BaseEntity
  {
    public LeaveRequest()
    {
      LeaveHistoryUsings = new HashSet<LeaveHistoryUsing>();
      LeaveRequestDetails = new HashSet<LeaveRequestDetail>();
    }
    /// <summary>
    /// 0 SelfWedding, 1 FamilyWedding, 2 FamilyBereavement, 3 RelativeBereavement, 4 SelfMaternity, 5 FamilyMaternity, 6 Annual, 7 Unpaid
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// thời điểm bắt đầu
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// thời điểm kết thúc
    /// </summary>
    public DateTime EndDate { get; set; }
    /// <summary>
    /// lý do xin nghỉ
    /// </summary>
    public string? Reason { get; set; }
    /// <summary>
    /// khoảng thời gian nghỉ (đơn vị giờ)
    /// </summary>
    public double Hour { get; set; }
    /// <summary>
    /// trạng thái: 0 Pending, 1 Approve, 2 Rejecte
    /// </summary>
    public int Status { get; set; }
    /// <summary>
    /// người review request (người thực hiện reject/approve
    /// </summary>
    public long? Reviewer { get; set; }
    public virtual ICollection<LeaveHistoryUsing> LeaveHistoryUsings { get; set; }
    public virtual ICollection<LeaveRequestDetail> LeaveRequestDetails { get; set; }
  }
}
