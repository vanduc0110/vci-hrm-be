using Microsoft.EntityFrameworkCore;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Extensions;
using TTDesign.API.Persistence.Contexts;
using TTDesign.API.Resources;

namespace TTDesign.API.Persistence.Repositories
{
  public class ProjectRepository : GenericRepository<Project>, IProjectRepository
  {
    public ProjectRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<Project?> GetProjectDocument( long projectId )
    {
      //return await _context.Projects.Include( p => p.ProjectDocuments ).Where( p => p.Id == projectId ).FirstOrDefaultAsync();
      return await _context.Projects.Include( x => x.Client ).Where( p => p.Id == projectId ).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProjectOption>> GetOptionWorking( long userId )
    {
      var projects = await _context.Users
          .Include( u => u.Projects )
          .AsNoTracking()
          .Where( u => u.Id == userId )
          .SelectMany( u => u.Projects )
          .Where( p => p.Status  )
          .ToListAsync();

      var projectOptions = new List<ProjectOption>();

      foreach ( var project in projects ) {
        var categoryProjects = await _context.TeamCategories
            .Where( x => x.TeamId == project.TeamId )
            .Select( x => new CategoryProject
            {
              ProjectId = project.Id,
              CategotyId = x.Id,
              CategoryName = x.Name
            } )
            .ToListAsync();

        projectOptions.Add( new ProjectOption
        {
          Id = project.Id,
          Code = project.Code,
          FullName = Common.FormatProjectName( project ),
          TeamId = project.TeamId,
          CategoryProjects = categoryProjects
        } );
      }

      return projectOptions;
    }

    public async Task<IEnumerable<ProjectOption>> GetOption( long userId, long? teamId = null )
    {
      return await _context.Projects.AsNoTracking().Where( p => teamId == null || p.TeamId == teamId || EF.Functions.Like( p.ProjectManagement, $"%{userId}%" ) ).Select( p =>
      new ProjectOption()
      {
        Id = p.Id,
        Code = p.Code,
        FullName = Common.FormatProjectName( p )
      } ).ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetProjects( long userId, int position, long? teamId = null )
    {
      return await _context.Projects.Include( p => p.Client ).Include( p => p.Users ).ThenInclude( x => x.TeamUsers ).ThenInclude( x => x.Team ).Include( x => x.ProjectContracts )
        .Where( p => teamId == null || p.TeamId == teamId || ( ( position == ( int ) Enums.UserPosition.PM || position == ( int ) Enums.UserPosition.SubLead || position == ( int ) Enums.UserPosition.TeamLead ) && EF.Functions.Like( p.ProjectManagement, $"%{userId}%" ) ) ).ToListAsync();
    }

    public async Task<Project?> GetProjectDataById( long id )
    {
      return await _context.Projects.Include( p => p.Client ).Include( p => p.Users ).ThenInclude( p => p.TeamUsers ).ThenInclude( x => x.Team ).Where( p => p.Id == id ).FirstOrDefaultAsync();
    }

    public async Task UpdateMemberAsync( long id, List<User> member, bool? isUpdate = true )
    {
      var project = await _context.Projects.Include( g => g.Users ).Where( g => g.Id == id ).FirstAsync();
      if ( isUpdate is not null && !isUpdate.Value ) {
        var user = member.FirstOrDefault();
        project.Users.Remove( user! );
      }
      else {
        project.Users.Clear();
        foreach ( var user in member ) {
          project.Users.Add( user );
        }
      }

      _context.SaveChanges();
    }

    public async Task DeleteAsync( long id )
    {
      var project = await _context.Projects.Include( g => g.Users ).Where( g => g.Id == id ).FirstAsync();
      foreach ( var user in project.Users )
        project.Users.Remove( user );
      _context.SaveChanges();
      project = await _context.Projects.Where( g => g.Id == id ).FirstAsync();
      _context.Projects.Remove( project );
      _context.SaveChanges();
    }

    public async Task<Project?> GetLastProject()
    {
      return await _context.Projects.OrderBy( p => p.Id ).LastOrDefaultAsync();
    }

    public async Task<List<long>> GetMemberOfProjects( long projectManager )
    {
      return await _context.Projects.Include( p => p.Users ).Where( p => p.Status )
        .SelectMany( p => p.Users ).Select( u => u.Id ).Distinct().ToListAsync();
    }
  }
}
