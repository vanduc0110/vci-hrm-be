using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TTDesign.API.Constants;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  /// <summary>
  /// Class cho model input api [Create]/[Update]
  /// </summary>
  public class HolidayResource : IValidatableObject
  {
    /// <summary>
    /// Holiday ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// Name [input] [format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "HoliadyName" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Team Code [input, format]
    /// </summary>
    /// <example>Holiday</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "HolidayType" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 30, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Type { get; set; } = null!;

    /// <summary>
    /// Date Start [input]
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "HolidayStartDate" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Date End [input]
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "HolidayEndDate" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// danh sách apply [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "HolidayApply" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public BaseMember []? ApplyFor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      if ( !string.IsNullOrEmpty( Name ) ) {
        Name = Common.RemoveMultiBlank( Name );
      }
      StartDate = StartDate.Date;
      EndDate = EndDate.Date;
      if ( Type == Enum.GetName( Enums.HolidayType.Holiday ) ) {
        ApplyFor = null;
      }
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.Holidays.Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( ErrorMessageResource.ExistFieldError, new [] { Enums.ERROR_TEXT } ) );
      }
      // [TODO] đóng tính năng tạo holiday special vì đang phức tạp và chưa có yêu cầu sử dụng
      else if ( !string.IsNullOrEmpty( Type ) && Type == Enum.GetName( Enums.HolidayType.Special ) ) {
        results.Add( new ValidationResult( ErrorMessageResource.ComingSoon, new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( !string.IsNullOrEmpty( Type ) && !Enum.IsDefined( typeof( Enums.HolidayType ), Type ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.HolidayType ), new [] { nameof( Type ) } ) );
        }
        if ( StartDate.Year < DateTime.UtcNow.Year ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.HolidayDateRangeError, DisplayNameResource.HolidayStartDate ), new [] { nameof( StartDate ) } ) );
        }
        else if ( StartDate > EndDate ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.DateRangeError, DisplayNameResource.HolidayStartDate ), new [] { nameof( StartDate ) } ) );
        }
        else if ( dbContext.Holidays.Any( h => h.StartDate.Year == StartDate.Year && h.StartDate < EndDate && StartDate < h.EndDate && h.Id != Id ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.HadExistError, DisplayNameResource.HolidayStartDate ), new [] { nameof( StartDate ) } ) );
        }
        if ( ApplyFor != null && ApplyFor.Count() > 0 ) {
          var listAfterValid = new List<BaseMember>();
          var listUser = new List<long>();
          bool isValid = false;
          foreach ( var obj in ApplyFor.Where( h => h.Type == Enum.GetName( Enums.DynamicOption.Team ) ).Distinct() ) {
            var team = dbContext.Teams.Include( t => t.TeamUsers ).ThenInclude( t => t.User ).Where( u => u.Id == obj.Id ).FirstOrDefault();
            if ( team == null ) {
              results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.HolidayApply ), new [] { nameof( ApplyFor ) } ) );
              isValid = true;
              break;
            }
            else {
              listAfterValid.Add( obj );
              listUser.AddRange( team.TeamUsers.Where( t => t.User.IsActive ).Select( t => t.UserId ) );
            }
          }
          if ( !isValid ) {
            foreach ( var obj in ApplyFor.Where( h => h.Type == Enum.GetName( Enums.DynamicOption.User ) ).Distinct() ) {
              if ( listUser.Contains( obj.Id ) )
                continue;
              if ( !dbContext.Users.Any( u => u.Id == obj.Id && u.IsActive ) ) {
                results.Add( new ValidationResult( string.Format( ErrorMessageResource.UserNotFound, DisplayNameResource.HolidayApply ), new [] { nameof( ApplyFor ) } ) );
                isValid = true;
              }
              else {
                listAfterValid.Add( obj );
              }
            }
          }
          if ( !isValid ) {
            ApplyFor = listAfterValid.ToArray();
          }
        }
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class HolidayResponse
  {
    /// <summary>
    /// Holiday ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// Event namne
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// holiday/Special
    /// </summary>
    /// <example>Holiday</example>
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// danh sách apply
    /// </summary>
    /// <example>danh sách tên user</example>
    [Required]
    public string ApplyFor { get; set; } = string.Empty;
    /// <summary>
    /// Thời gian bắt đầu
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string StartDateText => StartDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// thời gian kết thúc
    /// </summary>
    [Required]
    public DateTime EndDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string EndDateText => EndDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// số ngày diễn ra
    /// </summary>
    /// <example>1</example>
    public double Duration { get; set; }
    /// <summary>
    /// trạng thái
    /// </summary>
    /// <example>Pending</example>
    [Required]
    public string Status { get; set; } = string.Empty;
  }
}
