namespace TTDesign.API.Domain.Models
{
  public class Config : BaseEntity
  {
    /// <summary>
    /// name
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// code
    /// </summary>
    /// <example>text</example>
    public string Code { get; set; } = null!;
    /// <summary>
    /// decription
    /// </summary>
    public string? Description { get; set; } = null!;
    /// <summary>
    /// type
    /// </summary>
    public string? Type { get; set; } = "ProjectType";
    public bool? IsUsing { get; set; } = false;
  }
}
