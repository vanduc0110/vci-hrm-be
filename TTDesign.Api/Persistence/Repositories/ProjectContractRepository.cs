using Microsoft.EntityFrameworkCore;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class ProjectContractRepository : GenericRepository<ProjectContract>, IProjectContractRepository
  {
    public ProjectContractRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<List<ProjectContract>?> GetListDetail( long? id )
    {
      return await _context.ProjectContracts.Include( t => t.ProjectDocuments ).Where( x => x.ProjectId == id ).ToListAsync();
    }
  }
}
