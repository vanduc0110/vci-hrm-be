using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  /// <summary>
  /// Class cho model input api [Create]/[Update]
  /// </summary>
  public class WfhResource : IValidatableObject
  {
    /// <summary>
    /// WFH request ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "WfhStart" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "WfhEnd" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Mô tả công việc OT [input]
    /// </summary>
    /// <example>lý do</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "WfhReason" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 500, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var httpContextAccessor = validationContext.GetService( typeof( IHttpContextAccessor ) ) as IHttpContextAccessor;
      var userLogin = long.Parse( httpContextAccessor!.HttpContext!.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );

      // format data
      StartTime = StartTime.Date;
      EndTime = EndTime.Date;

      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      // valid exit record
      if ( dbContext is null || ( Id is not null && !dbContext.WfhRequests.Any( t => t.Id == Id && t.CreatedBy == userLogin ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Wfh ), new [] { Enums.ERROR_TEXT } ) );
        return results;
      }
      // valid record is approved status for case update
      if ( Id is not null && dbContext.WfhRequests.Any( t => t.Id == Id && t.Status == ( int ) Enums.WfhRequestStatus.Approve ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Wfh ), new [] { Enums.ERROR_TEXT } ) );
        return results;
      }
      var dateValidMinimum = DateTime.Now.Date.AddDays( 1 - DateTime.Now.Day ).AddMonths( -2 ); // ngày mồng 1 của tháng trước, hôm nay 15/06/23 thì đây là 01/05/23
      var dateValidMaximum = DateTime.Now.Date.AddDays( 1 - DateTime.Now.Day ).AddMonths( 2 ); // ngày mồng 1 của 2 tháng sau, hôm nay 15/06/23 thì đây là 01/08/23
      if ( StartTime < dateValidMinimum || StartTime >= dateValidMaximum ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.DateRangeError, DisplayNameResource.WfhStart ), new [] { nameof( StartTime ) } ) );
      }
      if ( EndTime < dateValidMinimum || EndTime >= dateValidMaximum ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.DateRangeError, DisplayNameResource.WfhStart ), new [] { nameof( EndTime ) } ) );
      }
      if ( EndTime < StartTime ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.DatePastError, DisplayNameResource.Wfh ), new [] { nameof( StartTime ) } ) );
      }
      // valid ngày request đã bị lock timesheet chưa
      else if ( dbContext.Timesheets.Any( t => t.UserId == userLogin && StartTime <= t.Date && t.Date <= EndTime && t.LockBy != null ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.HadLocked, DisplayNameResource.Wfh ), new [] { Enums.ERROR_TEXT } ) );
      }
      // valid exist request
      else if ( dbContext.WfhRequests.Any( t => t.Id != Id && t.CreatedBy == userLogin && t.StartTime <= EndTime && StartTime <= t.EndTime ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.HadExistError, DisplayNameResource.Wfh ), new [] { Enums.ERROR_TEXT } ) );
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class WfhResponse : BaseModelResponse
  {
    /// <summary>
    /// Thời gian bắt đầu
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Required]
    public string StartTime { get; set; } = string.Empty;
    /// <summary>
    /// thời gian kết thúc
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Required]
    public string EndTime { get; set; } = string.Empty;
    /// <summary>
    /// trạng thái
    /// </summary>
    /// <example>Pending</example>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Mô tả công việc
    /// </summary>
    /// <example>lý do</example>
    public string Reason { get; set; } = string.Empty;
    /// <summary>
    /// số ngày xin wfh
    /// </summary>
    public int Days { get; set; }
  }
}
