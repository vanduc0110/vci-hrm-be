namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// đối tượng được hưởng kì nghỉ (chỉ áp dụng cho holiday type là special)
  /// </summary>
  public partial class HolidayApply
  {
    public long HolidayId { get; set; }
    /// <summary>
    /// đối tượng áp dụng: user id/ team id/ group id
    /// </summary>
    public long ApplyId { get; set; }
    /// <summary>
    /// đối tượng: User/ Team
    /// </summary>
    public string Type { get; set; } = null!;
    public virtual Holiday Holiday { get; set; } = null!;
  }
}
