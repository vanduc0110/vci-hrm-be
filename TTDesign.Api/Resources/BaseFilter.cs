namespace TTDesign.API.Resources
{
  public class BaseFilter
  {
    /// <summary>
    /// điều kiện tìm kiếm theo team
    /// </summary>
    public long? TeamId { get; set; }
    /// <summary>
    /// điều kiện tìm kiếm theo user
    /// </summary>
    public long? UserId { get; set; }
    /// <summary>
    /// điều kiện tìm kiếm theo position
    /// </summary>
    public int? Position { get; set; }
    public long Year { get; set; }
    public long Month { get; set; }
    /// <summary>
    /// điều kiện tìm kiếm: ngày bắt đầu
    /// </summary>
    public DateTime Start { get; set; }
    /// <summary>
    /// điều kiện tìm kiếm: ngày kết thúc
    /// </summary>
    public DateTime End { get; set; }
    public DateTime DateCheck { get; set; }
    /// <summary>
    /// điều kiện tìm kiếm: calendar object ID
    /// </summary>
    public long CalendarObjectId { get; set; }
    /// <summary>
    /// điều kiện tìm kiếm: đối tượng id
    /// </summary>
    public long ObjectId { get; set; }
    /// <summary>
    /// điều kiện lọc status report                                                                                                                                                                                                                                                      ==
    /// </summary>
    public int? Status { get; set; } = -1;
    public string? AssetStatus { get; set; }

    public long? AssetCategoryId { get; set; }
  }
}
