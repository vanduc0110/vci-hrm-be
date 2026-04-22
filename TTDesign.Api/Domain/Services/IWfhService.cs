using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IWfhService : IGenericService<WfhRequest>,
    BaseServiceResource<WfhResource>, BaseServiceList<WfhResponse>
  {
    /// <summary>
    /// lấy danh sách request WFH của user trong 1 tháng
    /// </summary>
    /// <param name="userRequest"></param>
    /// <param name="inYear"></param>
    /// <returns></returns>
    Task<IEnumerable<WfhResponse>> GetRequestList( long userRequest, long inYear );
    /// <summary>
    /// chuyển trạng thái request WFH
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isApprove"></param>
    /// <param name="reviewer"></param>
    /// <returns></returns>
    Task Approve( long id, bool isApprove, long reviewer );
    /// <summary>
    /// 
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    Task<int> GetTotalWfhRequest( long? teamId = null );
  }
}
