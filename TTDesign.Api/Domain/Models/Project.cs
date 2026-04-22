namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// danh sách dự án
  /// </summary>
  public partial class Project : BaseEntity
  {
    public Project()
    {
      Users = new HashSet<User>();
      ProjectContracts = new HashSet<ProjectContract>();
    }

    /// <summary>
    /// team quản lý dự án
    /// </summary>
    public long TeamId { get; set; }
    /// <summary>
    /// PM (user id)
    /// </summary>
    public string ProjectManagement { get; set; }
    public long? ClientId { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    /// <summary>
    /// 0 Pending, 1 Active, 2 End
    /// </summary>
    public bool Status { get; set; }
    /// <summary>
    /// dự án đóng/mở?
    /// </summary>
    public bool? IsPublic { get; set; }
    /// <summary>
    /// số thứ tự dự án
    /// </summary>
    public int ProjectNumber { get; set; }
    /// <summary>
    /// số giờ theo kế hoạch
    /// </summary>
    public int QuotationHour { get; set; }
    /// <summary>
    /// T, I, H
    /// </summary>
    public string Type { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    /// <summary>
    /// năm tài chính
    /// </summary>
    public int FiscalYear { get; set; }
    /// <summary>
    /// tổng thời gian làm việc thực tế (đơn vị giờ)
    /// </summary>
    public double WorkingHour { get; set; }
    public virtual Client? Client { get; set; }
    public virtual ICollection<User> Users { get; set; }
    public virtual ICollection<ProjectContract> ProjectContracts { get; set; }
  }
}
