using AutoMapper;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class TeamService : GenericService<Team>, ITeamService
  {
    private readonly ITeamRepository _teamRepository;
    private readonly ILogger<TeamService> _logger;
    private readonly ITeamUserRepository _teamUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public TeamService( ITeamRepository teamRepository,
      ILogger<TeamService> logger,
      IMapper mapper,
    ITeamUserRepository teamUserRepository,
      IUserRepository userRepository ) : base( teamRepository )
    {
      _teamRepository = teamRepository;
      _logger = logger;
      _mapper = mapper;
      _teamUserRepository = teamUserRepository;
      _userRepository = userRepository;
    }

    #region BaseServiceResource
    public async Task<long> Create( TeamResource resource, long creator )
    {
      var team = _mapper.Map<Team>( resource );
      team.CreatedBy = creator;
      await _teamRepository.CreateAsync( team );
      return team.Id;
    }

    public async Task Update( TeamResource resouce, long modifier )
    {
      var old = await _teamRepository.GetByCondition( t => t.Id == resouce.Id );
      old!.Name = resouce.Name;
      old.Code = resouce.Code;
      old.ModifiedBy = modifier;
      _teamRepository.Update( old );
    }
    #endregion

    #region BaseServiceOption
    public async Task<IEnumerable<TeamOption>> GetOption( long? teamId = null )
    {
      return await _teamRepository.GetOption( teamId );
    }
    #endregion

    #region BaseServiceList
    public async Task<IEnumerable<TeamResponse>> GetList( BaseFilter filter )
    {
      var teams = await _teamRepository.GetAll();
      var leaders = await _userRepository.GetLeaders();
      var teamResponse = _mapper.Map<IEnumerable<TeamResponse>>( teams );
      foreach ( var team in teamResponse ) {
        var leader = leaders.SelectMany( u => u.TeamUsers.Where( x => x.TeamId == team.Id ) );
        if ( leader != null ) {
          team.Leader = leader.Select( x => x.User.FullName ).ToArray();
        }
        var user = await _teamRepository.GetTeamUser( team.Id );
        team.Users = user.Select( u => new UserResponse()
        {
          Id = u.Id,
          StaffId = u.StaffId,
          UserName = u.UserName,
          FullName = u.FullName,
          Email = u.Email,
          Position = Enum.GetName( typeof( Enums.UserPosition ), u.Position ) ?? string.Empty,
          DateStartWork = ( DateTime ) u.DateStartWork!

        } ).ToArray();

      }
      return teamResponse;
    }
    #endregion

    #region Others
    public async Task<List<TeamUser>?> GetTeamUserByUserId( long userId )
    {
      var rs = await _teamUserRepository.GetListByConditionTrack( tu => tu.UserId == userId );
      return rs?.ToList();
    }

    public async Task<bool> CheckTeamBeforeDelete( long id )
    {
      return !await _teamUserRepository.Exist( t => t.TeamId == id );
    }

    public async Task<IEnumerable<User>> GetTeamUser( long id )
    {
      return await _teamRepository.GetTeamUser( id );
    }
    #endregion
  }
}
