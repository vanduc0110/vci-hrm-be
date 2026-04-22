using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Repositories
{
  public interface ITeamCategoryRepository : IGenericRepository<TeamCategory>
  {
    /// <summary>
    /// Lấy danh sách thu gọn Category
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    Task<IEnumerable<CategoryOption>> GetOption( long? teamId = null );
  }
}
