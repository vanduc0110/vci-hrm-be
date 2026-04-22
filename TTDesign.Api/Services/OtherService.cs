using AutoMapper;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;

namespace TTDesign.API.Services
{
  public class OtherService : IOtherService
  {
    private readonly ILogger<OtherService> _logger;
    private readonly IMapper _mapper;
    private readonly IClientRepository _clientRepository;
    private readonly IProjectRepository _projectRepository;

    public OtherService( IClientRepository clientRepository,
      IProjectRepository projectRepository,
      ILogger<OtherService> logger,
      IMapper mapper )
    {
      _logger = logger;
      _mapper = mapper;
      _clientRepository = clientRepository;
      _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<Client>> GetClients()
    {
      return await _clientRepository.GetAll();
    }

    public async Task<int> GetNewProjectNumber()
    {
      var lastProject = await _projectRepository.GetLastProject();
      return lastProject == null ? 1 : lastProject.ProjectNumber + 1;
    }
  }
}
