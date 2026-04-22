namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// lưu trữ lịch hoán đổi 
  /// </summary>
  public partial class SwapDayUser : BaseSimpleEntity
  {
    /// <summary>
    /// record hoán đổi ngày
    /// </summary>
    public long SwapDayId { get; set; }
    /// <summary>
    /// user áp dụng
    /// </summary>
    public long UserId { get; set; }
    public string UserName { get; set; } = null!;

    public virtual SwapDay SwapDay { get; set; } = null!;
  }
}
