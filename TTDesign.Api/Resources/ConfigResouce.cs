using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TTDesign.API.Constants;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  public class ConfigResource : IValidatableObject
  {
    /// <summary>
    /// Config ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// Config Name [input] [format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ConfigName" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Config Code [input, format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ConfigCode" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 30, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Code { get; set; } = null!;
    /// <summary>
    /// Decription [Input]
    /// </summary>
    public string? Description { get; set; } = null!;
    /// <summary>
    /// Type
    /// </summary>
    public string? Type { get; set; } = null!;
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
      if ( dbContext is null || ( Id is not null && !dbContext.Configs.AsNoTracking().Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Config ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( dbContext.Configs.AsNoTracking().Any( t => t.Id != Id && t.Code == Code ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.ConfigCode ), new [] { nameof( Code ) } ) );
        }
      }
      return results;
    }
  }

  public class ConfigResponse
  {
    /// <summary>
    /// Config ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// Config Name
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Config Code
    /// </summary>
    [Required]
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// Config Description
    /// </summary>
    public string? Description { get; set; } = string.Empty;
    /// <summary>
    /// Kiểm tra đã được sử dụng hay chưa
    /// </summary>
    public bool? IsUsing { get; set; }
    /// <summary>
    /// Ngày tạo Config
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>format: yyyy/MM/dd</example>
    public string CreatedDateText => CreatedDate.ToString( Enums.DATE_FORMAT );
  }

  public class ConfigOption
  {
    public string? Type { get; set; } = "ProjectType";
  }
}
