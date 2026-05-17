using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface ITaxBracketService : IGenericService<TaxBracket>
  {
    Task<IEnumerable<TaxBracketResponse>> GetList( int year );
    Task<long> Create( TaxBracketResource resource, long creator );
    Task Update( TaxBracketResource resource, long editor );
    Task Delete( long id );
  }
}
