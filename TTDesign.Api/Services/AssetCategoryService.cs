using AutoMapper;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class AssetCategoryService : GenericService<AssetCategory>, IAssetCategoryService
  {
    private readonly IAssetCategoryRepository _assetCategoryRepository;
    private readonly IAssetRepository _assetRepository;
    private readonly IMapper _mapper;
    public AssetCategoryService( IAssetCategoryRepository assetCategoryRepository, IMapper mapper, IAssetRepository assetRepository ) : base( assetCategoryRepository )
    {
      _assetCategoryRepository = assetCategoryRepository;
      _mapper = mapper;
      _assetRepository = assetRepository;
    }

    public async Task<bool> CheckHadUsingBeforeDelete( long id )
    {
      var assetCategory = await _assetCategoryRepository.GetByCondition( x => x.ParentId == id );
      var assetCategories = await _assetRepository.GetByCondition( x => x.AssetCategoryId != id );
      if ( assetCategories != null ) {
        return true;
      }
      else if ( assetCategory != null ) {
        return true;
      }
      return false;
    }

    public async Task<long> Create( AssetCategoryResource obj, long creator )
    {
      var assetCategory = _mapper.Map<AssetCategory>( obj );
      assetCategory.Code = obj.Code ?? Common.GeneratedCode( assetCategory.Name );
      assetCategory.CreatedBy = creator;
      if ( assetCategory.ParentId != null ) {
        assetCategory.Level = 2;
      }
      else {
        assetCategory.Level = 1;
      }
      await _assetCategoryRepository.CreateAsync( assetCategory );
      return assetCategory.Id;
    }

    public async Task<IEnumerable<AssetCategoryResponse>> GetList( BaseFilter filter )
    {
      var assetCategories = await _assetCategoryRepository.GetListByConditionTrack( x => x.ParentId == null );
      var result = _mapper.Map<IEnumerable<AssetCategoryResponse>>( assetCategories );
      foreach ( var item in result ) {

        item.Children = _mapper.Map<List<AssetCategoryResponse>>( await _assetCategoryRepository.GetListByConditionTrack( x => x.ParentId == item.Id ) );
      }
      return result.OrderBy( x => x.Name ).ToList();
    }

    public async Task Update( AssetCategoryResource resouce, long editor )
    {
      var old = await _assetCategoryRepository.GetByCondition( c => c.Id == resouce.Id );
      old!.Code = resouce.Code ?? Common.GeneratedCode( old.Name );
      old.ModifiedBy = editor;
      if ( old.ParentId != null ) {
        old.Level = 2;
      }
      else {
        old.Level = 1;
      }
      _assetCategoryRepository.Update( old );
    }
  }
}
