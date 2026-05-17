using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Repositories
{
  public interface IBonusRepository : IGenericRepository<Bonus>
  {
    Task<IEnumerable<BonusResponse>> GetListWithUser( int month, int year );
  }
}
