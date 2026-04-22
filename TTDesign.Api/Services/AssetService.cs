using AutoMapper;
using ClosedXML.Excel;
using Newtonsoft.Json;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{

  public class AssetService : GenericService<Asset>, IAssetService
  {
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetCategoryRepository _assetCategoryRepository;
    private readonly IComponentRepository _componentRepository;
    private readonly IAssetComponentRepository _assetComponentRepository;
    private readonly IMapper _mapper;
    private readonly IAssetAllocateRepository _assetAllocateRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWebHostEnvironment _env;
    public AssetService( IAssetRepository assetRepository, IMapper mapper,
      IAssetCategoryRepository assetCategoryRepository,
      IComponentRepository componentRepository, IAssetComponentRepository assetComponentRepository,
      IAssetAllocateRepository assetAllocateRepository,
      IUserRepository userRepository,
      IWebHostEnvironment env ) : base( assetRepository )
    {
      _assetRepository = assetRepository;
      _mapper = mapper;
      _assetCategoryRepository = assetCategoryRepository;
      _componentRepository = componentRepository;
      _assetComponentRepository = assetComponentRepository;
      _assetAllocateRepository = assetAllocateRepository;
      _userRepository = userRepository;
      _env = env;
    }

    public async Task AddComponentToAsset( long assetId, List<AssetToComponent> objs, long creator )
    {
      var asset = await _assetRepository.GetByCondition( a => a.Id == assetId );
      var components = await _componentRepository.GetListByCondition( a => a.AssetId == assetId );
      foreach ( var comp in objs ) {
        var compo = components.FirstOrDefault( c => c.ComponentId == comp.AssetId );
        if ( compo != null ) {
          continue;
        }
        else {
          var assetComp = await _assetRepository.GetByCondition( a => a.Id == comp.AssetId );
          var newComp = _mapper.Map<Component>( assetComp! );
          newComp.AssetId = assetId;
          newComp.ComponentId = comp.AssetId;
          newComp.CreatedBy = creator;
          await _componentRepository.CreateAsync( newComp );
          await _assetComponentRepository.CreateAsync( new AssetComponentHistory { AssetId = assetId, ComponentId = comp.AssetId, CreatedBy = creator, Type = 0, Note = comp.Note ?? "Add component from asset" } );
          assetComp!.Status = ( int ) Enums.AssetStatus.InUse;

          var assetCategory = await _assetCategoryRepository.GetByCondition( a => a.Id == assetComp.AssetCategoryId );
          if ( assetCategory!.Name == "Computer" ) {
            var componentAsset = await _assetRepository.GetByCondition( a => a.Id == comp.AssetId );
            UpdateComputerSpecs( assetComp, componentAsset!, true );
          }
          _assetRepository.Update( assetComp );
        }
      }
    }

    public async Task RemoveComponentToAsset( long assetId, long componentId, long editor )
    {
      var asset = await _assetRepository.GetByCondition( a => a.Id == componentId );
      var component = await _componentRepository.GetByCondition( a => a.AssetId == assetId && a.ComponentId == componentId );
      asset!.Status = ( int ) Enums.AssetStatus.InStorage;
      await _assetComponentRepository.CreateAsync( new AssetComponentHistory { AssetId = assetId, ComponentId = componentId, CreatedBy = editor, Type = 1, Note = "Remove component from asset" } );
      _componentRepository.Delete( component! );
      var assetCategory = await _assetCategoryRepository.GetByCondition( a => a.Id == asset.AssetCategoryId );
      if ( assetCategory!.Name == "Computer" || assetCategory!.Name == "Screen" ) {
        var componentAsset = await _assetRepository.GetByCondition( a => a.Id == componentId );
        UpdateComputerSpecs( asset, componentAsset!, true );
      }
      _assetRepository.Update( asset );
    }
    private void UpdateComputerSpecs( Asset computerAsset, Asset componentAsset, bool isInstalling )
    {
      if ( string.IsNullOrEmpty( computerAsset.DetailedSpecs ) )
        return;

      try {
        var computerSpecs = JsonConvert.DeserializeObject<ComputerSpecs>( computerAsset.DetailedSpecs );
        if ( computerSpecs == null )
          computerSpecs = new ComputerSpecs();

        var componentSpecs = string.IsNullOrEmpty( componentAsset.DetailedSpecs )
            ? null
            : JsonConvert.DeserializeObject<ComponentSpecs>( componentAsset.DetailedSpecs );

        if ( componentSpecs == null )
          return;
        switch ( componentSpecs.ComponentType?.ToUpper() ) {
          case "RAM":
            computerSpecs.RAM = isInstalling ? componentAsset.Name : "";
            break;
          case "SSD":
            computerSpecs.SSD = isInstalling ? componentAsset.Name : "";
            break;
          case "CPU":
            computerSpecs.CPU = isInstalling ? componentAsset.Name : "";
            break;
          case "GPU":
            computerSpecs.GPU = isInstalling ? componentAsset.Name : "";
            break;
        }
        computerAsset.DetailedSpecs = JsonConvert.SerializeObject( computerSpecs );
      }
      catch ( JsonException ) {
      }
    }
    public async Task<long> Create( AssetResource obj, long creator )
    {
      var asset = _mapper.Map<Asset>( obj );
      asset.AssetCode = obj.AssetCode ?? await GenerateAssetCode( asset );
      asset.CreatedBy = creator;
      await _assetRepository.CreateAsync( asset );
      if ( obj.Component != null && obj.Component.Count() > 0 ) {
        await AddComponentToAsset( asset.Id, obj.Component, creator );
      }
      return asset.Id;
    }

    public async Task<AssetDetailResponse?> GetDetail( long id )
    {
      var asset = await _assetRepository.GetDetailByConditionNoTrack( a => a.Id == id );
      if ( asset!.AssetCategory.Name == "Computer" ) {
        var response = _mapper.Map<AssetDetailResponse>( asset! );
        var source = _mapper.Map<ComputerSpecs>( response );
        var comp = JsonConvert.DeserializeObject<ComputerSpecs>( asset.DetailedSpecs! );
        source.CPU = comp!.CPU;
        source.Type = comp.Type;
        source.RAM = comp!.RAM;
        source.GPU = comp.GPU;
        source.Type = comp.Type;
        source.SSD = comp.SSD;
        source.Allocations = _mapper.Map<IEnumerable<AssetAllocationResponse>>( asset.AssetAllocations );
        return source;

      }
      else if ( asset!.AssetCategory.Name == "Screen" ) {
        var response = _mapper.Map<AssetDetailResponse>( asset! );
        var source = _mapper.Map<MonitorSpecs>( response );
        var comp = JsonConvert.DeserializeObject<MonitorSpecs>( asset.DetailedSpecs! );
        source.ScreenSize = comp!.ScreenSize;
        return source;

      }
      else if ( asset!.AssetCategory.Name == "Accessory" ) {
        var response = _mapper.Map<AssetDetailResponse>( asset! );
        var source = _mapper.Map<ComponentSpecs>( response );
        var comp = asset.DetailedSpecs! != null ? JsonConvert.DeserializeObject<ComponentSpecs>( asset.DetailedSpecs! ) : new ComponentSpecs();
        source.ComponentType = comp!.ComponentType;
        source.ReplacementOrStorageDate = comp.ReplacementOrStorageDate;
        return source;

      }
      else {
        return _mapper.Map<AssetDetailResponse>( asset );
      }
    }

    public async Task<IEnumerable<AssetResponse>> GetList( BaseFilter filter )
    {
      var assets = await _assetRepository.GetAll();
      if ( filter.AssetStatus != null ) {
        assets = assets.Where( a => a.Status == ( int ) Enum.Parse( typeof( Enums.AssetStatus ), filter.AssetStatus ) );
      }
      if ( filter.AssetCategoryId != null ) {
        var assetCategory = await _assetCategoryRepository.GetByCondition( a => a.Id == filter.AssetCategoryId );
        var parentCategory = await _assetCategoryRepository.GetListByCondition( a => a.ParentId == assetCategory!.Id );
        if ( parentCategory != null && parentCategory.Count() > 0 ) {
          var parentCategoryIds = parentCategory.Select( a => a.Id ).ToList();
          assets = assets.Where( a => parentCategoryIds.Any( x => x == a.AssetCategoryId ) );
        }
        else {
          assets = assets.Where( a => a.AssetCategoryId == filter.AssetCategoryId );
        }
      }
      var response = _mapper.Map<IEnumerable<AssetResponse>>( assets );
      foreach ( var item in response ) {
        var assetAllocations = await _assetAllocateRepository.GetListByConditionTrack( x => x.AssetId == item.Id && x.EventType == ( int ) ( Enums.AllocationEventType.Allocate ) );
        item.AssetCategory = _mapper.Map<AssetCategoryResponse>( await _assetCategoryRepository.GetByCondition( a => a.Id == item.AssetCategoryId ) );
        var assetUser = ( assetAllocations != null && assetAllocations.Count() > 0 ) ? assetAllocations.OrderBy( x => x.EventDate ).FirstOrDefault() : null;
        if ( assetUser == null )
          continue;
        var user = await _userRepository.GetByCondition( x => x.Id == assetUser!.CurrentUserId );
        item.AssignmentUser = user!.FullName;
        item.AssignmentDate = assetUser.EventDate;

      }
      return response;
    }
    public async Task Update( AssetResource resouce, long editor )
    {
      var old = await _assetRepository.GetByCondition( a => a.Id == resouce.Id );
      old = _mapper.Map( resouce, old );
      old!.AssetCode = resouce.AssetCode ?? await GenerateAssetCode( old );
      old.ModifiedBy = editor;
      _assetRepository.Update( old );
    }
    private async Task<string> GenerateAssetCode( Asset asset )
    {
      var lastAssets = await _assetRepository.GetListByCondition( a => a.AssetCategoryId == asset.AssetCategoryId );
      if ( lastAssets != null && lastAssets.Count() > 0 ) {
        var lastAsset = lastAssets.OrderByDescending( a => a.CreatedDate ).FirstOrDefault();
        string [] codeParts = lastAsset!.AssetCode.Split( '-' );
        var lastNumber = Convert.ToInt32( codeParts.Last() );
        var assetCode = string.Empty;
        for ( int i = 0; i < codeParts.Length - 1; i++ ) {
          assetCode += codeParts [ i ] + "-";
        }
        return assetCode + ( lastNumber + 1 ).ToString( "D3" );
      }
      else {
        var assetCategory = await _assetCategoryRepository.GetByCondition( a => a.Id == asset.AssetCategoryId );
        if ( assetCategory!.ParentId != null ) {
          var parentCategory = await _assetCategoryRepository.GetByCondition( a => a.Id == assetCategory.ParentId );
          return $"{parentCategory!.Code}-{assetCategory.Code}-{Enums.AssetCode.ToString( "D3" )}";
        }
        else {
          return $"{assetCategory!.Code}-{Enums.AssetCode.ToString( "D3" )}";
        }
      }
    }

    public async Task AllocateAsset( AllocateAssetResource obj, long userId )
    {
      foreach ( var assetId in obj.AssetIds ) {
        var asset = await _assetRepository.GetByCondition( a => a.Id == assetId );
        var allocationUsing = new AssetAllocation
        {
          AssetId = asset!.Id,
          EventDate = obj.AllocationDate ?? DateTime.UtcNow,
          CurrentUserId = obj!.UserId,
          OldUserId = null,
          EventType = ( int ) Enums.AllocationEventType.Allocate,
          StatusNotes = obj.Notes,
          CreatedBy = userId,
          Quantity = obj.Quantity,
          AssetStatus = ( int ) Enums.AssetStatus.InUse
        };
        if ( asset.Quantity > 1 && asset.Quantity > obj.Quantity ) {
          var allocation = new AssetAllocation
          {
            AssetId = asset!.Id,
            EventDate = obj.AllocationDate ?? DateTime.UtcNow,
            CurrentUserId = obj!.UserId,
            OldUserId = null,
            EventType = null,
            StatusNotes = obj.Notes,
            CreatedBy = userId,
            Quantity = asset.Quantity - obj.Quantity,
            AssetStatus = ( int ) Enums.AssetStatus.InStorage
          };
          await _assetAllocateRepository.CreateAsync( allocation );
          asset.Status = ( int ) Enums.AssetStatus.Null;
        }
        else {
          asset.Status = ( int ) Enums.AssetStatus.InUse;
        }
        await _assetAllocateRepository.CreateAsync( allocationUsing );
        _assetRepository.Update( asset );
      }
    }
    public async Task ReturnAsset( ReturnAssetResource obj, long userId )
    {
      foreach ( var assetId in obj.AssetIds ) {
        var asset = await _assetRepository.GetByCondition( a => a.Id == assetId );
        var allocateHis = await _assetAllocateRepository.GetByCondition( a => a.AssetId == asset!.Id && a.CurrentUserId == obj.UserId
        && ( a.EventType == ( int ) Enums.AllocationEventType.Allocate || a.EventType == ( int ) Enums.AllocationEventType.Transfer ) );
        var allocationUsing = new AssetAllocation
        {
          AssetId = asset!.Id,
          EventDate = obj.AllocationDate ?? DateTime.UtcNow,
          CurrentUserId = null,
          OldUserId = allocateHis!.CurrentUserId,
          EventType = ( int ) Enums.AllocationEventType.Return,
          StatusNotes = obj.Notes,
          CreatedBy = userId,
          Quantity = obj.Quantity
        };
        if ( asset.Quantity > 1 ) {
          var allocateReturn = await _assetAllocateRepository.GetByCondition( a => a.AssetId == asset!.Id && a.AssetStatus == ( int ) Enums.AssetStatus.InStorage );

          var allocation = new AssetAllocation
          {
            AssetId = asset!.Id,
            StatusNotes = obj.Notes,
            CreatedBy = userId,
            Quantity = allocateReturn!.Quantity + obj.Quantity,
            AssetStatus = ( int ) Enums.AssetStatus.InStorage,
            EventDate = obj.AllocationDate ?? DateTime.UtcNow,
          };
          await _assetAllocateRepository.CreateAsync( allocation );
          asset.Status = asset.Quantity == allocation.Quantity ? ( int ) Enums.AssetStatus.InStorage : ( int ) Enums.AssetStatus.Null;
        }
        else {
          asset.Status = ( int ) Enums.AssetStatus.InStorage;
        }
        await _assetAllocateRepository.CreateAsync( allocationUsing );
        _assetAllocateRepository.Delete( allocateHis );
        _assetRepository.Update( asset );
      }
    }
    public async Task TranferAsset( TransferAssetResource obj, long userId )
    {
      foreach ( var assetId in obj.AssetIds ) {
        var asset = await _assetRepository.GetByCondition( a => a.Id == assetId );
        var allocateHis = await _assetAllocateRepository.GetByCondition( a => a.AssetId == asset!.Id && a.CurrentUserId == obj.UserId && a.EventType == ( int ) Enums.AllocationEventType.Allocate );

        var allocation = new AssetAllocation
        {
          AssetId = asset!.Id,
          EventDate = obj.TransferDate ?? DateTime.UtcNow,
          CurrentUserId = obj.NewUserId,
          OldUserId = obj.UserId,
          EventType = ( int ) Enums.AllocationEventType.Transfer,
          StatusNotes = obj.Notes,
          CreatedBy = userId,
          AssetStatus = ( int ) Enums.AssetStatus.InUse,
          Quantity = allocateHis!.Quantity
        };
        await _assetAllocateRepository.CreateAsync( allocation );
        _assetAllocateRepository.Delete( allocateHis );
      }
    }

    public async Task DisposeAsset( DisposeAssetResource obj, long userId )
    {
      var asset = await _assetRepository.GetByCondition( a => a.Id == obj.AssetId );
      var allocation = new AssetAllocation
      {
        AssetId = asset!.Id,
        EventDate = obj.DisposeDate ?? DateTime.UtcNow,
        CurrentUserId = null,
        OldUserId = null,
        EventType = ( int ) Enums.AllocationEventType.Dispose,
        StatusNotes = obj.Notes,
        CreatedBy = userId,
      };
      if ( asset.Quantity > 1 ) {
        var allocateSto = await _assetAllocateRepository.GetByCondition( a => a.AssetId == asset!.Id && a.AssetStatus == ( int ) Enums.AssetStatus.InStorage );
        var allocationStorage = new AssetAllocation
        {
          AssetId = asset!.Id,
          CreatedBy = userId,
          Quantity = allocateSto != null ? allocateSto!.Quantity - obj.Quantity : ( asset.Quantity - obj.Quantity ),
          AssetStatus = ( int ) Enums.AssetStatus.InStorage,
          EventDate = obj.DisposeDate ?? DateTime.UtcNow,
        };
        await _assetAllocateRepository.CreateAsync( allocationStorage );
        asset.Status = ( int ) Enums.AssetStatus.Null;
      }
      else {
        asset.Status = ( int ) Enums.AssetStatus.Disposed;
      }
      await _assetAllocateRepository.CreateAsync( allocation );
      _assetRepository.Update( asset );
    }

    public async Task DestroyAsset( DestroyAssetResource obj, long userId )
    {
      var asset = await _assetRepository.GetByCondition( a => a.Id == obj.AssetId );
      var allocation = new AssetAllocation
      {
        AssetId = asset!.Id,
        EventDate = obj.DestroyDate ?? DateTime.UtcNow,
        CurrentUserId = null,
        OldUserId = null,
        EventType = ( int ) Enums.AllocationEventType.Destroy,
        StatusNotes = obj.Notes,
        CreatedBy = userId
      };
      if ( asset.Quantity > 1 ) {
        var allocateSto = await _assetAllocateRepository.GetByCondition( a => a.AssetId == asset!.Id && a.AssetStatus == ( int ) Enums.AssetStatus.InStorage );
        var allocationStorage = new AssetAllocation
        {
          AssetId = asset!.Id,
          CreatedBy = userId,
          Quantity = allocateSto!.Quantity - obj.Quantity,
          AssetStatus = ( int ) Enums.AssetStatus.InStorage,
          EventDate = obj.DestroyDate ?? DateTime.UtcNow,
        };
        await _assetAllocateRepository.CreateAsync( allocationStorage );
        asset.Status = ( int ) Enums.AssetStatus.Null;
      }
      else {
        asset.Status = ( int ) Enums.AssetStatus.Damaged;
      }
      await _assetAllocateRepository.CreateAsync( allocation );
      _assetRepository.Update( asset );
    }

    public async Task<IEnumerable<AssetAllocationResponse>> GetAssignment()
    {
      var allocations = await _assetAllocateRepository.GetAll();
      allocations = allocations.Where( a => a.EventType != null );
      var allocationsAssign = _mapper.Map<IEnumerable<AssetAllocationResponse>>( allocations );
      foreach ( var item in allocationsAssign ) {
        var asset = await _assetRepository.GetByCondition( a => a.Id == item.AssetId );
        var currentUser = await _userRepository.GetByCondition( u => u.Id == item.CurrentUserId );
        item.Name = asset!.Name;
        item.CurrentUser = item.CurrentUserId != null ? new DashboardUser
        {
          Id = currentUser!.Id,
          Name = currentUser.FullName,
          State = Enum.GetName( typeof( Enums.UserState ), currentUser.State )!,
          Avatar = currentUser.Avatar
        } : null;

        var old = await _userRepository.GetByCondition( u => u.Id == item.OldUserId );
        item.PreviousUser = item.OldUserId != null ? new DashboardUser
        {
          Id = old!.Id,
          Name = old.FullName,
          State = Enum.GetName( typeof( Enums.UserState ), old.State )!,
          Avatar = old.Avatar
        } : null;
        item.AssetCode = asset!.AssetCode;

        var findUser = await _userRepository.GetByConditionNoTrack( u => u.Id == item.CreatedBy );
        item.ModifiedName = findUser != null ? findUser!.FullName : string.Empty;
      }
      return allocationsAssign;
    }

    public async Task<IEnumerable<AssetUserResponse>> GetAssetsUser()
    {
      var users = await _userRepository.GetUsersDataByCondition();
      users = users.Where( u => u.IsActive ).ToList();
      var assetUsers = new List<AssetUserResponse>();
      foreach ( var user in users ) {
        var assetUser = new AssetUserResponse
        {
          Id = user.Id,
          Fullname = user.FullName,
          Position = Enum.GetName( typeof( Enums.UserPosition ), user.Position )!,
          TeamName = string.Join( ",", user.TeamUsers.Select( x => x.Team.Name ) ) ?? string.Empty
        };
        var assetAllocations = await _assetAllocateRepository.GetDataListByCondition( x => x.CurrentUserId.HasValue && x.CurrentUserId == user.Id );

        var asset = assetAllocations.Where( x => ( x.Asset.Status == ( int ) Enums.AssetStatus.InUse || x.Asset.Status == ( int ) Enums.AssetStatus.Null ) && x.EventType != null );
        if ( assetAllocations == null || assetAllocations.Count() == 0 || asset == null || asset.Count() == 0 ) {
          continue;
        }
        assetUser.Total = asset.Count();
        assetUser.Computers = asset.Where( x => x.Asset.AssetCategory.Name == "Computer" ).Sum( x => x.Quantity );
        assetUser.Screens = asset.Where( x => x.Asset.AssetCategory.Name == "Screen" ).Sum( x => x.Quantity );
        assetUser.Components = asset.Where( x => x.Asset.AssetCategory.Name == "Accessory" ).Sum( x => x.Quantity );
        assetUser.Electronics = asset.Where( x => x.Asset.AssetCategory.Name == "Electronics" ).Sum( x => x.Quantity );
        assetUser.Funiture = asset.Where( x => x.Asset.AssetCategory.Name == "Furniture" ).Sum( x => x.Quantity );
        assetUser.Others = asset.Where( x => x.Asset.AssetCategory.Name == "Other" ).Sum( x => x.Quantity );
        assetUsers.Add( assetUser );
      }
      return assetUsers;
    }

    public async Task<AssetByUserResponse> GetAssetsUserDetail( long userId )
    {
      var user = await _userRepository.GetUserDataByConditionNoTracking( u => u.Id == userId );
      var assets = await _assetRepository.GetListAssetByCondition( x => x.AssetAllocations.Any( a => a.CurrentUserId == userId ) && ( x.Status == ( int ) Enums.AssetStatus.InUse || x.Status == ( int ) Enums.AssetStatus.Null ) );
      foreach ( var item in assets ) {
        var allocationAsset = await _assetAllocateRepository.GetListByConditionTrack( x => x.AssetId == item.Id && x.AssetStatus == ( int ) Enums.AssetStatus.InUse );
        if ( item.Status == ( int ) Enums.AssetStatus.Null ) {
          item.Quantity = allocationAsset.Sum( x => x.Quantity );
        }
      }
      var assetByUsers = new AssetByUserResponse();
      assetByUsers.Id = user!.Id;
      assetByUsers.Fullname = user.FullName;
      assetByUsers.Position = Enum.GetName( typeof( Enums.UserPosition ), user.Position )!;
      assetByUsers.TeamName = string.Join( ",", user.TeamUsers.Select( x => x.Team.Name ) ) ?? string.Empty;
      assetByUsers.Assets = assets != null ? _mapper.Map<List<AssetResponse>>( assets ) : new List<AssetResponse>();
      return assetByUsers;
    }

    public async Task<IEnumerable<AssetStorageResponse>> GetAssetStorage()
    {
      var assets = await _assetRepository.GetListAssetByCondition( x => x.Status == ( int ) Enums.AssetStatus.InStorage || x.Status == ( int ) Enums.AssetStatus.Null );
      var assetStorage = _mapper.Map<IEnumerable<AssetStorageResponse>>( assets );
      foreach ( var asset in assetStorage ) {
        var assetAllocation = await _assetAllocateRepository.GetListByConditionTrack( x => x.AssetId == asset.Id && x.AssetStatus == ( int ) Enums.AssetStatus.InStorage );
        var assetLast = assetAllocation.OrderByDescending( x => x.EventDate ).FirstOrDefault();
        asset.Quantity = assetLast != null ? assetLast.Quantity : asset.Quantity;
      }

      return assetStorage;
    }
    public async Task<IEnumerable<AssetStorageOption>> GetAssetStorageOption()
    {
      var assets = await _assetRepository.GetListAssetByCondition( x => x.Status == ( int ) Enums.AssetStatus.InStorage || x.Status == ( int ) Enums.AssetStatus.Null );
      var assetStorage = _mapper.Map<IEnumerable<AssetStorageOption>>( assets );
      foreach ( var asset in assetStorage ) {
        var assetAllocation = await _assetAllocateRepository.GetListByConditionTrack( x => x.AssetId == asset.Id && x.AssetStatus == ( int ) Enums.AssetStatus.InStorage );
        var assetLast = assetAllocation.OrderByDescending( x => x.EventDate ).FirstOrDefault();
        if ( asset.Quantity > 1 ) {
        }
        asset.Quantity = assetLast != null ? assetLast.Quantity : asset.Quantity;
      }

      return assetStorage;
    }
    public async Task<bool> CheckAssetBeforeDelete( long id )
    {
      var asset = await _assetRepository.GetByCondition( a => a.Id == id );
      if ( asset!.Status != ( int ) Enums.AssetStatus.InStorage ) {
        return false;
      }
      var assetAllocations = await _assetAllocateRepository.GetListByCondition( a => a.AssetId == id && a.EventType == ( int ) Enums.AllocationEventType.Allocate && a.CurrentUserId != null );
      if ( assetAllocations != null && assetAllocations.Count() > 0 ) {
        return false;
      }
      var components = await _assetComponentRepository.GetListByCondition( a => a.AssetId == id && a.Type == 0 );
      if ( components != null && components.Count() > 0 ) {
        return false;
      }
      return true;
    }

    public async Task<byte []> ExportAssets( int type )
    {
      //1: Computer, 2: Screen, 3: Accessory, 4: Electronics, 5: Furniture, 6: Other
      byte []? data = new byte [ 1 ];
      switch ( type ) {
        case 1:
          var computers = await GetList( new BaseFilter { AssetCategoryId = 5 } );
          var computerWithIndex = computers.Select( ( x, i ) => new
          {
            STT = i + 1,
            Asset = x,
            ComputerSpecs = !string.IsNullOrEmpty( x.DetailedSpecs ) ? JsonConvert.DeserializeObject<ComputerSpecs>( x.DetailedSpecs ) : new ComputerSpecs()
          } );
          data = AddSheet( "Computer", computerWithIndex, new (string, Func<dynamic, object?>) []
          {
                  ("STT", x => (int)x.STT),
                  ("Code", x => x.Asset.AssetCode),
                  ("ProductName", x => x.Asset.Name),
                  ("Brand", x => x.Asset.Brand),
                  ("CPU", x => x.ComputerSpecs.CPU),
                  ("RAM", x => x.ComputerSpecs.RAM),
                  ("SSD", x => x.ComputerSpecs.SSD),
                  ("GPU", x => x.ComputerSpecs.GPU),
                  ("Supplier", x => x.Asset.Supplier),
                  ("Purchase Date", x => x.Asset.PurchaseDate),
                  ("Serial Number", x => x.Asset.SerialNumber),
                  ("Status", x => x.Asset.Status),
                  ("Condition", x => x.Asset.Condition),
                  ("Assignee", x => x.Asset.AssignmentUser),
                  ("Assignment Date", x => x.Asset.AssignmentDate)
          } );
          break;
        case 2:
          var screens = await GetList( new BaseFilter { AssetCategoryId = 6 } );
          var screensWithIndex = screens.Select( ( x, i ) => new
          {
            STT = i + 1,
            Asset = x,
            MonitorSpecs = !string.IsNullOrEmpty( x.DetailedSpecs ) ? JsonConvert.DeserializeObject<MonitorSpecs>( x.DetailedSpecs ) : new MonitorSpecs()
          } );
          data = AddSheet( "Monitor", screensWithIndex, new (string, Func<dynamic, object?>) []
          {
                    ("STT", x => (int)x.STT),
                    ("Code", x => x.Asset.AssetCode),
                    ("ProductName", x => x.Asset.Name),
                    ("Brand", x => x.Asset.Brand),
                    ("Size", x => x.MonitorSpecs.ScreenSize),
                    ("Supplier", x => x.Asset.Supplier),
                    ("Status", x => x.Asset.PurchaseDate),
                    ("Condition", x => x.Asset.Condition),
                    ("User", x => x.Asset.AssignmentUser),
                    ("Serial Number", x => x.Asset.SerialNumber),
                    ("Allocation Date", x => x.Asset.AssignmentDate)
          } );
          break;
        case 3:
          var components = await GetList( new BaseFilter { AssetCategoryId = 7 } );
          var componentsWithIndex = components.Select( ( x, i ) => new
          {
            STT = i + 1,
            Asset = x,
            ComponentSpecs = !string.IsNullOrEmpty( x.DetailedSpecs ) ? JsonConvert.DeserializeObject<ComponentSpecs>( x.DetailedSpecs ) : new ComponentSpecs()
          } );
          data = AddSheet( "Component", componentsWithIndex, new (string, Func<dynamic, object?>) []
          {
                  ("STT", x => (int)x.STT),
                  ("Code", x => x.Asset.AssetCode),
                  ("ProductName", x => x.Asset.Name),
                  ("Brand", x => x.Asset.Brand),
                  ("Quantity", x => x.Asset.Quantity),
                  ("Type", x => x.ComponentSpecs.ComponentType),
                  ("Supplier", x => x.Asset.Supplier),
                  ("Purchase Date", x => x.Asset.PurchaseDate),
                  ("Status", x => x.Asset.Status),
                  ("Condition", x => x.Asset.Condition),
                  ("User", x => x.Asset.AssignmentUser),
                  ("Allocation Date", x => x.Asset.AssignmentDate),
                  ("Serial Number", x => x.Asset.SerialNumber)
          } );
          break;
        case 4:
          var electronics = await GetList( new BaseFilter { AssetCategoryId = 2 } );
          var electronicsWithIndex = electronics.Select( ( x, i ) => new
          {
            STT = i + 1,
            Asset = x
          } );
          data = AddSheet( "Electronics", electronicsWithIndex, new (string, Func<dynamic, object?>) []
          {
                    ("STT", x => (int)x.STT),
                    ("Code", x => x.Asset.AssetCode),
                    ("ProductName", x => x.Asset.Name),
                    ("Brand", x => x.Asset.Brand),
                    ("Quantity", x => x.Asset.Quantity),
                    ("Supplier", x => x.Asset.Supplier),
                    ("Purchase Date", x => x.Asset.PurchaseDate),
                    ("Status", x => x.Asset.Status),
                    ("Condition", x => x.Asset.Condition),
                    ("User", x => x.Asset.AssignmentUser),
                    ("Allocation Date", x => x.Asset.AssignmentDate),
                    ("Serial Number", x => x.Asset.SerialNumber),
                    ("Notes", x => x.Asset.Notes)
          } );
          break;
        case 5:
          var funitures = await GetList( new BaseFilter { AssetCategoryId = 3 } );
          var funituresWithIndex = funitures.Select( ( x, i ) => new
          {
            STT = i + 1,
            Asset = x
          } );
          data = AddSheet( "Funitures", funituresWithIndex, new (string, Func<dynamic, object?>) []
          {
                      ("STT", x => (int)x.STT),
                      ("Code", x => x.Asset.AssetCode),
                      ("ProductName", x => x.Asset.Name),
                      ("Quantity", x => x.Asset.Quantity),
                      ("Supplier", x => x.Asset.Supplier),
                      ("Purchase Date", x => x.Asset.PurchaseDate),
                      ("Status", x => x.Asset.Status),
                      ("Condition", x => x.Asset.Condition),
                      ("User", x => x.Asset.AssignmentUser),
                      ("Allocation Date", x => x.Asset.AssignmentDate),
                      ("Notes", x => x.Asset.Notes)
          } );
          break;
        case 6:
          var others = await GetList( new BaseFilter { AssetCategoryId = 4 } );
          var otherWithIndex = others.Select( ( x, i ) => new
          {
            STT = i + 1,
            Asset = x
          } );
          data = AddSheet( "Electronics", otherWithIndex, new (string, Func<dynamic, object?>) []
          {
                        ("STT", x => (int)x.STT),
                        ("Code", x => x.Asset.AssetCode),
                        ("ProductName", x => x.Asset.Name),
                        ("Brand", x => x.Asset.Brand),
                        ("Quantity", x => x.Asset.Quantity),
                        ("Supplier", x => x.Asset.Supplier),
                        ("Purchase Date", x => x.Asset.PurchaseDate),
                        ("Status", x => x.Asset.Status),
                        ("Condition", x => x.Asset.Condition),
                        ("User", x => x.Asset.AssignmentUser),
                        ("Serial Number", x => x.Asset.SerialNumber),
                        ("Allocation Date", x => x.Asset.AssignmentDate),

                        ("Notes", x => x.Asset.Notes)
          } );
          break;

      }
      return data;
    }

    public async Task<AssetImportReponse> ImportAssets( IFormFile file, int type )
    {
      using var stream = new MemoryStream();
      await file.CopyToAsync( stream );
      using var workbook = new XLWorkbook( stream );

      var worksheet = workbook.Worksheets.First();
      if ( worksheet == null )
        return new AssetImportReponse();
      var result = new AssetImportReponse();
      var rows = worksheet.RangeUsed().RowsUsed().Skip( 1 ); // bỏ header
      var checkSuccess = false;
      foreach ( var row in rows ) {


        // Mapping thêm theo type
        switch ( type ) {
          case 1: // Computer
            var asset = new Asset
            {
              AssetCode = row.Cell( 2 ).GetString(),
              Name = row.Cell( 3 ).GetString(),
              Brand = row.Cell( 4 ).GetString(),
              Supplier = row.Cell( 9 ).GetString(),
              PurchaseDate = row.Cell( 10 ).GetString() != string.Empty ? row.Cell( 10 ).GetDateTime() : null,
              SerialNumber = row.Cell( 11 ).GetString(),
              Condition = ( int ) Enums.AssetCondition.New,
              Status = ( int ) Enums.AssetStatus.InStorage,
              AssetCategoryId = 5
            };
            checkSuccess = await _assetRepository.Exist( x => x.AssetCode == asset.AssetCode );
            if ( checkSuccess ) {
              result.Failes++;
              continue;
            }
            var specs = new ComputerSpecs
            {
              CPU = row.Cell( 5 ).GetString(),
              RAM = row.Cell( 6 ).GetString(),
              SSD = row.Cell( 7 ).GetString(),
              GPU = row.Cell( 8 ).GetString()
            };
            asset.DetailedSpecs = JsonConvert.SerializeObject( specs );
            await _assetRepository.CreateAsync( asset );
            break;

          case 2: // Monitor
            var monitor = new Asset
            {
              AssetCode = row.Cell( 2 ).GetString(),
              Name = row.Cell( 3 ).GetString(),
              Brand = row.Cell( 4 ).GetString(),
              Supplier = row.Cell( 6 ).GetString(),
              PurchaseDate = row.Cell( 7 ).GetString() != string.Empty ? row.Cell( 7 ).GetDateTime() : null,
              SerialNumber = row.Cell( 8 ).GetString(),
              Condition = ( int ) Enums.AssetCondition.New,
              Status = ( int ) Enums.AssetStatus.InStorage,
              AssetCategoryId = 6
            };
            checkSuccess = await _assetRepository.Exist( x => x.AssetCode == monitor.AssetCode );
            if ( checkSuccess ) {
              result.Failes++;
              continue;
            }
            var monitorSpecs = new MonitorSpecs
            {
              ScreenSize = row.Cell( 5 ).GetString()
            };
            monitor.DetailedSpecs = JsonConvert.SerializeObject( monitorSpecs );
            await _assetRepository.CreateAsync( monitor );
            break;

          case 3: // Component
            var component = new Asset
            {
              AssetCode = row.Cell( 2 ).GetString(),
              Name = row.Cell( 3 ).GetString(),
              Brand = row.Cell( 4 ).GetString(),
              Supplier = row.Cell( 6 ).GetString(),
              PurchaseDate = row.Cell( 7 ).GetString() != string.Empty ? row.Cell( 7 ).GetDateTime() : null,
              SerialNumber = row.Cell( 9 ).GetString(),
              Condition = ( int ) Enums.AssetCondition.New,
              Status = ( int ) Enums.AssetStatus.InStorage,
              AssetCategoryId = 7
            };
            checkSuccess = await _assetRepository.Exist( x => x.AssetCode == component.AssetCode );
            if ( checkSuccess ) {
              result.Failes++;
              continue;
            }
            var compSpecs = new ComponentSpecs
            {
              ComponentType = row.Cell( 6 ).GetString()
            };
            component.DetailedSpecs = JsonConvert.SerializeObject( compSpecs );
            component.Quantity = row.Cell( 8 ).GetString() != string.Empty ? row.Cell( 8 ).GetValue<int>() : 1;
            await _assetRepository.CreateAsync( component );
            break;

          case 4: // Electronics         
          case 6: // Others
            var assett = new Asset
            {
              AssetCode = row.Cell( 2 ).GetString(),
              Name = row.Cell( 3 ).GetString(),
              Supplier = row.Cell( 4 ).GetString(),
              PurchaseDate = row.Cell( 5 ).GetString() != string.Empty ? row.Cell( 5 ).GetDateTime() : null,
              Brand = row.Cell( 8 ).GetString(),
              SerialNumber = row.Cell( 9 ).GetString(),
              Condition = ( int ) Enums.AssetCondition.New,
              Status = ( int ) Enums.AssetStatus.InStorage,
              AssetCategoryId = type - 2
            };
            checkSuccess = await _assetRepository.Exist( x => x.AssetCode == assett.AssetCode );
            if ( checkSuccess ) {
              result.Failes++;
              continue;
            }
            assett.Quantity = row.Cell( 6 ).GetString() != string.Empty ? row.Cell( 6 ).GetValue<int>() : 1;
            assett.Notes = row.Cell( 7 ).GetString();
            await _assetRepository.CreateAsync( assett );
            break;
          case 5: // Funiture
            var funiture = new Asset
            {
              AssetCode = row.Cell( 2 ).GetString(),
              Name = row.Cell( 3 ).GetString(),
              Supplier = row.Cell( 4 ).GetString(),
              PurchaseDate = row.Cell( 5 ).GetString() != string.Empty ? row.Cell( 5 ).GetDateTime() : null,
              Condition = ( int ) Enums.AssetCondition.New,
              Status = ( int ) Enums.AssetStatus.InStorage,
              AssetCategoryId = type - 2
            };
            checkSuccess = await _assetRepository.Exist( x => x.AssetCode == funiture.AssetCode );
            if ( checkSuccess ) {
              result.Failes++;
              continue;
            }
            funiture.Quantity = row.Cell( 6 ).GetString() != string.Empty ? row.Cell( 6 ).GetValue<int>() : 1;
            funiture.Notes = row.LastCellUsed().GetString();
            await _assetRepository.CreateAsync( funiture );
            break;
        }
        result.Success++;
      }
      return result;
    }
    private byte [] AddSheet<T>( string sheetName, IEnumerable<T> data, (string Header, Func<T, object?> Selector) [] cols )
    {
      byte [] workbookBytes;
      var pathFileTemp = Path.Combine( _env.WebRootPath, "Excel", "Data.xlsx" );

      using ( MemoryStream templateStream = new() ) {
        using ( FileStream fileStream = new( pathFileTemp, FileMode.Open, FileAccess.Read ) ) {
          fileStream.CopyTo( templateStream );
          templateStream.Position = 0;
        }
        var workbook = new XLWorkbook( templateStream );
        var ws = workbook.Worksheet( "Sheet1" );
        var list = data?.ToList() ?? new List<T>();
        if ( !list.Any() )
          return null;
        // Header
        for ( int i = 0; i < cols.Length; i++ ) {
          ws.Cell( 1, i + 1 ).Value = cols [ i ].Header;
          ws.Cell( 1, i + 1 ).Style.Font.Bold = true;
          ws.Cell( 1, i + 1 ).Style.Fill.BackgroundColor = XLColor.LightGray;
          ws.Cell( 1, i + 1 ).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }
        // Data
        for ( int r = 0; r < list.Count; r++ ) {
          for ( int c = 0; c < cols.Length; c++ ) {
            var val = cols [ c ].Selector( list [ r ] );
            ws.Cell( r + 2, c + 1 ).Value = val ?? string.Empty;
            if ( val is DateTime )
              ws.Cell( r + 2, c + 1 ).Style.DateFormat.Format = "dd/MM/yyyy";
            if ( val is bool  || val is double || val is float )
              ws.Cell( r + 2, c + 1 ).Style.NumberFormat.Format = "#,##0.00";
          }
        }
        // Auto adjust    
        var rng = ws.RangeUsed();
        rng.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rng.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        rng.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        rng.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        ws.SheetView.FreezeRows( 1 );
        ws.Name = sheetName;
        workbook.Save();
        workbookBytes = templateStream.ToArray();
      }
      return workbookBytes;

    }
    public async Task<IEnumerable<UserOption>> GetUserOptions()
    {
      var users = await _userRepository.GetOption();
      // loại user system khỏi danh sách lựa chọn
      return users.ToList().Where( u => u.Id != Enums.SYSTEM_ID ).OrderBy( u => u.Name );
    }
  }
}
