using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface ILeaveRepository : IGenericRepository<Leave>
  {
    /// <summary>
    /// lấy danh sách leave kể từ leave request đang quan hệ cùng,
    /// sử dụng cho logic tính toán lại quan hệ giữa leave và leave request khi request rejected
    /// </summary>
    /// <param name="type"></param>
    /// <param name="leaveRequestId"></param>
    /// <returns></returns>
    Task<List<Leave>> GetListBeginLeaveRequest( int type, long leaveRequestId );
  }
}
