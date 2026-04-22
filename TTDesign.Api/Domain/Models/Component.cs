using System.ComponentModel.DataAnnotations;

namespace TTDesign.API.Domain.Models
{
  public class Component : BaseCreateEntity
  {
    [Required, MaxLength( 20 )]
    public string AssetCode { get; set; } = string.Empty;

    [Required, MaxLength( 100 )]
    public string Name { get; set; } = string.Empty;

    [MaxLength( 50 )]
    public string? Brand { get; set; }

    [MaxLength( 50 )]
    public string? SerialNumber { get; set; }
    public DateTime? PurchaseDate { get; set; }

    [MaxLength( 50 )]
    public string? Supplier { get; set; }

    /// <summary>
    /// Trạng thái sử dụng
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string Condition { get; set; } = string.Empty;

    [MaxLength( 100 )]
    public string Location { get; set; } = string.Empty;
    public string ComponentType { get; set; } = string.Empty;
    public long AssetId { get; set; }
    public long ComponentId { get; set; }
  }
}
