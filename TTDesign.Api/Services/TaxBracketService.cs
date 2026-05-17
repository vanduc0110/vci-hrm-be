using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class TaxBracketService : GenericService<TaxBracket>, ITaxBracketService
  {
    private readonly ITaxBracketRepository _taxBracketRepository;

    public TaxBracketService( ITaxBracketRepository taxBracketRepository ) : base( taxBracketRepository )
    {
      _taxBracketRepository = taxBracketRepository;
    }

    public async Task<IEnumerable<TaxBracketResponse>> GetList( int year )
    {
      var brackets = await _taxBracketRepository.GetListByCondition( t => t.Year == year );
      return brackets.OrderBy( t => t.FromAmount ).Select( t => new TaxBracketResponse
      {
        Id = t.Id,
        FromAmount = t.FromAmount,
        ToAmount = t.ToAmount,
        TaxRate = t.TaxRate,
        QuickDeduction = t.QuickDeduction,
        Year = t.Year,
        IsActive = t.IsActive,
      } );
    }

    public async Task<long> Create( TaxBracketResource resource, long creator )
    {
      var bracket = new TaxBracket
      {
        FromAmount = resource.FromAmount,
        ToAmount = resource.ToAmount,
        TaxRate = resource.TaxRate,
        QuickDeduction = resource.QuickDeduction,
        Year = resource.Year,
        IsActive = true,
        CreatedBy = creator,
        CreatedDate = DateTime.UtcNow,
      };
      await _taxBracketRepository.CreateAsync( bracket );
      return bracket.Id;
    }

    public async Task Update( TaxBracketResource resource, long editor )
    {
      var bracket = await _taxBracketRepository.GetByCondition( t => t.Id == resource.Id );
      if ( bracket == null ) throw new Exception( "Không tìm thấy bậc thuế" );
      bracket.FromAmount = resource.FromAmount;
      bracket.ToAmount = resource.ToAmount;
      bracket.TaxRate = resource.TaxRate;
      bracket.QuickDeduction = resource.QuickDeduction;
      bracket.Year = resource.Year;
      bracket.ModifiedBy = editor;
      bracket.ModifiedDate = DateTime.UtcNow;
      _taxBracketRepository.Update( bracket );
    }

    public async Task Delete( long id )
    {
      var bracket = await _taxBracketRepository.GetByCondition( t => t.Id == id );
      if ( bracket == null ) throw new Exception( "Không tìm thấy bậc thuế" );
      _taxBracketRepository.Delete( bracket );
    }
  }
}
