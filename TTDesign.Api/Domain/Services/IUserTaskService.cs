using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IUserTaskService : IGenericService<UserTask>, BaseServiceList<UserTaskResponse>, BaseServiceResource<UserTaskResource>
  {

    public Task<UserTaskResponse> GetCurrentTask( DateTime dateCheck, long id );
    public Task<bool> CheckUserTaskBeforeDelete( long id );
  }
}
