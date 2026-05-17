using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class TaxBracketRepository : GenericRepository<TaxBracket>, ITaxBracketRepository
  {
    public TaxBracketRepository( AppDbContext context ) : base( context ) { }
  }
}
