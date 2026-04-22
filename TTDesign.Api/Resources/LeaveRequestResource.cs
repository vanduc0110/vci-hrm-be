using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TTDesign.API.Constants;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  /// <summary>
  /// Class cho model input api [Create]/[Update]
  /// </summary>
  public class LeaveRequestResource : IValidatableObject
  {
    /// <summary>
    /// Leave Request ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// Reason [input]
    /// </summary>
    /// <example>lý do</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "LeaveRequestReason" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 500, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Reason { get; set; } = null!;
    /// <summary>
    /// Date Start [input]
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "LeaveRequestStartDate" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public DateTime StartDate { get; set; }
    /// <summary>
    /// Start Time [input]
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "LeaveRequestStartTime" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public string StartTime { get; set; } = null!;
    /// <summary>
    /// Date End [input]
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "LeaveRequestEndDate" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public DateTime EndDate { get; set; }
    /// <summary>
    /// End Time [input]
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "LeaveRequestEndTime" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public string EndTime { get; set; } = null!;
    /// <summary>
    /// Type [input]
    /// </summary>
    /// <example>Annual</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "LeaveRequestType" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public string Type { get; set; } = null!;
    /// <summary>
    /// giá trị thời gian leave sau khi được tính toán, đơn vị giờ [not input]
    /// </summary>
    [JsonIgnore]
    public double Hours { get; set; }
    /// <summary>
    /// danh sách thời gian nghỉ chi tiết từng ngày
    /// </summary>
    [JsonIgnore]
    public List<LeaveRequestDetailData>? LeaveDetail { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.LeaveRequests.AsNoTracking().Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.LeaveRequest ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( !string.IsNullOrEmpty( Type ) && !Enum.IsDefined( typeof( Enums.LeaveType ), Type ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.LeaveRequestType ), new [] { nameof( Type ) } ) );
        }
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class LeaveRequestResponse
  {
    /// <summary>
    /// Leave Request ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// Trạng thái request
    /// </summary>
    /// <example>Pending</example>
    [Required]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// ngày giờ bắt đầu
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string StartDateText => StartDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Required]
    public string StartTime { get; set; } = string.Empty;
    /// <summary>
    /// ngày giờ kết thúc
    /// </summary>
    public DateTime EndDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string EndDateText => EndDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Required]
    public string EndTime { get; set; } = string.Empty;
    /// <summary>
    /// hình thức trả
    /// </summary>
    /// <example>Salary</example>
    [Required]
    public string PaidBy => Type == Enum.GetName( Enums.LeaveType.SelfWedding )! || Type == Enum.GetName( Enums.LeaveType.FamilyWedding )! ||
      Type == Enum.GetName( Enums.LeaveType.FamilyBereavement )! || Type == Enum.GetName( Enums.LeaveType.RelativeBereavement )! ||
      Type == Enum.GetName( Enums.LeaveType.Annual )! ? Enum.GetName( Enums.LeaveRequestPaid.Salary )! :
      ( Type == Enum.GetName( Enums.LeaveType.SelfMaternity )! || Type == Enum.GetName( Enums.LeaveType.FamilyMaternity )! ? Enum.GetName( Enums.LeaveRequestPaid.SocialInsurance ) :
      Enum.GetName( Enums.LeaveRequestPaid.Unpaid ) )!;
    /// <summary>
    /// thời gian nghỉ
    /// </summary>
    /// <example>text</example>
    [Required]
    public string TimeLeave { get; set; } = string.Empty;
    /// <summary>
    /// lý do nghỉ
    /// </summary>
    /// <example>lý do</example>
    [Required]
    public string Reason { get; set; } = string.Empty;
    /// <summary>
    /// người xử lý request
    /// </summary>
    public long Reviewer { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string ReviewerName { get; set; } = string.Empty;
    /// <summary>
    /// hình thức nghỉ
    /// </summary>
    /// <example>Annual</example>
    [Required]
    public string Type { get; set; } = string.Empty;
    /// <summary>
    ///  người tạo request
    /// </summary>
    public long CreatedBy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    [Required]
    public string CreatorName { get; set; } = string.Empty;
    public List<TeamUserOption> Teams { get; set; } = new List<TeamUserOption>();
  }

  /// <summary>
  /// class phục vụ cho tổng hợp số ngày nghỉ còn lại
  /// </summary>
  public class RemainLeave
  {
    /// <summary>
    /// số ngày nghỉ phép còn lại, đơn vị ngày
    /// </summary>
    public double Annual { get; set; }
  }

  /// <summary>
  /// class phục vụ cho danh sách lịch sử thay đổi ngày nghỉ
  /// </summary>
  public class LeaveHistoryResponse
  {
    /// <summary>
    /// mô tả hành động đã thực hiện
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// trạng thái thay đổi 
    /// </summary>
    [Required]
    public string State { get; set; } = string.Empty;
    /// <summary>
    /// đơn vị thay đổi
    /// </summary>
    [Required]
    public double Unit { get; set; }
    /// <summary>
    /// ngày cập nhật
    /// </summary>
    public DateTime CreatedDate { get; set; }
    public string CreatedDateText => CreatedDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// số ngày nghỉ phép còn lại, đơn vị ngày
    /// </summary>
    public double Annual { get; set; }
    public long ModifiedBy { get; set; }
    public string ModifiedName { get; set; } = null!;
    public DateTime ModifiedDate { get; set; }
    public string ModifiedDateText => ModifiedDate.ToString( Enums.DATE_FORMAT );
  }

  /// <summary>
  /// chi tiết thời gian nghỉ theo các ngày của 1 request leave
  /// </summary>
  public class LeaveRequestDetailData
  {
    /// <summary>
    /// ngày
    /// </summary>
    public DateTime Date { get; set; }
    public string DateText => Date.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// thời gian nghỉ theo ngày
    /// </summary>
    public double Hours { get; set; }
  }

  /// <summary>
  /// model phục vụ view report leave
  /// </summary>
  public class LeaveReport
  {
    /// <summary>
    /// lịch sử nghỉ phép
    /// </summary>
    public LeaveReport()
    {
      AnnualLeaves = new List<LeaveReportDetail>();
    }
    public List<LeaveReportDetail> AnnualLeaves { get; set; } = null!;
  }

  /// <summary>
  /// model phục vụ view report leave: chi tiết record
  /// </summary>
  public class LeaveReportDetail
  {
    public LeaveReportDetail()
    {
      Jan = new List<LeaveReportMonthly>();
      Feb = new List<LeaveReportMonthly>();
      Mar = new List<LeaveReportMonthly>();
      Apr = new List<LeaveReportMonthly>();
      May = new List<LeaveReportMonthly>();
      Jun = new List<LeaveReportMonthly>();
      Jul = new List<LeaveReportMonthly>();
      Aug = new List<LeaveReportMonthly>();
      Sep = new List<LeaveReportMonthly>();
      Oct = new List<LeaveReportMonthly>();
      Nov = new List<LeaveReportMonthly>();
      Dec = new List<LeaveReportMonthly>();
    }
    /// <summary>
    /// user name
    /// </summary>
    [Required]
    public long UserId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string Staff { get; set; } = string.Empty;
    /// <summary>
    /// team của user
    /// </summary>
    [Required]
    public long [] TeamId { get; set; } = new long [ 0 ];
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string Team { get; set; } = string.Empty;
    /// <summary>
    /// tổng số ngày nghỉ được cấp trong năm
    /// </summary>
    public double Total { get; set; }
    /// <summary>
    /// tổng số ngày phép đã sử dụng trong năm
    /// </summary>
    public double TotalUsing { get; set; }
    public double Remain => Total - TotalUsing;
    /// <summary>
    /// ngày phép đã được sử dụng như thế nào theo các tháng, đã sắp xếp 
    /// </summary>
    public List<LeaveReportMonthly> Jan { get; set; }
    public List<LeaveReportMonthly> Feb { get; set; }
    public List<LeaveReportMonthly> Mar { get; set; }
    public List<LeaveReportMonthly> Apr { get; set; }
    public List<LeaveReportMonthly> May { get; set; }
    public List<LeaveReportMonthly> Jun { get; set; }
    public List<LeaveReportMonthly> Jul { get; set; }
    public List<LeaveReportMonthly> Aug { get; set; }
    public List<LeaveReportMonthly> Sep { get; set; }
    public List<LeaveReportMonthly> Oct { get; set; }
    public List<LeaveReportMonthly> Nov { get; set; }
    public List<LeaveReportMonthly> Dec { get; set; }
  }
  public class LeaveReportMonthly
  {
    public string Type { get; set; } = string.Empty;
    public double Day { get; set; }
  }
}
