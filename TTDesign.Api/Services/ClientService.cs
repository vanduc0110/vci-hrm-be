
using AutoMapper;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class ClientService : GenericService<Client>, IClientService
  {
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ClientService> _logger;
    private readonly IProjectRepository _projectRepository;
    public ClientService( IClientRepository clientRepository, IMapper mapper, ILogger<ClientService> logger, IProjectRepository projectRepository ) : base( clientRepository )
    {
      _clientRepository = clientRepository;
      _mapper = mapper;
      _logger = logger;
      _projectRepository = projectRepository;
    }
    #region BaseServiceResource
    public async Task<long> Create( ClientResource resource, long creator )
    {
      var client = _mapper.Map<Client>( resource );
      client.Code = Common.GeneratedCode( client.Name );
      client.CreatedBy = creator;
      await _clientRepository.CreateAsync( client );
      return client.Id;
    }

    public async Task Update( ClientResource resouce, long editor )
    {
      var client = await _clientRepository.GetByCondition( c => c.Id == resouce.Id );
      client!.Name = resouce.Name;
      client.Code = Common.GeneratedCode( client.Name );
      _clientRepository.Update( client );
    }

    #endregion

    #region BaseServiceList
    public async Task<IEnumerable<ClientResponse>> GetList( BaseFilter filter )
    {
      var clients = await _clientRepository.GetAll();
      return _mapper.Map<IEnumerable<ClientResponse>>( clients );
    }

    public async Task<bool> CheckClientBeforeDelete( long id )
    {
      return !await _projectRepository.Exist( p => p.ClientId == id );
    }
    #endregion
  }
}
