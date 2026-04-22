namespace TTDesign.API.Resources
{
  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class LeaveInformationResponse
  {
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Leave Type
    /// </summary>
    /// <example>Annual</example>
    public string Type { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string TypeName { get; set; } = null!;
    /// <summary>
    /// Loại ngày nghỉ
    /// </summary>
    /// <example>text</example>
    public string Detail { get; set; } = null!;
    /// <summary>
    /// số ngày nghỉ
    /// </summary>
    /// <example>text</example>
    public string LeaveDay { get; set; } = null!;
    /// <summary>
    /// Thời gian bắt đầu
    /// </summary>
    /// <example>text</example>
    public string StartCondition { get; set; } = null!;
    /// <summary>
    /// thời gian kết thúc
    /// </summary>
    /// <example>text</example>
    public string End { get; set; } = null!;
    /// <summary>
    /// Block
    /// </summary>
    /// <example>text</example>
    public string Block { get; set; } = null!;
    /// <summary>
    /// Cách sử dụng
    /// </summary>
    /// <example>text</example>
    public string Using { get; set; } = null!;
    /// <summary>
    /// Lưu ý
    /// </summary>
    /// <example>ghi chú</example>
    public string Note { get; set; } = null!;
  }
}
