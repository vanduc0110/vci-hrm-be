using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services;

public interface IClientService : IGenericService<Client>, BaseServiceResource<ClientResource>, BaseServiceList<ClientResponse>
{
  /// <summary>
  /// Valid Team trước khi tiến hành xóa
  /// </summary>
  /// <param name="id"></param>
  /// <returns>false: không được xóa</returns>
  Task<bool> CheckClientBeforeDelete( long id );
}
