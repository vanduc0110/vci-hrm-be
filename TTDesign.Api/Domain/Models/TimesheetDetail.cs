namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// chi tiết task làm việc trong 1 ngày timesheet: work log, overtime, leave và holiday
  /// </summary>
  public partial class TimesheetDetail : BaseEntity
  {
    /// <summary>
    /// 0 Project, 1 UnpaidLeave, 2 PaidLeave
    /// </summary>
    public int Type { get; set; }
    public long TimesheetId { get; set; }
    public long ProjectId { get; set; }
    /// <summary>
    /// reference to overtime_request, leave_request, holiday_id
    /// </summary>
    public long ReferenceId { get; set; }
    public long? TimesheetCategoryId { get; set; }
    public string? Description { get; set; }
    /// <summary>
    /// số giờ làm việc, user khai báo (đơn vị giờ)
    /// </summary>
    public double Hours { get; set; }
    public virtual Timesheet Timesheet { get; set; } = null!;
  }
}
