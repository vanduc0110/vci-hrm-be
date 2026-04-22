namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// các request của hệ thống để chạy các crond service/ service ngầm
  /// </summary>
  public partial class SystemRequest : BaseCreateEntity
  {
    public long UserId { get; set; }
    /// <summary>
    /// 0 ActiveUser, 1 InactiveUser, 2 DefineTimesheetNextMonth, 3 DefineAnnualLeaveNextMonth, 4 TakeBackLeave
    /// </summary>
    public int Type { get; set; }
    public DateTime Date { get; set; }
    /// <summary>
    /// 0 pending, 1 fail
    /// </summary>
    public int Status { get; set; }
    /// <summary>
    /// id của đối tượng request
    /// </summary>
    public long ObjectId { get; set; }
  }
}
