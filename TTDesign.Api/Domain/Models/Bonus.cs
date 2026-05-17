namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// Thưởng / phạt thủ công từng tháng
  /// </summary>
  public class Bonus : BaseEntity
  {
    public long UserId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    /// <summary>Số tiền (dương = thưởng, âm = phạt)</summary>
    public decimal Amount { get; set; }
    public string Reason { get; set; } = null!;
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }

    public virtual User User { get; set; } = null!;
  }
}
