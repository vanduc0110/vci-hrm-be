using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IUserReportService : IGenericService<UserReport>, BaseServiceList<UserReportResponse>, BaseServiceResource<UserReportResource>
  {
    /// <summary>
    /// change status
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <param name="editor"></param>
    /// <returns></returns>
    public Task UpdateStatus( long id, long editor, int status );
  }
}
