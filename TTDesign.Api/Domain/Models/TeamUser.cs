namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// quan hệ team-user
  /// </summary>
  public partial class TeamUser
    {
        public long TeamId { get; set; }
        public long UserId { get; set; }

        public virtual Team Team { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
