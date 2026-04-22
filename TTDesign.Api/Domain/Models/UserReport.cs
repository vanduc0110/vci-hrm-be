namespace TTDesign.API.Domain.Models
{
  public class UserReport : BaseEntity
  {
    /// <summary>
    /// tiêu đề
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// Noi dung
    /// </summary>
    public string Content { get; set; } = null!;
    /// <summary>
    /// open, process, done, pending
    /// </summary>
    public int Status { get; set; }
    /// <summary>
    /// bug, new, update, refer
    /// </summary>
    public int Type { get; set; }
  }
}
