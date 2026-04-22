
using AutoMapper;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class ConfigService : GenericService<Config>, IConfigService
  {
    private readonly IConfigRepository _configRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ConfigService> _logger;
    private readonly IProjectRepository _projectRepository;
    public ConfigService( IConfigRepository ConfigRepository, IMapper mapper, ILogger<ConfigService> logger, IProjectRepository projectRepository ) : base( ConfigRepository )
    {
      _configRepository = ConfigRepository;
      _mapper = mapper;
      _logger = logger;
      _projectRepository = projectRepository;
    }
    #region BaseServiceResource
    public async Task<long> Create( ConfigResource resource, long creator )
    {
      var config = _mapper.Map<Config>( resource );
      config.CreatedBy = creator;
      config.Type = resource.Type == "Day" ? "Day" : "ProjectType";
      await _configRepository.CreateAsync( config );
      return config.Id;
    }

    public async Task Update( ConfigResource resouce, long editor )
    {
      var old = await _configRepository.GetByCondition( c => c.Id == resouce.Id );
      var config = _mapper.Map( resouce, old );
      _configRepository.Update( config! );
    }

    #endregion

    #region BaseServiceList
    public async Task<IEnumerable<ConfigResponse>> GetList( BaseFilter filter )
    {
      var configs = await _configRepository.GetAll();
      return _mapper.Map<IEnumerable<ConfigResponse>>( configs );
    }

    #endregion

    #region Other
    public async Task<bool> CheckHadUsingBeforeDelete( long id )
    {
      var config = await _configRepository.GetByCondition( x => x.Id == id );
      switch ( config!.Type ) {
        case "ProjectType": {
            var projectTypeUsing = await _projectRepository.GetListByCondition( x => x.Type == config.Code );
            if ( !projectTypeUsing.Any() ) {
              config.IsUsing = true;
              _configRepository.Update( config );
              return false;
            }
            return true;
          }
        default:
          return false;
      }
    }
    public async Task<IEnumerable<ConfigResponse>> GetOption( string? type )
    {
      var configs = await _configRepository.GetListByCondition( x => x.Type!.ToLower() == type!.ToLower() );
      return _mapper.Map<IEnumerable<ConfigResponse>>( configs );
    }

    public async Task<string> GetTotalWorking( string? type = "Day" )
    {
      var month = DateTime.UtcNow.Month.ToString();
      var config = await _configRepository.GetByCondition( x => x.Type!.ToLower() == type!.ToLower() && x.Description == month );
      return config?.Code ?? "0";
    }
    #endregion
  }
}
