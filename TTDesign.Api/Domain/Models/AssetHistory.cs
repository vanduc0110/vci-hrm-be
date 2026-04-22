
namespace TTDesign.API.Domain.Models
{
  public class AssetHistory : BaseEntity
  {
    public long AssetId { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime ActionDate { get; set; }
    public string? ToUserId { get; set; }
    public string? FromUserId { get; set; }
  }
}
