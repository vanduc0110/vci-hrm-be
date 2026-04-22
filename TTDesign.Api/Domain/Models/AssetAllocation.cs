using System.ComponentModel.DataAnnotations;

namespace TTDesign.API.Domain.Models
{
  public class AssetAllocation : BaseEntity
  {
    public long AssetId { get; set; }
    public DateTime? EventDate { get; set; }
    public int? EventType { get; set; } = null!;
    public long? OldUserId { get; set; } = null!;
    public long? CurrentUserId { get; set; } = null!;
    [MaxLength( 1000 )]
    public string? StatusNotes { get; set; }
    public int Quantity { get; set; }
    public int? AssetStatus { get; set; }
    public virtual Asset Asset { get; set; } = null!;
  }
}
