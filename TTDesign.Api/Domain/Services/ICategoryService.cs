using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface ICategoryService : IGenericService<TeamCategory>, BaseServiceResource<CategoryResource>, BaseServiceOption<CategoryOption>, BaseServiceList<CategoryResponse>
  {
    /// <summary>
    /// Valid Category trước khi tiến hành xóa
    /// </summary>
    /// <param name="id"></param>
    /// <returns>true: had using</returns>
    Task<bool> CheckCategoryBeforeDelete( long id );
  }
}
