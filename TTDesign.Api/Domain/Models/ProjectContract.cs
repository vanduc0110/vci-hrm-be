namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// danh sách hợp đồng của dự án
  /// </summary>
  public partial class ProjectContract : BaseEntity
  {
    public ProjectContract()
    {
      ProjectDocuments = new HashSet<ProjectDocument>();
    }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public DateTime Date { get; set; }
    public long ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;
    public virtual ICollection<ProjectDocument> ProjectDocuments { get; set; }
  }
}
