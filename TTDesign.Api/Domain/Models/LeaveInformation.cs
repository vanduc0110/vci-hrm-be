namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// thông tin các loại hình nghỉ quy định trong hệ thống
  /// </summary>
  public partial class LeaveInformation
  {
    public long Id { get; set; }
    public string Type { get; set; } = null!;
    public string TypeName { get; set; } = null!;
    public string Detail { get; set; } = null!;
    public string LeaveDay { get; set; } = null!;
    public string StartCondition { get; set; } = null!;
    public string End { get; set; } = null!;
    public string Block { get; set; } = null!;
    public string Using { get; set; } = null!;
    public string Note { get; set; } = null!;
  }
}
