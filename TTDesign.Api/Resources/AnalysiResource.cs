using System.ComponentModel.DataAnnotations;
using TTDesign.API.Constants;

namespace TTDesign.API.Resources
{
  /// <summary>
  /// Class cho model output api GetAnalysisView [Get Detail]
  /// </summary>
  public class AnalysisResponse
  {
    /// <summary>
    /// Project ID
    /// </summary>
    [Required]
    public long ProjectId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string ProjectName { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    /// <example>Pending</example>
    public string Status { get; set; } = null!;
    public string ProjectType { get; set; } = null!;
    public string Client { get; set; } = null!;
    /// <summary>
    /// thông số tổng hợp của Project
    /// </summary>
    [Required]
    public ProjectSummary Summary { get; set; } = null!;
    /// <summary>
    /// tổng hợp thời gian làm việc theo category
    /// </summary>
    [Required]
    public ProjectObjectDetail [] Categories { get; set; } = null!;
    /// <summary>
    /// tổng hợp thời gian làm việc theo user
    /// </summary>
    [Required]
    public ProjectUserDetail [] Users { get; set; } = null!;
  }

  /// <summary>
  /// chi tiết thời gian phân bổ theo object trong project
  /// </summary>
  public class ProjectSummary
  {
    /// <summary>
    /// tên team phụ trách
    /// </summary>
    /// <example>text</example>
    public string TeamName { get; set; } = null!;
    /// <summary>
    /// user quản lý
    /// </summary>
    /// <example>text</example>
    public string Manager { get; set; } = null!;
    /// <summary>
    /// tổng số member
    /// </summary>
    public int TotalMember { get; set; }
    /// <summary>
    /// thời gian bắt đầu
    /// </summary>
    public DateTime Start { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string StartText => Start.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// thời gian kết thúc
    /// </summary>
    public DateTime? End { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string EndText => End != null ? ( ( DateTime ) End ).ToString( Enums.DATE_FORMAT ) : string.Empty;
    /// <summary>
    /// tổng thời gian làm việc
    /// </summary>
    public double TotalWorkingHour { get; set; }
    /// <summary>
    /// tổng thời gian
    /// </summary>
    public double TotalHour => TotalWorkingHour;
    /// <summary>
    /// 
    /// </summary>
    public double Quotation { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public double TotalWorking => TotalHour;

    public long QuotationDay { get; set; } = 0;
    public double CurrentDay { get; set; } = 0;
    /// <summary>
    /// 
    /// </summary>
    /// <example></example>
    public string UsagePercentage => TotalWorking > 0 && Quotation > 0 ? string.Format( "{0:0.##%}", TotalWorking / Quotation ) : string.Empty;
  }

  /// <summary>
  /// chi tiết thời gian phân bổ theo object trong project
  /// </summary>
  /// </summary>
  public class ProjectObjectDetail
  {
    /// <summary>
    /// category/object name
    /// </summary>
    /// <example>text</example>
    public string Name { get; set; } = null!;
    /// <summary>
    /// tổng số giờ làm trên project
    /// </summary>
    public double WorkingHours { get; set; }
  }

  /// <summary>
  /// chi tiết thời gian làm việc của user trong project
  /// </summary>
  public class ProjectUserDetail
  {
    /// <summary>
    /// user name
    /// </summary>
    /// <example>text</example>
    public string Name { get; set; } = null!;
    /// <summary>
    /// tổng số giờ làm trên project
    /// </summary>
    public double WorkingHours { get; set; }
  }

  /// <summary>
  /// thông tin về tài liệu đính kèm
  /// </summary>
  public class ProjectDocumentDetail
  {
    public long Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string Name { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string Comment { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string Link { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string CreatedDateText => CreatedDate.ToString( Enums.DATE_FORMAT );
  }
  public class AnalysisFilter
  {
    public AnalysisFilter()
    {
      Categories = new HashSet<ProjectObjectDetail>();
    }
    public ICollection<ProjectObjectDetail> Categories { get; set; }
  }
}
