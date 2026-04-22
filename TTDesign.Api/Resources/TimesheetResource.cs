using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TTDesign.API.Constants;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  #region model timesheet detail để cho user khai báo timesheet theo từng ngày
  /// <summary>
  /// Class cho model input api (khai báo timesheet)
  /// </summary>
  public class TimesheetResource : IValidatableObject
  {
    /// <summary>
    /// Timesheet ID [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "Timesheet" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public long Id { get; set; }
    /// <summary>
    /// user khai báo timesheet task [not input]
    /// </summary>
    [JsonIgnore]
    public long UserId { get; set; }
    /// <summary>
    /// danh sách project [input]
    /// </summary>
    public List<TimesheetProjectResourceData>? Projects { get; set; }
    /// <summary>
    /// lỗi khi valid: lỗi chung
    /// </summary>
    /// <example></example>
    public string Error { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || !dbContext.Timesheets.Any( t => t.Id == Id ) ) {
        results.Add( new ValidationResult( ErrorMessageResource.ExistFieldError, new [] { Enums.ERROR_TEXT } ) );
      }
      return results;
    }
  }

  /// <summary>
  /// project để log work
  /// </summary>
  public class TimesheetProjectResourceData
  {
    /// <summary>
    /// Project ID [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "TimesheetProject" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public long ProjectId { get; set; }
    /// <summary>
    /// lỗi khi valid projectId
    /// </summary>
    /// <example>text</example>
    public string ErrorProject { get; set; } = string.Empty;
    /// <summary>
    /// danh sách task [input]
    /// </summary>
    public List<TimesheetResourceData>? Tasks { get; set; }
  }

  /// <summary>
  /// model khai báo các chi tiết task
  /// </summary>
  public class TimesheetResourceData
  {
    /// <summary>
    /// category
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "TimesheetCategory" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public long CategoryId { get; set; }
    /// <summary>
    /// lỗi khi valid categoryId
    /// </summary>
    /// <example></example>
    public string ErrorCategory { get; set; } = string.Empty;
    /// <summary>
    /// object
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "TimesheetObject" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public long ObjectId { get; set; }
    /// <summary>
    /// lỗi khi valid objectId
    /// </summary>
    /// <example></example>
    public string ErrorObject { get; set; } = string.Empty;
    /// <summary>
    /// mô tả công việc
    /// </summary>
    /// <example>mô tả</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "TimesheetDescription" )]
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// lỗi khi valid description
    /// </summary>
    /// <example></example>
    public string ErrorDescription { get; set; } = string.Empty;
    /// <summary>
    /// thời gian (đơn vị giờ), block 15'
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "TimesheetHour" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public string Hours { get; set; } = string.Empty;
    /// <summary>
    /// lỗi khi valid hour
    /// </summary>
    /// <example></example>
    public string ErrorHour { get; set; } = string.Empty;
    /// <summary>
    /// giá trị Hour sau khi valid và format
    /// </summary>
    public double HourValid { get; set; }
  }
  #endregion

  #region model để user xem tổng quát timesheet trong tháng
  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class TimesheetResponse
  {
    /// <summary>
    /// Record ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// trạng thái timesheet: true (lock) false (unclock)
    /// </summary>
    public bool Lock { get; set; }
    /// <summary>
    /// trạng thái đăng ký WFH: true (đăng ký WFH) false (không đăng ký WFH)
    /// </summary>
    public bool IsWfh { get; set; }
    /// <summary>
    /// check in (theo chấm công/đăng ký WFH)
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Required]
    public string TimeIn { get; set; } = string.Empty;
    /// <summary>
    /// check out (theo chấm công/đăng ký WFH)
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Required]
    public string TimeOut { get; set; } = string.Empty;
    /// <summary>
    /// ngày công
    /// </summary>
    [Required]
    public DateTime Date { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string DateText => Date.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// Tên ngày lễ, nếu blank là ngày thường, ngược lại là ngày lễ
    /// </summary>
    /// <example></example>
    public string? HolidayName { get; set; } = string.Empty;
    /// <summary>
    /// nội dung của ngày hoán đổi
    /// </summary>
    /// <example></example>
    public string? SwapDayDetail { get; set; } = string.Empty;
    /// <summary>
    /// tổng số giờ làm việc theo khai báo
    /// </summary>
    public double HourWorking { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string HourWorkingStr => Common.FormatHoursDoubleToString( HourWorking );
    /// <summary>
    /// tổng số giờ theo thời gian check in - check out
    /// </summary>
    public double HourTotal { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string HourTotalStr => Common.FormatHoursDoubleToString( HourTotal );
    /// <summary>
    /// task theo project khai báo trong ngày
    /// </summary>
    public List<TimesheetData>? Tasks { get; set; }
  }

  /// <summary>
  /// danh sách thu gọn các project làm việc trong 1 ngày
  /// </summary>
  public class TimesheetData
  {
    /// <summary>
    /// project IDs
    /// </summary>
    public long ProjectId { get; set; }
    /// <summary>
    /// tên project
    /// </summary>
    /// <example>text</example>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// số giờ làm: thời gian phân bổ cho project
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string Hours { get; set; } = string.Empty;
    /// <summary>
    /// loại: project (bao gồm cả project và OT)/ leave
    /// </summary>
    /// <example>text</example>
    public string Type { get; set; } = string.Empty;
  }
  #endregion

  #region model trả về thông tin timesheet detail trong 1 ngày cho user
  /// <summary>
  /// Class cho model output api [Get View]
  /// </summary>
  public class TimesheetDetailResponse
  {
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// trạng thái timesheet: true (lock) false (unclock)
    /// </summary>
    public bool Lock { get; set; }
    /// <summary>
    /// nếu ngày hôm đó đăng ký wfh được approve, giá trị là true, và ngược lại false
    /// </summary>
    /// <example>false</example>
    public bool IsWfh { get; set; }
    /// <summary>
    /// check in (theo chấm công)
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Required]
    public string TimeIn { get; set; } = string.Empty;
    /// <summary>
    /// check out (theo chấm công)
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Required]
    public string TimeOut { get; set; } = string.Empty;
    /// <summary>
    /// tổng số giờ làm việc theo khai báo
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string HourWorking { get; set; } = string.Empty;
    /// <summary>
    /// tổng số giờ theo thời gian check in - check out
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string HourTotal { get; set; } = string.Empty;
    /// <summary>
    /// task chi tiết được khai báo trong ngày
    /// </summary>
    public List<TimesheetDetailProjectData>? Projects { get; set; }
  }

  /// <summary>
  /// model project trong chi tiết timesheet
  /// </summary>
  public class TimesheetDetailProjectData
  {
    /// <summary>
    /// project ID
    /// </summary>
    public long ProjectId { get; set; }
    /// <summary>
    /// project name
    /// </summary>
    /// <example>text</example>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// thời gian (đơn vị giờ)
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string Hours { get; set; } = string.Empty;
    /// <summary>
    /// task chi tiết được khai báo trong ngày
    /// </summary>
    public List<TimesheetDetailData>? Tasks { get; set; }
    public List<CategoryProject>? Categories { get; set; }
  }

  public class TimesheetDetailData
  {
    /// <summary>
    /// timesheet detail id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// category
    /// </summary>
    public long CategoryId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string Category { get; set; } = string.Empty;
    /// <summary>
    /// object
    /// </summary>
    public long ObjectId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string Object { get; set; } = string.Empty;
    /// <summary>
    /// mô tả công việc
    /// </summary>
    /// <example>mô tả</example>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// loại: thường/OT
    /// </summary>
    /// <example></example>
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// thời gian (đơn vị giờ)
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string Hours { get; set; } = string.Empty;
  }
  #endregion

  /// <summary>
  /// model request lock/unlock
  /// </summary>
  public class TimesheetRequestLock
  {
    /// <summary>
    /// ngày bắt đầu
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    public DateTime Start { get; set; }
    public string StartText => Start.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// ngày kết thúc
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    public DateTime End { get; set; }
    public string EndText => End.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// true: request lock, false: request unlock
    /// </summary>
    /// <example>true</example>
    public bool IsLock { get; set; }
  }

  /// <summary>
  /// model phục vụ view dashboard, chứa thông tin hôm nay, checkin-checkout
  /// </summary>
  public class DashboardTimesheet
  {
    /// <summary>
    /// ngày hiện tại của hệ thống
    /// </summary>
    public DateTime Today { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string TodayText => Today.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// giờ check in
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string TimeIn { get; set; } = string.Empty;
    /// <summary>
    /// giờ check out
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string TimeOut { get; set; } = string.Empty;
  }

  /// <summary>
  /// model phục vụ view dashboard: project
  /// </summary>
  public class DashboardProject
  {
    /// <summary>
    /// tổng số giờ log time trong tháng
    /// </summary>
    public double WorkingHour { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string StrWorkingHour => Common.FormatHoursDoubleToString( WorkingHour );
    /// <summary>
    /// tổng số giờ leave trong tháng (annual)
    /// </summary>
    public double LeaveHour { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string StrLeaveHour => Common.FormatHoursDoubleToString( LeaveHour );
    /// <summary>
    /// tổng số giờ
    /// </summary>
    public double TotalHour => WorkingHour;
    /// <summary>
    /// thời gian làm việc theo từng project trong tháng
    /// </summary>
    public HashSet<DashboardProjectDetail> Projects { get; set; } = null!;
  }

  /// <summary>
  /// chi tiết thời gian phân bổ theo project trong tháng
  /// </summary>
  public class DashboardProjectDetail
  {
    /// <summary>
    /// tên project
    /// </summary>
    /// <example>text</example>
    public string Name { get; set; } = null!;
    /// <summary>
    /// tổng số giờ làm trong tháng trên project (log time, overtime)
    /// </summary>
    public double WorkingHours { get; set; }
  }

  /// <summary>
  /// model phục vụ view report
  /// </summary>
  public class TimesheetReportDetail
  {
    /// <summary>
    /// thông tin user
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string UserName { get; set; } = string.Empty;
    public List<TeamUserOption> Teams { get; set; } = new List<TeamUserOption>();
    /// <summary>
    /// tổng số ngày công làm việc (đơn vị ngày)
    /// </summary>
    public double WorkingDay { get; set; }
    /// <summary>
    /// số giờ work log (đơn vị giờ:phút)
    /// </summary>
    public double WorkLog { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string WorkLogStr => Common.FormatHoursDoubleToString( WorkLog );
    /// <summary>
    /// số giờ theo nghỉ lễ (đơn vị giờ)
    /// </summary>
    public double Holiday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string HolidayStr => Common.FormatHoursDoubleToString( Holiday );
    /// <summary>
    /// số giờ nghỉ không lương (đơn vị giờ:phút)
    /// </summary>
    public double UnpaidLeave { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string UnpaidLeaveStr => Common.FormatHoursDoubleToString( UnpaidLeave );
    /// <summary>
    /// số giờ nghỉ phép (đơn vị giờ:phút)
    /// </summary>
    public double PaidLeave { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string PaidLeaveStr => Common.FormatHoursDoubleToString( PaidLeave );
  }

  public class DashboardTime
  {
    /// <summary>
    /// tổng số ngày công làm việc (đơn vị ngày)
    /// </summary>
    public double WorkingDay { get; set; }
    /// <summary>
    /// số giờ work log (đơn vị giờ:phút)
    /// </summary>
    public double WorkLog { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string WorkLogStr => Common.FormatHoursDoubleToString( WorkLog );
    /// <summary>
    /// số giờ theo nghỉ lễ (đơn vị giờ)
    /// </summary>
    public double Holiday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string HolidayStr => Common.FormatHoursDoubleToString( Holiday );
    /// <summary>
    /// số giờ nghỉ không lương (đơn vị giờ:phút)
    /// </summary>
    public double UnpaidLeave { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string UnpaidLeaveStr => Common.FormatHoursDoubleToString( UnpaidLeave );
    /// <summary>
    /// số giờ nghỉ phép (đơn vị giờ:phút)
    /// </summary>
    public double PaidLeave { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string PaidLeaveStr => Common.FormatHoursDoubleToString( PaidLeave );
  }
}
