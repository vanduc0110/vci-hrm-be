using Microsoft.EntityFrameworkCore;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;
using TTDesign.API.Resources;

namespace TTDesign.API.Persistence.Repositories
{
  public class TeamCategoryRepository : GenericRepository<TeamCategory>, ITeamCategoryRepository
  {
    public TeamCategoryRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<IEnumerable<CategoryOption>> GetOption( long? teamId = null )
    {
      return await _context.TeamCategories.Where( t => teamId == null || t.TeamId == teamId ).Select( t => new CategoryOption()
      {
        Id = t.Id,
        Name = t.Name
      } ).OrderBy( c => c.Name ).ToListAsync();
    }
  }
}
