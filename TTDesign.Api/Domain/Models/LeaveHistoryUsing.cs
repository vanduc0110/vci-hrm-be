namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// quan hệ Leave và Leave request
  /// </summary>
  public partial class LeaveHistoryUsing
  {
    public long Id { get; set; }
    public long LeaveId { get; set; }
    public long LeaveRequestId { get; set; }
    public double Hours { get; set; }

    //public virtual Leave Leave { get; set; } = null!;
    public virtual LeaveRequest LeaveRequest { get; set; } = null!;
  }
}
