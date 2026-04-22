namespace TTDesign.API.Resources
{
  public class TimesheetByMonthResource
  {
    public string FullName { get; set; }

    public string PaidLeave { get; set; }

    public List<string> Projects { get; set; }
  }

  public class TimesheetWorkingByMonthResource
  {
    public long ProjectId { get; set; }
    public long UserId { get; set; }
    public string FullName { get; set; }
    public string ProjecName { get; set; }
    public string WorkLog { get; set; }
  }
}
