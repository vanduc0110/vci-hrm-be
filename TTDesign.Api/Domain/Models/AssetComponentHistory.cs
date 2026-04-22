namespace TTDesign.API.Domain.Models
{
  public class AssetComponentHistory : BaseEntity
  {
    public long AssetId { get; set; }
    public long ComponentId { get; set; }
    public DateTime EventDate { get; set; } = DateTime.UtcNow;
    public int Type { get; set; }
    public string Note { get; set; }
    public virtual Asset? Asset { get; set; }
  }
}
