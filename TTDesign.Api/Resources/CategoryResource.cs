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
  [Index( nameof( Name ), Name = "Ix_CategoryName" )]
  public class CategoryResource : IValidatableObject
  {
    /// <summary>
    /// Team ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// Category Name [input] [format]
    /// </summary>
    /// <example>tên category</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "CategoryName" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Category thuộc Team
    /// </summary>
    public long TeamId { get; set; }
    /// <summary>
    /// Description [input] [format]
    /// </summary>
    public string? Description { get; set; } = null!;
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
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.TeamCategories.Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Category ), new [] { Enums.ERROR_TEXT } ) );
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class CategoryResponse
  {
    /// <summary>
    /// Category ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Category Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Team ID
    /// </summary>
    public long TeamId { get; set; }
    /// <summary>
    /// Thuộc Team Code
    /// </summary>
    /// <example>code team: chữ hoa, không có space</example>
    [Required]
    public string TeamCode { get; set; } = string.Empty;
    /// <summary>
    /// Thuộc Team Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string TeamName { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
  }

  /// <summary>
  /// Class cho model output api [Option]
  /// </summary>
  public class CategoryOption
  {
    /// <summary>
    /// Category ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Category Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
  }
  public class CategoryProject
  {
    public long ProjectId { get; set; }
    public long CategotyId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
  }
}
