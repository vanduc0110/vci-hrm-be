using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class RoleService : GenericService<Role>, IRoleService
  {
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<RoleService> _logger;
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public RoleService( IRoleRepository roleRepository,
      ILogger<RoleService> logger, RoleManager<Role> roleManager, UserManager<User> userManager, IUserRepository userRepository,
      IMapper mapper ) : base( roleRepository )
    {
      _roleRepository = roleRepository;
      _logger = logger;
      _mapper = mapper;
      _roleManager = roleManager;
      _userManager = userManager;
      _userRepository = userRepository;
    }

    #region BaseServiceList
    public async Task<IEnumerable<RoleResponse>> GetList( BaseFilter filter )
    {
      var roles = await _roleRepository.GetAll();
      var result = _mapper.Map<IEnumerable<RoleResponse>>( roles );
      var modifier = new Dictionary<long, string>() { };
      foreach ( var role in result ) {
        if ( !modifier.ContainsKey( role.ModifiedBy ) ) {
          var user = await _userRepository.GetByConditionNoTrack( u => u.Id == role.ModifiedBy );
          if ( user != null ) {
            modifier.Add( role.ModifiedBy, user.UserName );
            role.ModifierName = user.UserName;
          }
        }
        else {
          role.ModifierName = modifier [ role.ModifiedBy ];
        }
      }
      return result;
    }
    #endregion

    #region BaseServiceDetail
    public async Task<RoleDetailResponse?> GetDetail( long id )
    {
      var role = await _roleRepository.GetByConditionNoTrack( t => t.Id == id );
      if ( role == null )
        return null;
      var claims = await _roleManager.GetClaimsAsync( await _roleManager.FindByNameAsync( role.Name ) );
      var claimKey = claims.Select( c => c.Type + "." + c.Value );
      var collectionRole = Common.CollectionRole();
      foreach ( var roleModels in collectionRole.Values ) {
        foreach ( var roleModel in roleModels ) {
          if ( claimKey.Contains( roleModel.Type + "." + roleModel.Value ) ) {
            roleModel.IsActive = true;
          }
        }
      }
      return new RoleDetailResponse()
      {
        Id = role.Id,
        Name = role.Name,
        Roles = _mapper.Map<Dictionary<string, RoleModelView []>>( collectionRole )
      };
    }
    #endregion

    #region BaseServiceResource
    public async Task<long> Create( RoleResource resource, long creator )
    {
      // create role
      var role = new Role
      {
        Name = resource.Name,
        CreatedBy = creator,
        ModifiedBy = creator,
        Type = ( int ) Enums.RoleType.Custom,
      };
      await _roleManager.CreateAsync( role );
      // create role claim
      foreach ( var menu in resource.Roles ) {
        foreach ( var record in menu.Value ) {
          if ( record.IsActive ) {
            await _roleManager.AddClaimAsync( role, new Claim( record.Type, record.Name ) );
          }
        }
      }
      return role.Id;
    }

    public async Task Update( RoleResource resource, long editor )
    {
      var role = await _roleRepository.GetByCondition( r => r.Id == resource.Id );
      role!.Name = resource.Name;
      role.ModifiedBy = editor;
      await _roleManager.UpdateAsync( role );
      // valid: so sánh 2 bộ claim old và update
      var claims = await _roleManager.GetClaimsAsync( await _roleManager.FindByNameAsync( role.Name ) );
      var oldClaims = claims.Select( c => c.Type + "." + c.Value ).ToHashSet();
      foreach ( var menu in resource.Roles ) {
        foreach ( var record in menu.Value ) {
          if ( record.IsActive ) {
            var key = record.Type + "." + record.Name;
            if ( !oldClaims.Contains( key ) )
              await _roleManager.AddClaimAsync( role, new Claim( record.Type, record.Name ) );
            else
              oldClaims.Remove( key );
          }
        }
      }
      if ( oldClaims.Count > 0 ) {
        foreach ( var key in oldClaims ) {
          var type = key.Split( '.' ) [ 0 ];
          var value = key.Split( '.' ) [ 1 ];
          var claim = claims.Where( c => c.Type == type && c.Value == value ).First();
          if ( claim != null ) {
            await _roleManager.RemoveClaimAsync( role, claim );
          }
        }
      }
    }
    #endregion

    #region BaseServiceOption
    public async Task<IEnumerable<RoleOption>> GetOption( long? position = null )
    {
      var roles = await _roleManager.Roles.ToListAsync();
      return roles.Where( r => r.Name != Roles.ROLE_NAME_SYSTEM ).Select( r =>
        new RoleOption() { Id = r.Id, Name = r.Type == ( int ) Enums.RoleType.Default ? $"(Def) {r.Name}" : $"(Cus) {r.Name}" }
      );
    }
    #endregion
  }
}
