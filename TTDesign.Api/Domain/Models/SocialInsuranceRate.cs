namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// Tỉ lệ bảo hiểm bắt buộc (BHXH, BHYT, BHTN)
  /// </summary>
  public class SocialInsuranceRate : BaseEntity
  {
    /// <summary>% BHXH nhân viên đóng (mặc định 8%)</summary>
    public decimal SocialInsuranceEmployee { get; set; } = 8m;
    /// <summary>% BHYT nhân viên đóng (mặc định 1.5%)</summary>
    public decimal HealthInsuranceEmployee { get; set; } = 1.5m;
    /// <summary>% BHTN nhân viên đóng (mặc định 1%)</summary>
    public decimal UnemploymentInsuranceEmployee { get; set; } = 1m;
    /// <summary>Giảm trừ bản thân (mặc định 11 triệu)</summary>
    public decimal PersonalDeduction { get; set; } = 11_000_000m;
    /// <summary>Giảm trừ mỗi người phụ thuộc (mặc định 4.4 triệu)</summary>
    public decimal DependentDeduction { get; set; } = 4_400_000m;
    public DateTime EffectiveDate { get; set; }
    public bool IsActive { get; set; } = true;
  }
}
