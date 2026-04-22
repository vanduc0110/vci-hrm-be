using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IProjectService : IGenericService<Project>,
    BaseServiceResource<ProjectResource>, BaseServiceList<ProjectResponse>, BaseServiceDetail<ProjectDetailResponse>
  {
    /// <summary>
    /// danh sách project (hoạt động) mà user tham gia
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<ProjectOption>> GetOptionWorking( long userId );
    /// <summary>
    /// danh sách project của team quản lý hoặc là user làm PM
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="teamId"></param>
    /// <returns></returns>
    Task<IEnumerable<ProjectOption>> GetOption( long userId, long? teamId = null );
    /// <summary>
    /// chi tiết project, kèm danh sách groups
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProjectDetailResponse?> GetDetailForEdit( long id );
    /// <summary>
    /// kiểm tra project đã được khai báo timesheet chưa
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> HadTimesheet( long id );
    /// <summary>
    /// xóa project
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete( long id );
    /// <summary>
    /// lấy tổng hợp số liệu của project
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="teamId"></param>
    /// <param name="userId"></param>
    /// <param name="categoriesFilter"></param>
    /// <returns></returns>
    Task<AnalysisResponse> GetAnalysisView( long projectId, long? teamId = null, long? userId = null, HashSet<ProjectObjectDetail>? categoriesFilter = null );
    /// <summary>
    /// lấy danh sách document của project
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    Task<List<ProjectContractResponse>> GetProjectContract( long projectId );
    /// <summary>
    /// tạo thông tin hợp đồng cho project
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="resource"></param>
    /// <returns></returns>
    Task<long> CreateContract( long projectId, ProjectContractResource resource );
    Task UpdateContract( long projectId, ProjectContractResource resource, long editor );
    /// <summary>
    /// xóa document
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    Task<List<ProjectDocumentDetail>> DeleteContractDocument( long documentId );

    Task DeleteContract( long contractId );
    /// <summary>
    /// xuất danh sách timesheet làm việc cho project
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<byte []?> ExportAnalysis( long id );
    /// <summary>
    /// kiểm tra user PM có làm quản lý 1 project nào đó mà user tham gia
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="projectManager"></param>
    /// <returns>true: đúng, false: sai</returns>
   // Task<bool> IsMemberOfProject( long id, long userId, long projectManager );

    /// <summary>
    /// get danh sách document trong hợp đồng
    /// </summary>
    /// <param name="contractId"></param>
    /// <returns></returns>
    Task<List<ProjectDocumentDetail>> GetDocumentInContract( long contractId );
    /// <summary>
    /// thêm list user vào project
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    Task<List<UserOption>> AddUserProject( long id, List<long> userIds );
    /// <summary>
    /// remove user ra khỏi project
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<UserOption>> RemoveUserProject( long id, long userId );
  }
}
