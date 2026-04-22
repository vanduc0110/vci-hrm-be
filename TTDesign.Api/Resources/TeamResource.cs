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
  [Index( nameof( Code ), IsUnique = true, Name = "Ix_TeamCode" )]
  public class TeamResource : IValidatableObject
  {
    /// <summary>
    /// Team ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// Team Name [input] [format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "TeamName" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Team Code [input, format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "TeamCode" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 30, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Code { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      if ( !string.IsNullOrEmpty( Code ) ) {
        Code = Common.FormatCodeInput( Code );
      }
      if ( !string.IsNullOrEmpty( Name ) ) {
        Name = Common.RemoveMultiBlank( Name );
      }
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.Teams.AsNoTracking().Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Team ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( dbContext.Teams.AsNoTracking().Any( t => t.Id != Id && t.Code == Code ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.TeamCode ), new [] { nameof( Code ) } ) );
        }
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class TeamResponse
  {
    /// <summary>
    /// Team ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// Team Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Team Code
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// Số member trong Team
    /// </summary>
    public long Amount { get; set; }
    /// <summary>
    /// Leader, null value nếu Team chưa có Leader
    /// </summary>
    /// <example></example>
    public string [] Leader { get; set; } = Array.Empty<string>();
    /// <summary>
    /// Ngày tạo Team
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string CreatedDateText => CreatedDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// Danh sách member trong team
    /// </summary>
    public UserResponse []? Users { get; set; } = null;
  }

  /// <summary>
  /// Class cho model output api [Option]
  /// </summary>
  public class TeamOption
  {
    /// <summary>
    /// Team ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Team Code
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// Team Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
  }
}
