namespace TTDesign.API.Resources
{
  public class DashboardResource
  {
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng yyyy-MM-dd HH:mm</example>
    public DateTime DateCheck { get; set; }
    public long? TeamId { get; set; }
  }
  public class DashboardSummary
  {
    public int TotalEmployee { get; set; }
    public int TotalProject { get; set; }
    public int TotalActiveProject { get; set; }
    public int TotalCompletedProject { get; set; }
  }
  public class DashboardTimeTracking
  {
    public DashboardTimeTracking()
    {
      UserLates = new List<UserLate>();
      UserLeaves = new List<UserLeave>();
      UserLatesToday = new List<UserLate>();
      UserLeavesToday = new List<UserLeave>();
    }
    public int TotalEmps { get; set; }
    public double TotalWorkLogs { get; set; }
    public int TotalLateDay { get; set; }
    public int TotalLeaveDay { get; set; }
    public int TotalLateMonth { get; set; }
    public int TotalLeaveMonth { get; set; }
    public int TotalWorkingLog { get; set; }
    public List<UserLate> UserLates { get; set; } = null!;
    public List<UserLate> UserLatesToday { get; set; } = null!;
    public List<UserLeave> UserLeaves { get; set; } = null!;
    public List<UserLeave> UserLeavesToday { get; set; } = null!;

  }
  public class UserLate
  {
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int Number { get; set; }

  }
  public class UserLeave
  {
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public double Days { get; set; }

  }

  public class GetActiveProjectHoursResponse
  {
    public long Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public double TotalHour { get; set; }
    public ProjectObjectDetail [] Categories { get; set; } = null!;
  }
}
