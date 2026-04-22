using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TTDesign.API.Constants;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  [Index( nameof( ContractCode ), IsUnique = true, Name = "Ix_ProjectCode" )]
  public class ProjectContractResource : IValidatableObject
  {
    public long? Id { get; set; }
    public string ContractName { get; set; } = null!;
    public string ContractCode { get; set; } = null!;
    /// <summary>
    /// Contract Date [input]
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    public DateTime ContractDate { get; set; }
    public IFormFileCollection? Document { get; set; }

    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      if ( !string.IsNullOrEmpty( ContractCode ) ) {
        ContractCode = Common.FormatCodeInput( ContractCode );
      }
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.ProjectContracts.Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.ProjectContract ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( dbContext.ProjectContracts.Any( t => t.Id != Id && t.Code == ContractCode ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.ProjectContractCode ), new [] { nameof( ContractCode ) } ) );
        }
      }
      return results;
    }
  }

  public class ProjectContractResponse
  {
    public long? Id { get; set; }
    public string ContractName { get; set; } = null!;
    public string ContractCode { get; set; } = null!;
    public DateTime ContractDate { get; set; }
    public string CreatedDateText => ContractDate.ToString( Enums.DATE_FORMAT );
    public ProjectDocumentDetail []? Docs { get; set; }
  }
}