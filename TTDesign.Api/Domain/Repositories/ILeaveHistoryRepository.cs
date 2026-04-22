using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface ILeaveHistoryRepository : IGenericRepository<LeaveHistory>
  {
    /// <summary>
    /// lấy record history mới nhất
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<LeaveHistory?> GetLast( long userId );
  }
}
