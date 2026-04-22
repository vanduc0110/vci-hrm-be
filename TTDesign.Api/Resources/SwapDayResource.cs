using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  /// <summary>
  /// Class cho model input api [Create]/[Update]
  /// </summary>
  public class SwapDayResource : IValidatableObject
  {
    /// <summary>
    /// ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// Team Name [input] [format]
    /// </summary>
    /// <example>mô tả</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "SwapDayDescription" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Description { get; set; } = null!;

    /// <summary>
    /// ngày được hoán đổi
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "SwapDayFromDate" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public DateTime FromDate { get; set; }

    /// <summary>
    /// ngày hoán đổi
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "SwapDayToDate" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public DateTime ToDate { get; set; }

    /// <summary>
    /// danh sách user áp dụng
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "SwapDayUserId" )]
    public long []? UserIds { get; set; }
    /// <summary>
    /// danh sách thông tin user
    /// </summary>
    [JsonIgnore]
    public HashSet<SwapDayUser>? Users { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      FromDate = FromDate.Date;
      ToDate = ToDate.Date;
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.SwapDays.Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( ErrorMessageResource.ExistFieldError, new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( dbContext.SwapDays.Any( t => t.Id != Id && ( t.FromDate == FromDate || t.ToDate == FromDate ) ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.SwapDayFromDate ), new [] { nameof( FromDate ) } ) );
        }
        else if ( dbContext.SwapDays.Any( t => t.Id != Id && ( t.ToDate == ToDate || t.FromDate == ToDate ) ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.SwapDayToDate ), new [] { nameof( ToDate ) } ) );
        }
        else {
          // valid
          var dateValidMinimum = DateTime.UtcNow.Date.AddDays( 1 - DateTime.UtcNow.Day ).AddMonths( -1 ); // ngày mồng 1 của tháng trước, hôm nay 15/06/23 thì đây là 01/05/23
          var dateValidMaximum = DateTime.UtcNow.Date.AddDays( 1 - DateTime.UtcNow.Day ).AddMonths( 2 ); // ngày mồng 1 của 2 tháng sau, hôm nay 15/06/23 thì đây là 01/08/23
          if ( FromDate < dateValidMinimum || FromDate >= dateValidMaximum ) {
            results.Add( new ValidationResult( string.Format( ErrorMessageResource.DateRangeError, DisplayNameResource.SwapDayFromDate ), new [] { nameof( FromDate ) } ) );
          }
          else if ( ToDate < dateValidMinimum || ToDate >= dateValidMaximum ) {
            results.Add( new ValidationResult( string.Format( ErrorMessageResource.DateRangeError, DisplayNameResource.SwapDayToDate ), new [] { nameof( ToDate ) } ) );
          }// valid ngày request đã bị lock timesheet chưa
          else if ( dbContext.Timesheets.Where( t => t.LockBy != null && ( t.Date == FromDate || t.Date == ToDate ) ).FirstOrDefault() is not null ) {
            results.Add( new ValidationResult( string.Format( ErrorMessageResource.HadLocked, DisplayNameResource.SwapDayFromDate ), new [] { nameof( FromDate ) } ) );
          } // valid ngày được hoán đổi phải là ngày trong tuần và ko phải ngày lễ
          else if ( FromDate.DayOfWeek == DayOfWeek.Sunday ||
            dbContext.Holidays.Any( h => h.Type == ( int ) Enums.HolidayType.Holiday && h.Status == ( int ) Enums.HolidayStatus.Apply && h.StartDate <= FromDate && h.EndDate >= FromDate ) ) {
            results.Add( new ValidationResult( string.Format( ErrorMessageResource.TimeMustWeekday, DisplayNameResource.SwapDayFromDate ), new [] { nameof( FromDate ) } ) );
          } // valid ngày hoán đổi là ngày cuối tuần và ko phải ngày lễ
          else if ( ToDate.DayOfWeek != DayOfWeek.Sunday ||
            dbContext.Holidays.Any( h => h.Type == ( int ) Enums.HolidayType.Holiday && h.Status == ( int ) Enums.HolidayStatus.Apply && h.StartDate <= ToDate && h.EndDate >= ToDate ) ) {
            results.Add( new ValidationResult( string.Format( ErrorMessageResource.TimeMustWeekend, DisplayNameResource.SwapDayToDate ), new [] { nameof( ToDate ) } ) );
          }
        }
        if ( UserIds == null || !UserIds.Any() ) {
          UserIds = null;
        }
        else {
          var users = new HashSet<SwapDayUser>();
          foreach ( var userId in UserIds ) {
            var userName = dbContext.Users.Where( u => userId != Enums.SYSTEM_ID && u.Id == userId && u.IsActive ).Select( u => u.UserName ).FirstOrDefault();
            if ( userName is null ) {
              results.Add( new ValidationResult( string.Format( ErrorMessageResource.UserNotFound, DisplayNameResource.SwapDayUserId ), new [] { nameof( UserIds ) } ) );
              break;
            }
            users.Add( new SwapDayUser() { UserId = userId, UserName = userName } );
          }
          Users = users;
        }
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class SwapDayResponse
  {
    /// <summary>
    /// Record ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Description
    /// </summary>
    /// <example>text</example>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// danh sách user áp dụng
    /// </summary>
    /// <example>text</example>
    public string Users { get; set; } = string.Empty;
    /// <summary>
    /// danh sách user id áp dụng
    /// </summary>
    /// <example>text</example>
    public long [] UserIds { get; set; } = new long [] { };
    /// <summary>
    /// ngày được hoán đổi
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string FromDateText => FromDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// ngày hoán đổi
    /// </summary>
    public DateTime ToDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string ToDateText => ToDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// người tạo
    /// </summary>
    public long CreatedBy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string CreatedByName { get; set; } = string.Empty;
    /// <summary>
    /// ngày tạo
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string CreatedByText => CreatedDate.ToString( Enums.DATE_FORMAT );
  }
}
