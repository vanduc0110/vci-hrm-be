using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Seeds
{
  public static class DefaultProjectType
  {
    public static async Task SeedDefaultProjectType( AppDbContext context )
    {
      if ( !context.Configs.Any() ) {
        var configs = new List<Domain.Models.Config>
        {
          new Domain.Models.Config
          {
            Id = 1,
            Name = "Internal",
            Code = "I",
            Description = "",
            Type = "ProjectType",
            IsUsing = false
          },
          new Domain.Models.Config
          {
            Id = 2,
            Name = "Outsource",
            Code = "O",
            Description = "",
            Type = "ProjectType",
            IsUsing = false
          },
          new Domain.Models.Config
          {
            Id = 3,
            Name = "D",
            Code = "D",
            Description = "",
            Type = "ProjectType",
            IsUsing = false
          },
          new Domain.Models.Config
          {
            Id = 4,
            Name = "Product",
            Code = "P",
            Description = "",
            Type = "ProjectType",
            IsUsing = false
          }
        };
        await context.Configs.AddRangeAsync( configs );
        await context.SaveChangesAsync();
      }
    }
  }
}
