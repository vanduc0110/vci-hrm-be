using System.Text.Json.Serialization;

namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// client khách request project
  /// </summary>
  public partial class Client : BaseEntity
  {
    public Client()
    {
      Projects = new HashSet<Project>();
    }

    public long Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string Name { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string Code { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    /// 
    [JsonIgnore]
    public virtual ICollection<Project> Projects { get; set; }
  }
}
