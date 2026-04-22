using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Repositories
{
  public interface ISwapDayRepository : IGenericRepository<SwapDay>
  {
    /// <summary>
    /// lấy danh sách Swap Day kèm danh sách User áp dụng
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public Task<IEnumerable<SwapDayResponse>> GetListData( Expression<Func<SwapDay, bool>> predicate );
    /// <summary>
    /// lấy record SwapDay kèm danh sách SwapDayUser
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public Task<SwapDay?> GetData( Expression<Func<SwapDay, bool>> predicate );
  }
}
