using TTDesign.API.Constants;

namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// Chi tiết từng dòng cộng/trừ trong bảng lương
  /// </summary>
  public class PayrollDetail : BaseCreateEntity
  {
    public long PayrollId { get; set; }
    public string Description { get; set; } = null!;
    /// <summary>SocialInsurance / HealthInsurance / UnemploymentInsurance / IncomeTax / Other</summary>
    public int DeductionType { get; set; }
    /// <summary>Số tiền (dương = cộng, âm = trừ)</summary>
    public decimal Amount { get; set; }
    public string? Notes { get; set; }

    public virtual Payroll Payroll { get; set; } = null!;
  }
}
