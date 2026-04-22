using Microsoft.EntityFrameworkCore;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;
using TTDesign.API.Resources;

namespace TTDesign.API.Persistence.Repositories
{
  public class TeamRepository : GenericRepository<Team>, ITeamRepository
  {
    public TeamRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<IEnumerable<TeamOption>> GetOption( long? teamId = null )
    {
      return await _context.Teams.Where( t => teamId == null || t.Id == teamId ).Select( t => new TeamOption()
      {
        Id = t.Id,
        Code = t.Code,
        Name = t.Name
      } ).ToListAsync();
    }

    public void UpdateAmountOfTeam()
    {
      var teams = _context.Teams.ToList();
      var teamUserCounts = _context.Users
          .Where( u => u.IsActive  )
          .Join( _context.TeamUsers,
                u => u.Id,
                tu => tu.UserId,
                ( u, tu ) => new { tu.TeamId } )
          .GroupBy( x => x.TeamId )
          .Select( g => new { TeamId = g.Key, Amount = g.Count() } )
          .ToList();

      foreach ( var team in teams ) {
        var teamCount = teamUserCounts.FirstOrDefault( tc => tc.TeamId == team.Id );
        team.Amount = teamCount != null ? teamCount.Amount : 0;
        _context.SaveChanges();
      }
    }

    public async Task<IEnumerable<User>> GetTeamUser( long id )
    {
      return await _context.TeamUsers.Include( t => t.User ).Where( t => t.TeamId == id && t.User == null || !t.User.IsActive )
        .Select( t => t.User ).ToListAsync();
    }
  }
}
