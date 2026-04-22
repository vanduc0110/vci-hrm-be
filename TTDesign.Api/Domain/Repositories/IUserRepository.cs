using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Repositories
{
  public interface IUserRepository : IGenericRepository<User>
  {
    Task<User?> GetUserDataByCondition( Expression<Func<User, bool>> predicate );
    Task<User?> GetUserDataByConditionNoTracking( Expression<Func<User, bool>> predicate );
    /// <summary>
    /// Kiểm tra UserName đã tồn tại chưa
    /// </summary>
    /// <param name="userName"></param>
    /// <returns>null: nếu chưa tồn tại, hoặc user có No lớn nhất</returns>
    Task<User?> FindLastUserHasName( string userName );
    /// <summary>
    /// Danh sách User, bao gồm TeamUser, Team
    /// </summary>
    /// <remarks>Danh sách phụ thuộc Team ID</remarks>
    /// <param name="teamId"></param>
    /// <returns></returns>
    Task<IEnumerable<User>> GetUsersDataByCondition( long? teamId = null );
    Task<IEnumerable<User>> GetLeaders();
    Task<IEnumerable<UserOption>> GetOption();
    /// <summary>
    /// danh sachs user kèm state
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<DashboardUser>> GetDashboardUser();
    /// <summary>
    /// lấy thông tin user kèm danh sách leader quản lý
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Dictionary<string, List<User>>> GetUserWithLeader( long id );
  }
}
