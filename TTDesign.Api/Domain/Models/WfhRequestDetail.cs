namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// request WFH quan hệ ngày wfh với timesheet
  /// </summary>
  public partial class WfhRequestDetail : BaseSimpleEntity
  {
    /// <summary>
    /// timesheet id được apply tương ứng
    /// </summary>
    public long? TimesheetId { get; set; }
    /// <summary>
    /// wfh request tương ứng
    /// </summary>
    public long WfhRequestId { get; set; }
    public virtual Timesheet? Timesheet { get; set; } = null!;
  }
}
