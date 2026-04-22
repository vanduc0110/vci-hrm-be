namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// request WFH
  /// </summary>
  public partial class WfhRequest : BaseEntity
  {
    public WfhRequest()
    {
      Timesheets = new HashSet<Timesheet>();
    }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    /// <summary>
    /// lý do
    /// </summary>
    public string Reason { get; set; } = null!;
    /// <summary>
    /// trạng thái: 0 Pending, 1 Approve, 2 Reject
    /// </summary>
    public int Status { get; set; }
    /// <summary>
    /// người review request
    /// </summary>
    public long? Reviewer { get; set; }

    public virtual ICollection<Timesheet> Timesheets { get; set; }
  }
}
