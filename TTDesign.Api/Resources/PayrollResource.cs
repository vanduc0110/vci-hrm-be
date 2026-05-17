using System.ComponentModel.DataAnnotations;

namespace TTDesign.API.Resources
{
  // ── REQUEST ─────────────────────────────────────────
  public class SalaryResource
  {
    public long? Id { get; set; }
    [Required] public long UserId { get; set; }
    [Required] public decimal BasicSalary { get; set; }
    public decimal AllowanceHousing { get; set; }
    public decimal AllowanceTransport { get; set; }
    public decimal AllowanceFood { get; set; }
    public decimal AllowanceOther { get; set; }
    public DateTime EffectiveDate { get; set; } = DateTime.Today;
    public string? Notes { get; set; }
  }

  public class PayrollCalculateRequest
  {
    [Required] public int Month { get; set; }
    [Required] public int Year { get; set; }
    /// <summary>null = tính tất cả nhân viên</summary>
    public long? UserId { get; set; }
  }

  public class PayrollUpdateRequest
  {
    [Required] public long Id { get; set; }
    public decimal? Bonus { get; set; }
    public string? Notes { get; set; }
  }

  public class BonusResource
  {
    public long? Id { get; set; }
    [Required] public long UserId { get; set; }
    [Required] public int Month { get; set; }
    [Required] public int Year { get; set; }
    [Required] public decimal Amount { get; set; }
    [Required] public string Reason { get; set; } = null!;
  }

  public class TaxBracketResource
  {
    public long? Id { get; set; }
    [Required] public decimal FromAmount { get; set; }
    public decimal? ToAmount { get; set; }
    [Required] public decimal TaxRate { get; set; }
    [Required] public decimal QuickDeduction { get; set; }
    [Required] public int Year { get; set; }
  }

  public class SocialInsuranceRateResource
  {
    public long? Id { get; set; }
    public decimal SocialInsuranceEmployee { get; set; } = 8m;
    public decimal HealthInsuranceEmployee { get; set; } = 1.5m;
    public decimal UnemploymentInsuranceEmployee { get; set; } = 1m;
    public decimal PersonalDeduction { get; set; } = 11_000_000m;
    public decimal DependentDeduction { get; set; } = 4_400_000m;
    public DateTime EffectiveDate { get; set; } = DateTime.Today;
  }

  // ── RESPONSE ─────────────────────────────────────────
  public class SalaryResponse
  {
    public long Id { get; set; }
    public long UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string StaffId { get; set; } = null!;
    public decimal BasicSalary { get; set; }
    public decimal AllowanceHousing { get; set; }
    public decimal AllowanceTransport { get; set; }
    public decimal AllowanceFood { get; set; }
    public decimal AllowanceOther { get; set; }
    public decimal TotalAllowance => AllowanceHousing + AllowanceTransport + AllowanceFood + AllowanceOther;
    public DateTime EffectiveDate { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public bool IsApproved => ApprovedBy.HasValue;
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
  }

  public class PayrollResponse
  {
    public long Id { get; set; }
    public long UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string StaffId { get; set; } = null!;
    public string? BankName { get; set; }
    public string? AccountBank { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal TotalAllowance { get; set; }
    public decimal OvertimeSalary { get; set; }
    public decimal Bonus { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal SocialInsurance { get; set; }
    public decimal HealthInsurance { get; set; }
    public decimal UnemploymentInsurance { get; set; }
    public decimal IncomeTax { get; set; }
    public decimal TotalDeduction { get; set; }
    public decimal NetSalary { get; set; }
    public double StandardWorkDays { get; set; }
    public double ActualWorkHours { get; set; }
    public double OvertimeHours { get; set; }
    public string Status { get; set; } = null!;
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? Notes { get; set; }
    public List<PayrollDetailResponse> Details { get; set; } = new();
  }

  public class PayrollDetailResponse
  {
    public long Id { get; set; }
    public string Description { get; set; } = null!;
    public string DeductionType { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
  }

  public class BonusResponse
  {
    public long Id { get; set; }
    public long UserId { get; set; }
    public string FullName { get; set; } = null!;
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = null!;
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime CreatedDate { get; set; }
  }

  public class TaxBracketResponse
  {
    public long Id { get; set; }
    public decimal FromAmount { get; set; }
    public decimal? ToAmount { get; set; }
    public decimal TaxRate { get; set; }
    public decimal QuickDeduction { get; set; }
    public int Year { get; set; }
    public bool IsActive { get; set; }
  }

  public class SocialInsuranceRateResponse
  {
    public long Id { get; set; }
    public decimal SocialInsuranceEmployee { get; set; }
    public decimal HealthInsuranceEmployee { get; set; }
    public decimal UnemploymentInsuranceEmployee { get; set; }
    public decimal PersonalDeduction { get; set; }
    public decimal DependentDeduction { get; set; }
    public DateTime EffectiveDate { get; set; }
    public bool IsActive { get; set; }
  }
}
