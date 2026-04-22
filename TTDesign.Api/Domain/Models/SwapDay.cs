namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// lưu trữ lịch hoán đổi 
  /// </summary>
  public partial class SwapDay : BaseCreateEntity
  {
    public SwapDay()
    {
      SwapDayUsers = new HashSet<SwapDayUser>();
    }

    /// <summary>
    /// ngày được hoán đổi
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// ngày hoán đổi
    /// </summary>
    public DateTime ToDate { get; set; }
    /// <summary>
    /// nội dung
    /// </summary>
    public string Description { get; set; } = null!;

    public virtual ICollection<SwapDayUser> SwapDayUsers { get; set; }
  }
}
