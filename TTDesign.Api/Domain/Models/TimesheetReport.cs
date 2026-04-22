namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// tổng hợp timesheet làm việc tron ngày, nhóm theo project và type: project (gồm working, OT) hay leave hay holiday
  /// </summary>
  public partial class TimesheetReport : IComparer<TimesheetReport>
  {
    public long Id { get; set; }
    public long TimesheetId { get; set; }
    /// <summary>
    /// nếu là leave thì project = 0. holiday thì project = -1
    /// </summary>
    public long ProjectId { get; set; }
    /// <summary>
    /// thời gian log (đơn vị: giờ)
    /// </summary>
    public double Hours { get; set; }
    public virtual Timesheet Timesheet { get; set; } = null!;

    public int Compare( TimesheetReport x, TimesheetReport y )
    {
      // TODO: Handle x or y being null, or them not having names
      return x.Id.CompareTo( y.Id );
    }
  }
}
