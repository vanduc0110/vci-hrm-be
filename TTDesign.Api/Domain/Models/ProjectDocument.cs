namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// danh sách document của dự án
  /// </summary>
  public partial class ProjectDocument : BaseCreateEntity
  {
    /// <summary>
    /// tên 
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// ghi chú
    /// </summary>
    public string? Comment { get; set; }
    /// <summary>
    /// path link local
    /// </summary>
    public string Link { get; set; } = null!;
    public long ContractId { get; set; }
    public virtual ProjectContract ProjectContract { get; set; } = null!;
  }
}
