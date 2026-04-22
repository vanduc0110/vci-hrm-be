using System.ComponentModel.DataAnnotations;

namespace TTDesign.API.Domain.Models
{
  public class AssetCategory : BaseEntity
  {
    [Required, MaxLength( 100 )]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength( 10 )]
    public string Code { get; set; } = string.Empty;

    [MaxLength( 200 )]
    public string? Description { get; set; }
    public int Level { get; set; } = 1;
    public long? ParentId { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual AssetCategory? Parent { get; set; }
    public virtual ICollection<AssetCategory> Children { get; set; } = new List<AssetCategory>();
  }
}
