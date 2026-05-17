using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class SocialInsuranceRateRepository : GenericRepository<SocialInsuranceRate>, ISocialInsuranceRateRepository
  {
    public SocialInsuranceRateRepository( AppDbContext context ) : base( context ) { }
  }
}
