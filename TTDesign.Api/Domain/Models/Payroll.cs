using TTDesign.API.Constants;

namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// Bảng lương tháng — 1 record = 1 nhân viên / 1 tháng
  /// </summary>
  public class Payroll : BaseEntity
  {
    public long UserId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    /// <summary>Lương cơ bản áp dụng tháng này</summary>
    public decimal BasicSalary { get; set; }
    /// <summary>Tổng phụ cấp</summary>
    public decimal TotalAllowance { get; set; }
    /// <summary>Lương OT</summary>
    public decimal OvertimeSalary { get; set; }
    /// <summary>Thưởng tháng</summary>
    public decimal Bonus { get; set; }
    /// <summary>Tổng lương từ = BasicSalary + Allowance + OT + Bonus</summary>
    public decimal GrossSalary { get; set; }
    /// <summary>BHXH nhân viên đóng</summary>
    public decimal SocialInsurance { get; set; }
    /// <summary>BHYT nhân viên đóng</summary>
    public decimal HealthInsurance { get; set; }
    /// <summary>BHTN nhân viên đóng</summary>
    public decimal UnemploymentInsurance { get; set; }
    /// <summary>Thuế TNCN</summary>
    public decimal IncomeTax { get; set; }
    /// <summary>Tổng khấu trừ</summary>
    public decimal TotalDeduction { get; set; }
    /// <summary>Lương thực lĩnh = GrossSalary - TotalDeduction</summary>
    public decimal NetSalary { get; set; }
    /// <summary>Số ngày làm việc chuẩn của tháng</summary>
    public double StandardWorkDays { get; set; }
    /// <summary>Số giờ thực tế từ máy chấm công</summary>
    public double ActualWorkHours { get; set; }
    /// <summary>Số giờ OT</summary>
    public double OvertimeHours { get; set; }
    /// <summary>Draft / Approved / Paid / Canceled</summary>
    public int Status { get; set; } = (int)Enums.PayrollStatus.Draft;
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? Notes { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual ICollection<PayrollDetail> Details { get; set; } = new List<PayrollDetail>();
  }
}
