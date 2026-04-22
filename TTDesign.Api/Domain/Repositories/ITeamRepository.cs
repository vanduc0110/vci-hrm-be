using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Repositories
{
  public interface ITeamRepository : IGenericRepository<Team>
  {
    /// <summary>
    /// Lấy danh sách thu gọn Team
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    Task<IEnumerable<TeamOption>> GetOption( long? teamId = null );
    /// <summary>
    /// Cập nhật thông tin: số lượng member trong 1 team
    /// </summary>
    /// <remarks>chạy script</remarks>
    void UpdateAmountOfTeam();
    /// <summary>
    /// Lấy danh sách user active thuộc team
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IEnumerable<User>> GetTeamUser( long id );
  }
}
