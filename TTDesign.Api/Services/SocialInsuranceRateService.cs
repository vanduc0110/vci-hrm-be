using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class SocialInsuranceRateService : GenericService<SocialInsuranceRate>, ISocialInsuranceRateService
  {
    private readonly ISocialInsuranceRateRepository _rateRepository;

    public SocialInsuranceRateService( ISocialInsuranceRateRepository rateRepository ) : base( rateRepository )
    {
      _rateRepository = rateRepository;
    }

    public async Task<IEnumerable<SocialInsuranceRateResponse>> GetList()
    {
      var rates = await _rateRepository.GetAll();
      return rates.OrderByDescending( r => r.EffectiveDate ).Select( r => new SocialInsuranceRateResponse
      {
        Id = r.Id,
        SocialInsuranceEmployee = r.SocialInsuranceEmployee,
        HealthInsuranceEmployee = r.HealthInsuranceEmployee,
        UnemploymentInsuranceEmployee = r.UnemploymentInsuranceEmployee,
        PersonalDeduction = r.PersonalDeduction,
        DependentDeduction = r.DependentDeduction,
        EffectiveDate = r.EffectiveDate,
        IsActive = r.IsActive,
      } );
    }

    public async Task<SocialInsuranceRateResponse?> GetActive()
    {
      var rate = await _rateRepository.GetByConditionNoTrack( r => r.IsActive );
      if ( rate == null ) return null;
      return new SocialInsuranceRateResponse
      {
        Id = rate.Id,
        SocialInsuranceEmployee = rate.SocialInsuranceEmployee,
        HealthInsuranceEmployee = rate.HealthInsuranceEmployee,
        UnemploymentInsuranceEmployee = rate.UnemploymentInsuranceEmployee,
        PersonalDeduction = rate.PersonalDeduction,
        DependentDeduction = rate.DependentDeduction,
        EffectiveDate = rate.EffectiveDate,
        IsActive = rate.IsActive,
      };
    }

    public async Task<long> Create( SocialInsuranceRateResource resource, long creator )
    {
      var existing = await _rateRepository.GetByCondition( r => r.IsActive );
      if ( existing != null ) {
        existing.IsActive = false;
        existing.ModifiedBy = creator;
        existing.ModifiedDate = DateTime.UtcNow;
        _rateRepository.Update( existing );
      }

      var rate = new SocialInsuranceRate
      {
        SocialInsuranceEmployee = resource.SocialInsuranceEmployee,
        HealthInsuranceEmployee = resource.HealthInsuranceEmployee,
        UnemploymentInsuranceEmployee = resource.UnemploymentInsuranceEmployee,
        PersonalDeduction = resource.PersonalDeduction,
        DependentDeduction = resource.DependentDeduction,
        EffectiveDate = resource.EffectiveDate,
        IsActive = true,
        CreatedBy = creator,
        CreatedDate = DateTime.UtcNow,
      };
      await _rateRepository.CreateAsync( rate );
      return rate.Id;
    }
  }
}
