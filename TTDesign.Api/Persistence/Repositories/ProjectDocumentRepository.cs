using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class ProjectDocumentRepository : GenericRepository<ProjectDocument>, IProjectDocumentRepository
  {
    public ProjectDocumentRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
