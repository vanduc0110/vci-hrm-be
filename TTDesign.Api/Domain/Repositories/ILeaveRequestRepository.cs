using System.Linq.Expressions;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
  {
    /// <summary>
    /// lấy danh sách leave request, kèm theo các leave request detail
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<LeaveRequest>> GetLeaveRequestDetails( Expression<Func<LeaveRequest, bool>> predicate );
    /// <summary>
    /// lấy danh sách leave request quan hệ với danh sách leaves, trừ leave request id input
    /// </summary>
    /// <param name="leaveIds">danh sách id leaves</param>
    /// <param name="leaveRequestId">leave request id loại trừ</param>
    /// <returns></returns>
    Task<List<LeaveRequest>> GetListByLeaves( List<long> leaveIds, long leaveRequestId );
    /// <summary>
    /// xóa quan hệ giữa leave request và leave (xóa LeaveHistoryRequest)
    /// </summary>
    /// <param name="leaveRequestIds"></param>
    /// <returns></returns>
    Task RemoveLeaveHistoryUsing( List<long> leaveRequestIds );
  }
}
