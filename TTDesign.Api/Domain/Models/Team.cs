namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// danh sách team
  /// </summary>
  public partial class Team : BaseEntity
  {
    public Team()
    {
      TeamUsers = new HashSet<TeamUser>();
      TeamCategories = new HashSet<TeamCategory>();
    }

    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    /// <summary>
    /// số user active trong team
    /// </summary>
    public int Amount { get; set; } = 0;

    public virtual ICollection<TeamUser> TeamUsers { get; set; }
    public virtual ICollection<TeamCategory> TeamCategories { get; set; }
  }
}
