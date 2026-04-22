namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// thông tin ngày hoán đổi apply vào timesheet
  /// </summary>
  public partial class SwapDayRefer : BaseSimpleEntity
  {
    /// <summary>
    /// giờ check in
    /// </summary>
    public DateTime DateIn { get; set; }
    /// <summary>
    /// giờ check out
    /// </summary>
    public DateTime DateOut { get; set; }
    /// <summary>
    /// tổng khoảng thời gian giữa checkin và checkout (đơn vị giờ, block 15')
    /// </summary>
    public double HourTotal { get; set; }
    /// <summary>
    /// timesheet áp dụng
    /// </summary>
    public long FingerPrinterId { get; set; }
    /// <summary>
    /// record SwapDay tương ứng
    /// </summary>
    public long SwapDayId { get; set; }

    public virtual FingerPrinter FingerPrinter { get; set; } = null!;
  }
}
