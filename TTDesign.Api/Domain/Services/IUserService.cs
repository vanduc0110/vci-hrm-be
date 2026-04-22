using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IUserService : IGenericService<User>,
    BaseServiceResource<UserResource>, BaseServiceList<UserResponse>, BaseServiceOption<UserOption>, BaseServiceDetail<UserDetailResponse>
  {
    /// <summary>
    /// update user setting
    /// </summary>
    /// <param name="resource"></param>
    Task UpdateSetting( UserSettingResource resource );
    Task<UserSettingResponse> GetSetting( long id );
    /// <summary>
    /// Thay đổi password
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="modifier"></param>
    /// <returns>true: thành công</returns>
    Task<bool> ChangePassword( UserAndPassResource resource, long modifier );
    /// <summary>
    /// reset password
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="modifier"></param>
    /// <returns></returns>
    Task<string> ResetPassword( long userId, long modifier );
    /// <summary>
    /// chuyển trạng thái user
    /// </summary>
    /// <remarks>user active => inactive, ngược lại inactive => active</remarks>
    /// <param name="userId"></param>
    /// <param name="modifier"></param>
    /// <returns></returns>
    Task<bool> ChangeStatus( long userId, long modifier );
    Task<Role> GetRole( string roleName );
    Task<bool> UpdateAvatar( long id, string fileName );
    Task<bool> ChangeState( long id, int state );
    /// <summary>
    /// lấy danh sách lựa chọn gồm user/team theo teamId
    /// </summary>
    /// <param name="teamId">0: tất cả, phục vụ admin; còn >0 là phạm vi team id đó</param>
    /// <returns></returns>
    Task<List<DynamicOption>> GetDynamicOption(long? teamId);
    /// <summary>
    /// Lấy thông tin khởi tạo dashboard: users
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<DashboardUser>> GetDashboardUser();
    /// <summary>
    /// lấy thông tin user từ request view user staff
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<DetailOtherUser?> GetDetailOtherUser( long id );
    /// <summary>
    /// request reset password to admin/leader
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task RequestResetPassword( User user );
    /// <summary>
    /// tự cập nhật thông tin bản thân
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    Task SelfUpdate( YourSelfResource resource );
    /// <summary>
    /// lấy danh sách user theo position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    Task<IEnumerable<UserOption>> GetUserByPosition( long position );
  }
}
