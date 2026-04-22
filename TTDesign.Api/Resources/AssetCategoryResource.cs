using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TTDesign.API.Constants;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  public class AssetCategoryResource : IValidatableObject
  {
    public long? Id { get; set; }
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "AssetCategoryName" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Name { get; set; } = null!;
    public string? Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long? ParentId { get; set; }
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.Clients.AsNoTracking().Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.AssetCategory ), new [] { Enums.ERROR_TEXT } ) );
      }
      return results;
    }
  }

  public class AssetCategoryResponse
  {
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Quanity { get; set; } = 0;
    public List<AssetCategoryResponse> Children { get; set; } = new List<AssetCategoryResponse>();
  }
}
