namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// thông tin các kỳ nghỉ
  /// </summary>
  public partial class Holiday : BaseEntity
  {
    public Holiday()
    {
      HolidayApplys = new HashSet<HolidayApply>();
    }

    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// ngày bắt đầu
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// ngày kết thúc
    /// </summary>
    public DateTime EndDate { get; set; }
    /// <summary>
    /// loại nghỉ: 0 holiday (kỳ nghỉ của cty)/ 1 special (kỳ nghỉ đặc biệt của cá nhân)
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// 0 pending (mới tạo, đang chờ apply vào timesheet), 1 Apply (đã được accept vào timesheet), 2 Deleting (request xóa, đang chờ xóa khỏi timesheet)
    /// </summary>
    public int Status { get; set; }

    public virtual ICollection<HolidayApply> HolidayApplys { get; set; }
  }
}
