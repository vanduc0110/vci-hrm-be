namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// timesheet của user theo ngày
  /// </summary>
  public partial class Timesheet : BaseEntity
  {
    public Timesheet()
    {
      TimesheetDetails = new HashSet<TimesheetDetail>();
      TimesheetReports = new HashSet<TimesheetReport>();
    }

    public long UserId { get; set; }
    public DateTime Date { get; set; }
    /// <summary>
    /// ai đã thực hiện lock timesheet này
    /// </summary>
    public long? LockBy { get; set; }
    /// <summary>
    /// tên ngày nghỉ lễ, nếu string empty thì là ngày thường
    /// </summary>
    public string? HolidayName { get; set; } = string.Empty;
    /// <summary>
    /// ngày được hoán đổi
    /// </summary>
    public DateTime? SwapDay { get; set; }
    /// <summary>
    /// wfh request ID tương ứng
    /// </summary>
    public long? WfhRequestId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual FingerPrinter FingerPrinter { get; set; } = null!;
    public virtual WfhRequest? WfhRequest { get; set; } = null!;
    public virtual ICollection<TimesheetDetail> TimesheetDetails { get; set; }
    public virtual ICollection<TimesheetReport> TimesheetReports { get; set; }
  }
}
