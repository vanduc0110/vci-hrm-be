using Microsoft.AspNetCore.Identity;

namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// thông tin user 
  /// </summary>
  public partial class User : IdentityUser<long>
  {
    public User()
    {
      Timesheets = new HashSet<Timesheet>();
      Projects = new HashSet<Project>();
    }

    public string FullName { get; set; } = null!;
    public DateTime? DateStartWork { get; set; }
    /// <summary>
    /// 0 Inactive, 1 Active
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// trạng thái do người dùng chọn: 0 Available, 1 WFH, 2 Business, 3 Busy, 4 Unavailable
    /// </summary>
    public int State { get; set; }
    public string StaffId { get; set; } = null!;
    /// <summary>
    /// chức danh/vị trí: 0 System, 1 Director, 2 TeamLead, 3 SubLeader, 4 PM, 5 Official, 6 Probationary, 7 Intership
    /// </summary>
    public int Position { get; set; }
    public string? Avatar { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public long? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<TeamUser> TeamUsers { get; set; }
    public virtual ICollection<Timesheet> Timesheets { get; set; }
    public virtual ICollection<Project> Projects { get; set; }
  }
}
