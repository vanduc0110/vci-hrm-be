using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  public class AssetResource : IValidatableObject
  {
    public long? Id { get; set; }
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Name { get; set; } = string.Empty;
    public string AssetCode { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? PurchaseDate { get; set; }

    [MaxLength( 100 )]
    public string? Supplier { get; set; }
    public string Condition { get; set; } = string.Empty;

    [MaxLength( 500 )]
    public string? Notes { get; set; }
    [Required]
    public long AssetCategoryId { get; set; }
    [MaxLength( 100 )]
    public string? Location { get; set; } = string.Empty;
    public int? Quantity { get; set; } = 1;
    public string Status { get; set; } = string.Empty;
    public string? DetailedSpecs { get; set; }
    public List<AssetToComponent>? Component { get; set; } = new List<AssetToComponent>();
    public AllocateAssetResource? User { get; set; } = new AllocateAssetResource();

    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {

      if ( !string.IsNullOrEmpty( Name ) ) {
        Name = Common.RemoveMultiBlank( Name );
      }

      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.Assets.Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( dbContext.Assets.Any( t => t.Id != Id && t.AssetCode == AssetCode ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.AssetCode ), new [] { nameof( AssetCode ) } ) );
        }
        if ( Id > 0 && ( string.IsNullOrEmpty( Status ) || !Enum.IsDefined( typeof( Enums.AssetStatus ), Status ) ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.AssetStatus ), new [] { nameof( Status ) } ) );
        }
        if ( !dbContext.AssetCategories.Any( t => t.Id == AssetCategoryId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.AssetCategory ), new [] { nameof( AssetCategoryId ) } ) );
        }
        if ( DetailedSpecs != null ) {
          var assetCategory = dbContext.AssetCategories.FirstOrDefault( t => t.Id == AssetCategoryId );
          if ( assetCategory!.Name == "Computer" ) {
            try {
              var specs = JsonConvert.DeserializeObject<ComputerSpecs>( DetailedSpecs );
            }
            catch {
              results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.AssetComputerSpecs ), new [] { nameof( DetailedSpecs ) } ) );
            }
          }
          else if ( assetCategory.Name == "Monitor" ) {
            try {
              var specs = JsonConvert.DeserializeObject<MonitorSpecs>( DetailedSpecs );
            }
            catch {
              results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.AssetMonitorSpecs ), new [] { nameof( DetailedSpecs ) } ) );
            }
          }
          else if ( assetCategory.Name == "Component" ) {
            try {
              var specs = JsonConvert.DeserializeObject<ComponentSpecs>( DetailedSpecs );
            }
            catch {
              results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.AssetComponentSpecs ), new [] { nameof( DetailedSpecs ) } ) );
            }
          }

        }
      }
      return results;

    }
  }
  public class AssetResponse
  {
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long AssetCategoryId { get; set; }
    public string? Brand { get; set; }
    public string? AssetCode { get; set; } = string.Empty;
    public string AssetCategoryName { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public decimal? PurchasePrice { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public string? Supplier { get; set; }
    public string Condition { get; set; } = string.Empty;
    [MaxLength( 500 )]
    public string? Notes { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AssignmentUser { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public DateTime? AssignmentDate { get; set; }
    public dynamic? DetailedSpecs { get; set; }
    public AssetCategoryResponse AssetCategory { get; set; }
  }

  public class ComputerSpecs : AssetDetailResponse
  {
    public string Type { get; set; } = string.Empty;
    public string CPU { get; set; } = string.Empty;
    public string RAM { get; set; } = string.Empty;
    public string SSD { get; set; } = string.Empty;
    public string GPU { get; set; } = string.Empty;
  }
  public class MonitorSpecs : AssetDetailResponse
  {
    public string? ScreenSize { get; set; }
  }

  public class ComponentSpecs : AssetDetailResponse
  {
    public string ComponentType { get; set; } = string.Empty;
    public DateTime? ReplacementOrStorageDate { get; set; }

  }
  public class AssetDetailResponse
  {
    public AssetDetailResponse()
    {
      AssetCategory = new AssetCategoryResponse();
    }
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? AssetCode { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Location { get; set; }
    public string? SerialNumber { get; set; }
    public decimal? PurchasePrice { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public string? Supplier { get; set; }
    public string Condition { get; set; } = string.Empty;
    [MaxLength( 500 )]
    public string? Notes { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AssignmentUser { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public DateTime? AssignmentDate { get; set; }
    public dynamic? DetailedSpecs { get; set; }
    public AssetCategoryResponse AssetCategory { get; set; }
    public IEnumerable<AssetAllocationResponse> Allocations { get; set; } = new List<AssetAllocationResponse>();
    public IEnumerable<AssetComponentHistory> CompHistory { get; set; } = new List<AssetComponentHistory>();
  }
  public class ComponentToAsset
  {
    public List<AssetToComponent> ComponentIds { get; set; } = new List<AssetToComponent>();
  }
  public class AssetToComponent
  {
    public long AssetId { get; set; }
    public DateTime ChangeDate { get; set; }
    public string? Note { get; set; } = null!;
  }

  public class AssetStorageResponse
  {
    public long Id { get; set; }
    public string AssetCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string Condition { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? Supplier { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? StorageDate => PurchaseDate;
    public AssetCategoryResponse AssetCategory { get; set; } = null!;
  }
  public class AssetStorageOption
  {
    public long Id { get; set; }
    public string AssetCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public AssetCategoryResponse AssetCategory { get; set; } = null!;
  }

  public class AssetImportReponse
  {
    public int Failes { get; set; }
    public int Success { get; set; }
    public AssetImportReponse()
    {
      Failes = 0;
      Success = 0;
    }
  }
}

