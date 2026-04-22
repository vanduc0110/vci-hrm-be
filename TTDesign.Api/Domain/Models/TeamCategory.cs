namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// category theo team
  /// </summary>
  public partial class TeamCategory : BaseCreateEntity
  {
    public long TeamId { get; set; }
    public string Name { get; set; } = null!;
    public bool IsUsing { get; set; }
    public string? Description { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
  }
}
