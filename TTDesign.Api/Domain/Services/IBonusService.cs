using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IBonusService : IGenericService<Bonus>
  {
    Task<IEnumerable<BonusResponse>> GetList( int month, int year, long[]? allowedUserIds = null );
    Task<long> Create( BonusResource resource, long creator );
    Task Approve( long id, long approvedBy );
    Task Delete( long id );
  }
}
