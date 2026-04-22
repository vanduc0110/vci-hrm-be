using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TTDesign.API.Domain.Models
{
  public class Asset : BaseEntity
  {
    [Required, MaxLength( 20 )]
    public string AssetCode { get; set; } = string.Empty;

    [Required, MaxLength( 100 )]
    public string Name { get; set; } = string.Empty;

    [MaxLength( 50 )]
    public string? Brand { get; set; }

    [MaxLength( 50 )]
    public string? Model { get; set; }

    [MaxLength( 50 )]
    public string? SerialNumber { get; set; }
    public bool ? PurchasePrice { get; set; }
    public DateTime? PurchaseDate { get; set; }

    [MaxLength( 50 )]
    public string? Supplier { get; set; } 

    /// <summary>
    /// Trạng thái sử dụng
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Condition { get; set; }

    [MaxLength( 500 )]
    public string? Notes { get; set; }

    public long AssetCategoryId { get; set; }

    [MaxLength( 100 )]
    public string? Location { get; set; } = string.Empty;
    [DefaultValue( 1 )]
    public int Quantity { get; set; } = 1;
    [MaxLength( 2000 )]
    public string? DetailedSpecs { get; set; }
    public virtual AssetCategory AssetCategory { get; set; } = null!;
    public virtual ICollection<AssetAllocation> AssetAllocations { get; set; } = new List<AssetAllocation>();
    public virtual ICollection<AssetComponentHistory> AssetComponents { get; set; } = new List<AssetComponentHistory>();

  }
}
