namespace TTDesign.API.Resources
{
  public class UserReportResource
  {
    public long? Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Type { get; set; } = null!;
  }

  public class UserReportResponse
  {
    public long Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Type { get; set; } = null!;
    public long ModifiedBy { get; set; }
    public string ModifiedName { get; set; } = null!;
    public DateTime ModifiedDate { get; set; }
    public long CreatedBy { get; set; }
    public string CreatedName { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
  }
}
