namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// Cấu hình lương của từng nhân viên
  /// </summary>
  public class Salary : BaseEntity
  {
    public long UserId { get; set; }
    /// <summary>Lương cơ bản (đồng/tháng)</summary>
    public decimal BasicSalary { get; set; }
    /// <summary>Phụ cấp nhà ở</summary>
    public decimal AllowanceHousing { get; set; }
    /// <summary>Phụ cấp đi lại</summary>
    public decimal AllowanceTransport { get; set; }
    /// <summary>Phụ cấp ăn trưa</summary>
    public decimal AllowanceFood { get; set; }
    /// <summary>Phụ cấp khác</summary>
    public decimal AllowanceOther { get; set; }
    /// <summary>Ngày áp dụng</summary>
    public DateTime EffectiveDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
    /// <summary>null = chờ duyệt; có giá trị = đã được System/Director duyệt</summary>
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }

    public virtual User User { get; set; } = null!;
  }
}
