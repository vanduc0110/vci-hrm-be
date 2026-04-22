using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TTDesign.API.Constants;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  public class ClientResource : IValidatableObject
  {
    /// <summary>
    /// Client ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// Client Name [input] [format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ClientName" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Client Code [input, format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ClientCode" )]
    [StringLength( 30, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? Code { get; set; } = null!;
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
      if ( dbContext is null || ( Id is not null && !dbContext.Clients.AsNoTracking().Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Client ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( dbContext.Clients.AsNoTracking().Any( t => t.Id != Id && t.Code == Code ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.ClientCode ), new [] { nameof( Code ) } ) );
        }
      }
      return results;
    }
  }

  public class ClientResponse
  {
    /// <summary>
    /// Client ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// Client Name
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Client Code
    /// </summary>
    public string? Code { get; set; } = string.Empty;
    /// <summary>
    /// Ngày tạo client
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>format: yyyy/MM/dd</example>
    public string CreatedDateText => CreatedDate.ToString( Enums.DATE_FORMAT );
  }
}
