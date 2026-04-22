using static TTDesign.API.Constants.Enums;

namespace TTDesign.API.Domain.Models
{
  public class UserTask : BaseEntity
  {
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public StatusMark Status { get; set; } = StatusMark.None;
  }
}
