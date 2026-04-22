using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Repositories
{
  public interface IProjectRepository : IGenericRepository<Project>
  {
    /// <summary>
    /// lấy thông tin project, kèm theo danh sách file 
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    Task<Project?> GetProjectDocument( long projectId );
    /// <summary>
    /// danh sach project (hoạt động) user đang tham gia
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<ProjectOption>> GetOptionWorking( long userId );
    /// <summary>
    /// danh sách project theo team/theo PM
    /// </summary>
    /// <param name="teamId">null: tất cả</param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<ProjectOption>> GetOption( long userId, long? teamId = null );
    /// <summary>
    /// lấy danh sách Project theo Team hoặc user là project manager, kèm thông tin Client và Members
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="position"></param>
    /// <param name="teamId"></param>
    /// <returns></returns>
    Task<IEnumerable<Project>> GetProjects( long userId, int position, long? teamId = null );
    /// <summary>
    /// láy thông tin project kèm danh sách member
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Project?> GetProjectDataById( long id );
    /// <summary>
    /// lấy project mới nhất theo thứ tự id trong DB
    /// </summary>
    /// <returns></returns>
    Task<Project?> GetLastProject();
    /// <summary>
    /// xóa project, kèm theo xóa ProjectUser
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync( long id );
    /// <summary>
    /// update ProjectUser
    /// </summary>
    /// <param name="id"></param>
    /// <param name="member"></param>
    /// <param name="isUpdate"></param>
    /// <returns></returns>
    Task UpdateMemberAsync( long id, List<User> member, bool? isUpdate = true );
    /// <summary>
    /// lấy danh sách member tham gia project mà PM đang quản lý, trong TH project active
    /// </summary>
    /// <param name="projectManager"></param>
    /// <returns></returns>
    Task<List<long>> GetMemberOfProjects( long projectManager );
  }
}
