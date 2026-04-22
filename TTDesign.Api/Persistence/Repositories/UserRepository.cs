using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;
using TTDesign.API.Resources;

namespace TTDesign.API.Persistence.Repositories
{
  public class UserRepository : GenericRepository<User>, IUserRepository
  {
    public UserRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<IEnumerable<UserOption>> GetOption()
    {
      return await _context.Users.Include( u => u.TeamUsers ).ThenInclude( x => x.Team )
        .Where( u => u.IsActive )
        .Select( t => new UserOption()
        {
          Id = t.Id,
          Name = t.UserName,
          FullName = t.FullName,
          Position = Enum.GetName( ( Enums.UserPosition ) t.Position )!,
          Avatar = t.Avatar,
          State = Enum.GetName( ( Enums.UserState ) t.State )!,
          TeamIds = t.TeamUsers != null && t.TeamUsers.Count() > 0 ? t.TeamUsers.Select( tu => tu.TeamId ).ToArray() : Array.Empty<long>(),
          Teams = ( t.TeamUsers != null && t.TeamUsers.Count() > 0 ) ? t.TeamUsers.Select( tu => new TeamUserOption()
          {
            TeamCode = tu.Team.Code,
            TeamName = tu.Team.Name
          } ).ToList() : new List<TeamUserOption>()
        } ).ToListAsync();
    }

    public async Task<User?> GetUserDataByCondition( Expression<Func<User, bool>> predicate )
    {
      return await _context.Users.Include( x => x.TeamUsers ).ThenInclude( x => x.Team ).AsNoTracking().FirstOrDefaultAsync( predicate );
    }

    public async Task<User?> FindLastUserHasName( string userName )
    {
      return await _context.Users.Where( u => u.UserName.StartsWith( userName ) ).OrderByDescending( u => u.Id ).AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetUsersDataByCondition( long? teamId = null )
    {
      return await _context.Users.Include( x => x.TeamUsers ).ThenInclude( x => x.Team )
          .Where( u => teamId == null || ( teamId.HasValue && u.TeamUsers.Any( x => x.TeamId == teamId.Value ) ) )
          .AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<User>> GetLeaders()
    {
      return await _context.Users.Include( u => u.TeamUsers ).Where( u => u.IsActive  && u.Position == ( int ) Enums.UserPosition.TeamLead ).AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<DashboardUser>> GetDashboardUser()
    {
      return await _context.Users.Where( u => u.Id != Enums.SYSTEM_ID && u.IsActive )
        .Select( u => new DashboardUser()
        {
          Id = u.Id,
          Name = u.FullName,
          Avatar = u.Avatar,
          State = Enum.GetName( ( Enums.UserState ) u.State )!
        } ).OrderBy( u => u.Name ).ToListAsync();
    }

    public async Task<Dictionary<string, List<User>>> GetUserWithLeader( long id )
    {
      var result = new Dictionary<string, List<User>>();
      var userInfo = await _context.Users.Include( u => u.TeamUsers ).Where( u => u.Id == id ).FirstAsync();
      var approvalPositions = new List<int>
      {
          (int)Enums.UserPosition.Director,
          (int)Enums.UserPosition.TeamLead,
          (int)Enums.UserPosition.SubLead
      };
      int userPositionIndex = approvalPositions.IndexOf( userInfo.Position );

      List<int> leaderPositions;

      if ( userPositionIndex >= 0 ) {
        var leaderPosition = approvalPositions.ElementAtOrDefault( userPositionIndex - 1 );
        leaderPositions = leaderPosition != default ? new List<int> { leaderPosition } : new List<int>();
      }
      else {
        leaderPositions = approvalPositions;
      }
      var userTeamIds = userInfo.TeamUsers.Select( tu => tu.TeamId ).ToList();

      var leaders = await _context.Users
          .Include( u => u.TeamUsers )
          .Where( u =>
              u.IsActive &&
              leaderPositions.Contains( u.Position )
          )
          .ToListAsync();
      if ( userInfo.Position != ( int ) Enums.UserPosition.TeamLead ) {
        leaders = leaders.Where( l => l.TeamUsers.Any( tu => userTeamIds.Contains( tu.TeamId ) ) ).ToList();
      }
      return new Dictionary<string, List<User>>()
            {
                { "User", new List<User> { userInfo } },
                { "Leader", leaders }
            };
    }

    public async Task<User?> GetUserDataByConditionNoTracking( Expression<Func<User, bool>> predicate )
    {
      return await _context.Users.Include( x => x.TeamUsers ).ThenInclude( x => x.Team ).FirstOrDefaultAsync( predicate );
    }
  }
}
