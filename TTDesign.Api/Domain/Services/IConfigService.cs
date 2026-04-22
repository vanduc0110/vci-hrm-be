using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services;

public interface IConfigService : IGenericService<Config>, BaseServiceResource<ConfigResource>, BaseServiceList<ConfigResponse>
{
  Task<bool> CheckHadUsingBeforeDelete( long id );
  Task<IEnumerable<ConfigResponse>> GetOption( string? type = "ProjectType" );
  Task<string> GetTotalWorking( string? type = "Day" );
}
