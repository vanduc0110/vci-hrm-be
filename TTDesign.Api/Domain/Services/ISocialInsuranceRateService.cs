using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface ISocialInsuranceRateService : IGenericService<SocialInsuranceRate>
  {
    Task<IEnumerable<SocialInsuranceRateResponse>> GetList();
    Task<SocialInsuranceRateResponse?> GetActive();
    Task<long> Create( SocialInsuranceRateResource resource, long creator );
  }
}
