using TTDesign.API.Domain.Models;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Seeds
{
  public class DefaultAssetCategory
  {
    public static async Task SeedDefaultAssetCategory( AppDbContext context )
    {
      if ( !context.AssetCategories.Any() ) {
        var assets = new List<Domain.Models.AssetCategory>
        {
          new AssetCategory { Id = 1, Name = "IT Equipment", Code = "IT", Level = 1, CreatedBy = 1},
          new AssetCategory { Id = 2, Name = "Electronic", Code = "EL", Level = 1, CreatedBy = 1 },
          new AssetCategory { Id = 3, Name = "Furniture", Code = "FU", Level = 1, CreatedBy = 1 },
          new AssetCategory { Id = 4, Name = "Other", Code = "OT", Level = 1, CreatedBy = 1 },
          new AssetCategory { Id = 5, Name = "Computer", Code = "PC", Level = 2, ParentId = 1, CreatedBy = 1 },
          new AssetCategory { Id = 6, Name = "Screen", Code = "SC", Level = 2, ParentId = 1, CreatedBy = 1 },
          new AssetCategory { Id = 7, Name = "Accessory", Code = "AC", Level = 2, ParentId = 1, CreatedBy = 1 },

        };
        await context.AssetCategories.AddRangeAsync( assets );
        await context.SaveChangesAsync();
      }
    }
  }
}
