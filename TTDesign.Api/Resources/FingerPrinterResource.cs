using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TTDesign.API.Constants;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  /// <summary>
  /// Class cho model input api [Create]/[Update]
  /// </summary>
  public class FingerPrinterResource : IValidatableObject
  {
    /// <summary>
    /// FingerPrinter ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// Time In, format hh:mm [input]
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "FingerPrinterTimeIn" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public string TimeIn { get; set; } = null!;

    [JsonIgnore]
    public DateTime DateTimeIn { get; set; }

    /// <summary>
    /// Time Out, format hh:mm [input]
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "FingerPrinterTimeOut" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public string TimeOut { get; set; } = null!;

    [JsonIgnore]
    public DateTime DateTimeOut { get; set; }

    /// <summary>
    /// Note [input, format]
    /// </summary>
    /// <example>ghi chú</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "FingerPrinterNote" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Note { get; set; } = null!;

    /// <summary>
    /// Ngày, áp dung cho view create [input]
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "FingerPrinterDate" )]
    public DateTime? DateSelect { get; set; }
    /// <summary>
    /// Khai thông tin cho user nào, áp dụng cho view create [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "FingerPrinterUser" )]
    public long UserId { get; set; }
    /// <summary>
    /// team của user
    /// </summary>
    [JsonIgnore]
    public long [] TeamIds { get; set; } = new long [ 0 ];
    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      if ( !string.IsNullOrEmpty( Note ) ) {
        Note = Common.RemoveMultiBlank( Note );
      }
      if ( DateSelect is not null ) {
        DateSelect = ( ( DateTime ) DateSelect ).Date;
      }
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.FingerPrinters.Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.FingerPrinter ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( !dbContext.Users.Any( u => u.Id == UserId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UserNotFound, DisplayNameResource.FingerPrinterUser ), new [] { nameof( UserId ) } ) );
        }
        else {
          TeamIds = dbContext.Users.Include( u => u.TeamUsers ).Where( u => u.Id == UserId ).SelectMany( u => u.TeamUsers.Select( x => x.TeamId ) ).ToArray();
        }
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class FingerPrinterResponse
  {
    /// <summary>
    /// FingerPrinter ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// User ID
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// User Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// Date
    /// </summary>
    [Required]
    public DateTime Date { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string DateText => Date.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// Time In
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    [Required]
    public string? TimeIn { get; set; } = string.Empty;
    /// <summary>
    /// Time Out
    /// </summary>
    /// <example>định dạng: HH:mm</example>
    public string? TimeOut { get; set; } = string.Empty;
    /// <summary>
    /// Note
    /// </summary>
    /// <example>ghi chú</example>
    public string Note { get; set; } = string.Empty;
    /// <summary>
    /// Modified By
    /// </summary>
    public long ModifiedBy { get; set; }
    /// <summary>
    /// Modifier Name
    /// </summary>
    /// <example>text</example>
    public string ModifierName { get; set; } = string.Empty;
    /// <summary>
    /// Ngày sửa
    /// </summary>
    public DateTime ModifiedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string ModifiedDateText => ModifiedDate.ToString( Enums.DATE_FORMAT );
  }

  /// <summary>
  /// Class model điều kiện tìm kiếm
  /// </summary>
  public class FingerPrinterFilter
  {
    /// <summary>
    /// điều kiện tìm kiếm: ngày bắt đầu
    /// </summary>
    public DateTime Start { get; set; }
    /// <summary>
    /// điều kiện tìm kiếm: ngày kết thúc
    /// </summary>
    public DateTime End { get; set; }
    /// <summary>
    /// phạm vi team quản lý
    /// </summary>
    [JsonIgnore]
    public long? TeamId { get; set; }
  }

  public class FingerMachineFromMachine
  {
    public int STT { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan? TimeIn { get; set; }
    public TimeSpan? TimeOut { get; set; }
  }
}
