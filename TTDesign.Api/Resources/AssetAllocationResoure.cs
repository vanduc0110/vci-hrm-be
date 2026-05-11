using System.ComponentModel.DataAnnotations;
using TTDesign.API.Constants;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  public class AssetAllocationResponse
  {
    public AssetAllocationResponse()
    {
      CurrentUser = new DashboardUser();
      PreviousUser = new DashboardUser();
    }
    public long AssetId { get; set; }
    public string Name { get; set; }
    public long? OldUserId { get; set; }
    public long? CurrentUserId { get; set; }
    public string AssetCode { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string EventType { get; set; }
    public DashboardUser? CurrentUser { get; set; }
    public DashboardUser? PreviousUser { get; set; }
    public string? Note { get; set; }
    public long CreatedBy { get; set; }
    public string ModifiedName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

  }
  public class DisposeAssetResource : IValidatableObject
  {
    public long AssetId { get; set; }
    public DateTime? DisposeDate { get; set; }
    public string? Notes { get; set; }
    public int Quantity { get; set; } = 1;
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( !dbContext.Assets.Any( t => t.Id == AssetId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] { nameof( AssetId ) } ) );
        }
        if ( DisposeDate is null || DisposeDate > DateTime.UtcNow ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.InvalidError, DisplayNameResource.DisposeDate ), new [] { nameof( DisposeDate ) } ) );
        }
      }
      return results;
    }
  }
  public class DestroyAssetResource : IValidatableObject
  {
    public long AssetId { get; set; }
    public DateTime? DestroyDate { get; set; }
    public string? Notes { get; set; }
    public int Quantity { get; set; } = 1;
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( !dbContext.Assets.Any( t => t.Id == AssetId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] { nameof( AssetId ) } ) );
        }
        if ( DestroyDate is null || DestroyDate > DateTime.UtcNow ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.InvalidError, DisplayNameResource.DestroyDate ), new [] { nameof( DestroyDate ) } ) );
        }
      }
      return results;
    }
  }

  public class AllocateAssetResource : IValidatableObject
  {
    public List<long> AssetIds { get; set; } = new List<long>();
    public long UserId { get; set; }
    public DateTime? AllocationDate { get; set; }
    public string? Notes { get; set; }
    public int Quantity { get; set; } = 1;
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( dbContext.Assets.Any( t => AssetIds.Any(a => a == t.Id)  && t.Status != ( int ) Enums.AssetStatus.InStorage ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ErrorAllocateAssetStatus, DisplayNameResource.Asset ), new [] {"AssetId"  } ) );
        }
        if ( !dbContext.Assets.Any( t => AssetIds.Any( a => a == t.Id ) ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] {  "AssetId"  } ) );
        }
        if ( !dbContext.Users.Any( t => t.Id == UserId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ), new [] { nameof( UserId ) } ) );
        }
        if ( AllocationDate is null || AllocationDate > DateTime.UtcNow ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.InvalidError, DisplayNameResource.AllocationDate ), new [] { nameof( AllocationDate ) } ) );
        }
      }
      return results;
    }
  }
  public class ReturnAssetResource : IValidatableObject
  {
    public List<long> AssetIds { get; set; } = new List<long>();
    public long UserId { get; set; }
    public DateTime? AllocationDate { get; set; }
    public string? Notes { get; set; }
    public int Quantity { get; set; } = 1;
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( dbContext.Assets.Any( t => AssetIds.Any( a => a == t.Id ) && ( t.Status != ( int ) Enums.AssetStatus.InUse && t.Status != ( int ) Enums.AssetStatus.Null ) ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ErrorAllocateAssetStatus, DisplayNameResource.Asset ), new [] { "AssetId" } ) );
        }
        if ( !dbContext.Assets.Any( t => AssetIds.Any( a => a == t.Id ) ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] {  "AssetId"  } ) );
        }
        if ( !dbContext.Users.Any( t => t.Id == UserId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ), new [] { nameof( UserId ) } ) );
        }
        if ( AllocationDate is null || AllocationDate > DateTime.UtcNow ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.InvalidError, DisplayNameResource.AllocationDate ), new [] { nameof( AllocationDate ) } ) );
        }
      }
      return results;
    }
  }
  public class TransferAssetResource : IValidatableObject
  {
    public List<long> AssetIds { get; set; } = new List<long>();
    public long UserId { get; set; }
    public long NewUserId { get; set; }
    public DateTime? TransferDate { get; set; }
    public string? Notes { get; set; }
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( !dbContext.Assets.Any( t => AssetIds.Any(x => x == t.Id) )) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Asset ), new [] {  "AssetId"  } ) );
        }
        if ( !dbContext.Users.Any( t => t.Id == UserId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ), new [] { nameof( UserId ) } ) );
        }
        if ( TransferDate is null || TransferDate > DateTime.UtcNow ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.InvalidError, DisplayNameResource.TransferDate ), new [] { nameof( TransferDate ) } ) );
        }
        if ( UserId == NewUserId ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.InvalidError, DisplayNameResource.NewUser ), new [] { nameof( NewUserId ) } ) );
        }
      }
      return results;
    }


  }
}
