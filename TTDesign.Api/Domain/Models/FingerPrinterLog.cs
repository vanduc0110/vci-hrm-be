namespace TTDesign.API.Domain.Models
{
  public class FingerPrinterLog : BaseEntity
  {
    /// <summary>
    /// mã timestheet
    /// </summary>
    public long TimesheetId { get; set; }
    /// <summary>
    /// giờ check in sớm nhất trong ngày
    /// </summary>
    public DateTime? DateIn { get; set; }
    /// <summary>
    /// giờ check in muộn nhất trong ngày
    /// </summary>
    public DateTime? DateOut { get; set; }
    /// <summary>
    /// giờ check in sớm nhất trong ngày
    /// </summary>
    public DateTime? DateInUpdate { get; set; }
    /// <summary>
    /// giờ check in muộn nhất trong ngày
    /// </summary>
    public DateTime? DateOutUpdate { get; set; }
    /// <summary>
    /// người cập nhật
    /// </summary>
    public string UpdatedBy { get; set; }
    /// <summary>
    /// ngày cập nhật
    /// </summary>
    public DateTime UpdatedDate { get; set; }
  }
}
