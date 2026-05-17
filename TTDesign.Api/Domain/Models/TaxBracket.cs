namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// Bảng lũy tiến thuế TNCN (7 bậc theo luật VN)
  /// </summary>
  public class TaxBracket : BaseEntity
  {
    /// <summary>Thu nhập tính thuế từ (triệu đồng)</summary>
    public decimal FromAmount { get; set; }
    /// <summary>Thu nhập tính thuế đến (null = không giới hạn)</summary>
    public decimal? ToAmount { get; set; }
    /// <summary>Thuế suất (%)</summary>
    public decimal TaxRate { get; set; }
    /// <summary>Số tiền giảm trừ nhanh</summary>
    public decimal QuickDeduction { get; set; }
    public int Year { get; set; }
    public bool IsActive { get; set; } = true;
  }
}
