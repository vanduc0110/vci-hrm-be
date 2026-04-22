using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface ITeamService : IGenericService<Team>, BaseServiceResource<TeamResource>, BaseServiceOption<TeamOption>, BaseServiceList<TeamResponse>
  {
    /// <summary>
    /// Get TeamUser bằng User ID
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<TeamUser>?> GetTeamUserByUserId( long userId );
    /// <summary>
    /// Valid Team trước khi tiến hành xóa
    /// </summary>
    /// <param name="id"></param>
    /// <returns>false: không được xóa</returns>
    Task<bool> CheckTeamBeforeDelete( long id );
    /// <summary>
    /// Lấy danh sách user active thuộc team
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IEnumerable<User>> GetTeamUser( long id );
  }
}
