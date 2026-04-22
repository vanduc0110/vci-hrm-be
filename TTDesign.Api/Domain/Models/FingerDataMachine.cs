namespace TTDesign.API.Domain.Models
{
  public class FingerDataMachine : BaseCreateEntity
  {

    public string SN { get; set; } = string.Empty;
    public string Table { get; set; } = string.Empty;
    public int EmpId { get; set; }
    public DateTime PunchDate { get; set; }
    public int Status1 { get; set; }
    public int VerifyType { get; set; }
    public int Status2 { get; set; }
    public int Status3 { get; set; }
    public int Status4 { get; set; }
    public int Status5 { get; set; }
    public int Status6 { get; set; }
    public int Status7 { get; set; }
  }
}
