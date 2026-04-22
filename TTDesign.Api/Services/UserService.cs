using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Security.Hashing;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class UserService : GenericService<User>, IUserService
  {
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserInfoRepository _userInfoRepository;
    private readonly IUserSettingRepository _userSettingRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamUserRepository _teamUserRepository;
    private readonly ISystemRequestRepository _systemRequestRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly INotificationService _notificationService;

    public UserService( IUserRepository userRepository,
      ILogger<UserService> logger,
      IMapper mapper,
      IRoleRepository roleRepository,
      IUserInfoRepository userInfoRepository,
      IUserSettingRepository userSettingRepository,
      ITeamUserRepository teamUserRepository,
      ITeamRepository teamRepository,
      ISystemRequestRepository systemRequestRepository,
      UserManager<User> userManager, RoleManager<Role> roleManager,
      INotificationService notificationService,
      IPasswordHasher passwordHasher ) : base( userRepository )
    {
      _userRepository = userRepository;
      _roleRepository = roleRepository;
      _logger = logger;
      _mapper = mapper;
      _userInfoRepository = userInfoRepository;
      _userSettingRepository = userSettingRepository;
      _teamUserRepository = teamUserRepository;
      _passwordHasher = passwordHasher;
      _roleManager = roleManager;
      _userManager = userManager;
      _teamRepository = teamRepository;
      _systemRequestRepository = systemRequestRepository;
      _notificationService = notificationService;
    }

    #region BaseServiceOption
    public async Task<IEnumerable<UserOption>> GetOption( long? teamId = null )
    {
      var users = await _userRepository.GetOption();
      // loại user system khỏi danh sách lựa chọn
      return users.ToList().Where( u => u.Id != Enums.SYSTEM_ID ).OrderBy( u => u.Name );
    }
    #endregion

    #region BaseServiceResource
    public async Task<long> Create( UserResource resource, long creator )
    {
      var user = _mapper.Map<User>( resource );
      // validate user name duplicate
      user.UserName = await GenerateUserName( user.UserName );
      user.Avatar = "";
      user.CreatedBy = creator;
      user.ModifiedBy = creator;
      var listTeamUser = new List<TeamUser>();
      foreach ( var teamId in resource.TeamIds ) {
        listTeamUser.Add( new TeamUser() { TeamId = teamId } );
      }
      user.TeamUsers = listTeamUser;
      // create user
      var result = await _userManager.CreateAsync( user, Enums.PASS_INITIAL );
      if ( !result.Succeeded ) {
        throw new Exception( result.Errors.ToString() );
      }
      // add role
      await _userManager.AddToRoleAsync( user, resource.Role );
      // check role update user
      var claims = ( List<System.Security.Claims.Claim> ) await _roleManager.GetClaimsAsync( await _roleManager.FindByNameAsync( resource.Role ) );
      if ( claims.Exists( c => c.Type == Roles.ROLE_ADMIN_USER && c.Value == Roles.PERMISSION_UPDATE ) ) {
        await _userManager.AddClaimAsync( user, new System.Security.Claims.Claim( Roles.ROLE_ADMIN_USER, Roles.PERMISSION_UPDATE ) );
      }
      // add user info
      var userInfo = _mapper.Map<UserInfo>( resource );
      userInfo.UserId = user.Id;
      await _userInfoRepository.CreateAsync( userInfo );
      // add user setting
      CreateDefaultSetting( user.Id );
      // update amount of team
      _teamRepository.UpdateAmountOfTeam();
      // create system request
      await _systemRequestRepository.CreateAsync( new SystemRequest
      {
        UserId = user.Id,
        Type = ( int ) Enums.SystemRequestType.ActiveUser,
        Date = DateTime.UtcNow.Date,
        ObjectId = user.Id,
      } );
      // create noti welcome
      var notificationData = new Dictionary<string, string>() {
        { "Title", "Welcome" },
        { "Content", $"Welcome VCI JSC! Hard work, play hard!" },
        { "ObjectId", ((int)Enums.NotificationObjectId.WelcomeNewStaff).ToString() },
        { "UserName", user.UserName },
        { "CreatedBy", user.Id.ToString() },
        { "To", user.Id.ToString() }
      };
      await _notificationService.Create( ( int ) Enums.NotificationObjectType.Notification, notificationData );
      return user.Id;
    }

    public async Task Update( UserResource resource, long editor )
    {
      var old = await _userRepository.GetByCondition( u => u.Id == resource.Id );
      var user = _mapper.Map<User>( resource );
      // validate user name duplicate
      if ( user.UserName != old!.UserName ) {
        user.UserName = await GenerateUserName( user.UserName );
      }
      var oldPosition = old.Position;
      var newPosition = resource.Position;
      old.UserName = user.UserName;
      old.FullName = user.FullName;
      old.DateStartWork = user.DateStartWork;
      old.StaffId = user.StaffId;
      old.Position = user.Position;
      old.ModifiedBy = editor;
      old.Email = user.Email;
      // update team user
      var teamUser = await _userRepository.GetUserDataByCondition( u => u.Id == resource.Id );
      await _teamUserRepository.DeleteByCondition( t => t.UserId == resource.Id );
      foreach ( var teamId in resource.TeamIds ) {

        old.TeamUsers.Add( new TeamUser() { UserId = user.Id, TeamId = teamId } );
      }
      // update user
      var result = await _userManager.UpdateAsync( old );
      if ( !result.Succeeded ) {
        throw new Exception( result.Errors.ToString() );
      }
      // update role
      var roles = await _userManager.GetRolesAsync( old );
      if ( roles [ 0 ] != resource.Role ) {
        await _userManager.RemoveFromRoleAsync( old, roles [ 0 ] );
        await _userManager.AddToRoleAsync( old, resource.Role );
        // check role update user
        var claims = ( List<System.Security.Claims.Claim> ) await _roleManager.GetClaimsAsync( await _roleManager.FindByNameAsync( resource.Role ) );
        if ( claims.Exists( c => c.Type == Roles.ROLE_ADMIN_USER && c.Value == Roles.PERMISSION_UPDATE ) ) {
          var userIdentity = await _userManager.FindByEmailAsync( old.Email );
          var userClaims = await _userManager.GetClaimsAsync( userIdentity );
          if ( !userClaims.Any( c => c.Type == Roles.ROLE_ADMIN_USER && c.Value == Roles.PERMISSION_UPDATE ) ) {
            await _userManager.AddClaimAsync( userIdentity, new System.Security.Claims.Claim( Roles.ROLE_ADMIN_USER, Roles.PERMISSION_UPDATE ) );
          }
        }
        else {
          var userIdentity = await _userManager.FindByEmailAsync( old.Email );
          var userClaims = await _userManager.GetClaimsAsync( userIdentity );
          var userClaim = userClaims.FirstOrDefault( c => c.Type == Roles.ROLE_ADMIN_USER && c.Value == Roles.PERMISSION_UPDATE );
          if ( userClaim != null ) {
            await _userManager.RemoveClaimAsync( userIdentity, userClaim );
          }
        }
      }

      if ( oldPosition == ( int ) Enums.UserPosition.Probationary && newPosition == Enums.UserPosition.Official.ToString() ) {
        // create system request
        await _systemRequestRepository.CreateAsync( new SystemRequest
        {
          UserId = user.Id,
          Type = ( int ) Enums.SystemRequestType.DefineAnnualLeaveToOffical,
          Date = DateTime.UtcNow.Date,
          ObjectId = user.Id,
        } );
      }

      // update user info
      var oldUserInfo = await _userInfoRepository.GetByCondition( u => u.UserId == resource.Id );
      _mapper.Map( resource, oldUserInfo );
      _userInfoRepository.Update( oldUserInfo! );
      // update amount of team
      if ( !teamUser!.TeamUsers.Select( t => t.TeamId ).Any( t => resource.TeamIds.Contains( t ) ) )
        _teamRepository.UpdateAmountOfTeam();
    }
    #endregion

    #region BaseServiceList
    public async Task<IEnumerable<UserResponse>> GetList( BaseFilter filter )
    {
      var users = await _userRepository.GetUsersDataByCondition( filter.TeamId );
      // loại user system khỏi danh sách lựa chọn
      users = users.ToList().Where( u => u.Id != Enums.SYSTEM_ID ).OrderBy( u => u.UserName );
      var userResponse = _mapper.Map<IEnumerable<UserResponse>>( users );
      var listUserModified = new Dictionary<long, string>();
      foreach ( var user in userResponse ) {
        var userIdentity = await _userManager.FindByIdAsync( user.Id.ToString() );
        var roles = await _userManager.GetRolesAsync( userIdentity );
        var role = await _roleRepository.GetByConditionNoTrack( r => r.Name == roles [ 0 ] );
        user.RoleId = role!.Id;
        user.Role = role.Type == ( int ) Enums.RoleType.Default ? $"(Def) {role.Name}" : $"(Cus) {role.Name}";
        // set modifier
        if ( !listUserModified.ContainsKey( user.ModifiedBy ) ) {
          var findUser = await _userRepository.GetByConditionNoTrack( u => u.Id == user.ModifiedBy );
          if ( findUser != null ) {
            listUserModified.Add( findUser.Id, findUser.UserName );
            user.ModifierName = findUser.UserName;
          }
        }
        else {
          user.ModifierName = listUserModified [ user.ModifiedBy ];
        }
        // set teamuser info
        var listUserTeam = new List<TeamUserOption>();
        var teamUsers = await _teamUserRepository.GetListByConditionTrack( t => t.UserId == user.Id );
        foreach ( var teamUser in teamUsers ) {
          var teamUserOption = new TeamUserOption();
          var team = await _teamRepository.GetByConditionNoTrack( t => t.Id == teamUser.TeamId );
          if ( team != null ) {
            teamUserOption.TeamId = team.Id;
            teamUserOption.TeamCode = team.Code;
            teamUserOption.TeamName = team.Name;
            listUserTeam.Add( teamUserOption );
          }
        }
        user.Teams = listUserTeam;
      }
      return userResponse;
    }
    #endregion

    #region BaseServiceDetail
    public async Task<UserDetailResponse?> GetDetail( long id )
    {
      var user = await _userRepository.GetUserDataByCondition( u => u.Id == id && u.IsActive );
      if ( user == null ) {
        return null;
      }
      var userResponse = _mapper.Map<UserDetailResponse>( user );
      // set fields info
      var info = await _userInfoRepository.GetByConditionNoTrack( u => u.UserId == id );
      _mapper.Map( info!, userResponse );
      // set role
      var roles = await _userManager.GetRolesAsync( user );
      var role = await _roleRepository.GetByConditionNoTrack( r => r.Name == roles [ 0 ] );
      userResponse.RoleId = role!.Id;
      userResponse.Role = role.Type == ( int ) Enums.RoleType.Default ? $"(Def) {role.Name}" : $"(Cus) {role.Name}";
      return userResponse;
    }
    #endregion

    #region Others
    public async Task UpdateSetting( UserSettingResource resource )
    {
      var userSetting = _mapper.Map<UserSetting>( resource );
      if ( await _userSettingRepository.Exist( u => u.Id == userSetting.Id ) )
        _userSettingRepository.Update( userSetting );
      else
        await _userSettingRepository.CreateAsync( userSetting );
    }

    public async Task<UserSettingResponse> GetSetting( long id )
    {
      var userSetting = await _userSettingRepository.GetByCondition( u => u.Id == id );
      return _mapper.Map<UserSettingResponse>( userSetting );
    }

    /// <summary>
    /// Khởi tạo User Setting default
    /// </summary>
    /// <param name="id">User ID</param>
    private void CreateDefaultSetting( long id )
    {
      var userSetting = new UserSetting() { Id = id };
      _userSettingRepository.CreateAsync( userSetting );
    }

    public async Task<bool> ChangePassword( UserAndPassResource resource, long modifier )
    {
      var oldUser = await _userRepository.GetByCondition( u => u.Id == modifier );
      await _userManager.RemovePasswordAsync( oldUser! );
      await _userManager.AddPasswordAsync( oldUser!, resource.NewPassword );
      oldUser.ModifiedBy = modifier;
      await _userManager.UpdateAsync( oldUser );
      return true;
    }

    public async Task<string> ResetPassword( long userId, long modifier )
    {
      var oldUser = await _userRepository.GetByCondition( u => u.Id == userId );
      await _userManager.RemovePasswordAsync( oldUser! );
      await _userManager.AddPasswordAsync( oldUser!, Enums.PASS_INITIAL );
      // delete noti
      var noti = await _notificationService.GetByCondition( n => n.CreatedBy == userId && n.ObjectType == ( int ) Enums.NotificationObjectType.Notification && n.ObjectId == ( int ) Enums.NotificationObjectId.RequestResetPassword );
      if ( noti is not null ) {
        await _notificationService.Delete( noti.Id, null );
        await _notificationService.DeleteByCondition( n => n.Id == noti.Id );
      }
      return string.Empty;
    }

    public async Task<bool> ChangeStatus( long userId, long modifier )
    {
      var user = await _userRepository.GetByCondition( u => u.Id == userId );
      user!.IsActive = !user.IsActive;
      user.ModifiedBy = modifier;
      _userRepository.Update( user );
      // create system request
      await _systemRequestRepository.CreateAsync( new SystemRequest
      {
        UserId = userId,
        Type = user.IsActive ? ( int ) Enums.SystemRequestType.ActiveUser : ( int ) Enums.SystemRequestType.InactiveUser,
        Date = DateTime.UtcNow.Date.AddDays( 1 ),
      } );
      return true;
    }

    public async Task<bool> UserExist( long userId )
    {
      return await _userRepository.Exist( u => u.Id == userId );
    }

    public async Task<Role> GetRole( string roleName )
    {
      return await _roleManager.FindByNameAsync( roleName );
    }

    public async Task<bool> UpdateAvatar( long id, string fileName )
    {
      var oldUser = await _userRepository.GetByCondition( u => u.Id == id );
      oldUser!.Avatar = fileName;
      var result = await _userManager.UpdateAsync( oldUser );
      if ( !result.Succeeded ) {
        throw new Exception( result.Errors.ToString() );
      }
      return true;
    }

    public async Task<bool> ChangeState( long id, int state )
    {
      var oldUser = await _userRepository.GetByCondition( u => u.Id == id );
      oldUser!.State = state;
      var result = await _userManager.UpdateAsync( oldUser );
      if ( !result.Succeeded ) {
        throw new Exception( result.Errors.ToString() );
      }
      return true;
    }

    private async Task<string> GenerateUserName( string userName )
    {
      var userNameExit = await _userRepository.FindLastUserHasName( userName );
      if ( userNameExit != null ) {
        var elementName = userNameExit.UserName.Split( "." );
        int index;
        return userName + "." + ( int.TryParse( elementName.Last(), out index ) ? index + 1 : Enums.START_NO_USER );
      }
      return userName;
    }

    public async Task<List<DynamicOption>> GetDynamicOption( long? teamId )
    {
      var options = new List<DynamicOption>();
      // add option team
      var teams = await _teamRepository.GetOption( teamId );
      if ( teams.Count() > 0 ) {
        foreach ( var item in teams ) {
          options.Add( new DynamicOption()
          {
            Id = item.Id,
            Name = $"(T) {item.Name}",
            Type = Enum.GetName( Enums.DynamicOption.Team )!
          } );
        }
      }
      // add option user
      if ( teamId == null ) {
        var users = ( await _userRepository.GetOption() ).OrderBy( u => u.Name );
        foreach ( var user in users ) {
          options.Add( new DynamicOption()
          {
            Id = user.Id,
            Name = $"(U) {user.Name}",
            Type = Enum.GetName( Enums.DynamicOption.User )!
          } );
        }
      }
      else {
        var users = ( await _teamRepository.GetTeamUser( ( long ) teamId ) ).OrderBy( u => u.UserName );
        foreach ( var user in users ) {
          options.Add( new DynamicOption()
          {
            Id = user.Id,
            Name = $"(U) {user.UserName}",
            Type = Enum.GetName( Enums.DynamicOption.User )!
          } );
        }
      }
      //// add option group
      //var groups = await _groupRepository.GetOption();
      //if ( groups.Count() > 0 ) {
      //  foreach ( var item in groups ) {
      //    options.Add( new DynamicOption()
      //    {
      //      Id = item.Id,
      //      Name = $"(G) {item.Name}",
      //      Type = Enum.GetName( Enums.DynamicOption.Group )!
      //    } );
      //  }
      //}
      //// add option position
      //if ( teamId == null ) {
      //  options.Add( new DynamicOption()
      //  {
      //    Id = ( long ) Enums.UserPosition.GM,
      //    Name = $"(P) {Enum.GetName( Enums.UserPosition.GM )!}",
      //    Type = Enum.GetName( Enums.DynamicOption.Position )!
      //  } );
      //  options.Add( new DynamicOption()
      //  {
      //    Id = ( long ) Enums.UserPosition.Leader,
      //    Name = $"(P) {Enum.GetName( Enums.UserPosition.Leader )!}",
      //    Type = Enum.GetName( Enums.DynamicOption.Position )!
      //  } );
      //  options.Add( new DynamicOption()
      //  {
      //    Id = ( long ) Enums.UserPosition.SubLeader,
      //    Name = $"(P) {Enum.GetName( Enums.UserPosition.SubLeader )!}",
      //    Type = Enum.GetName( Enums.DynamicOption.Position )!
      //  } );
      //  options.Add( new DynamicOption()
      //  {
      //    Id = ( long ) Enums.UserPosition.Official,
      //    Name = $"(P) {Enum.GetName( Enums.UserPosition.Official )!}",
      //    Type = Enum.GetName( Enums.DynamicOption.Position )!
      //  } );
      //  options.Add( new DynamicOption()
      //  {
      //    Id = ( long ) Enums.UserPosition.Intership,
      //    Name = $"(P) {Enum.GetName( Enums.UserPosition.Intership )!}",
      //    Type = Enum.GetName( Enums.DynamicOption.Position )!
      //  } );
      //}
      return options;
    }

    public async Task<IEnumerable<DashboardUser>> GetDashboardUser()
    {
      return await _userRepository.GetDashboardUser();
    }

    public async Task<DetailOtherUser?> GetDetailOtherUser( long id )
    {
      var user = await _userRepository.GetUserDataByCondition( u => u.Id == id );
      if ( user == null ) {
        return null;
      }
      var userInfo = await _userInfoRepository.GetByConditionNoTrack( u => u.UserId == id );
      return new DetailOtherUser()
      {
        Id = user.Id,
        FullName = user.FullName,
        UserName = user.UserName,
        DateOfBirth = userInfo!.Birthday,
        Phone = userInfo!.PhoneNumber,
        Teams = user.TeamUsers.Select( t => new TeamUserOption()
        {
          TeamId = t.TeamId,
          TeamCode = t.Team!.Code,
          TeamName = t.Team!.Name
        } ).ToList(),
        Position = Enum.GetName( ( Enums.UserPosition ) user.Position )!,
        Email = user.Email,
        Address = userInfo.Address,
        About = userInfo.AboutMe,
        Avatar = user.Avatar
      };
    }

    public async Task RequestResetPassword( User user )
    {
      if ( await _notificationService.Exist( n => n.CreatedBy == user.Id && n.ObjectType == ( int ) Enums.NotificationObjectType.Notification && n.ObjectId == ( int ) Enums.NotificationObjectId.RequestResetPassword ) )
        return;
      // get list user had claim update user
      var users = await _userManager.GetUsersForClaimAsync( new System.Security.Claims.Claim( Roles.ROLE_ADMIN_USER, Roles.PERMISSION_UPDATE ) );
      // tạo noti yêu cầu tới user admin
      var notificationData = new Dictionary<string, string>() {
        { "Title", "Request Reset Password" },
        { "Content", $"User ({user.UserName} - {user.Email}) have request reset password" },
        { "ObjectId", ((int)Enums.NotificationObjectId.RequestResetPassword).ToString() },
        { "UserName", user.UserName },
        { "CreatedBy", user.Id.ToString() }
      };
      if ( users.Count() > 0 ) {
        var teamUser = await _teamUserRepository.GetByConditionNoTrack( t => t.UserId == user.Id );
        var listUserId = new List<long>();
        foreach ( var u in users ) {
          if ( Common.ValidRoleAdmin( u.Position ) ) {
            listUserId.Add( u.Id );
          }
          else {
            var team = await _teamUserRepository.GetByConditionNoTrack( t => t.UserId == u.Id );
            if ( team!.TeamId == teamUser!.TeamId ) {
              listUserId.Add( u.Id );
            }
          }
        }
        if ( listUserId.Count() > 0 ) {
          notificationData.Add( "To", string.Join( ",", listUserId ) );
        }
      }
      await _notificationService.Create( ( int ) Enums.NotificationObjectType.Notification, notificationData );
    }

    public async Task SelfUpdate( YourSelfResource resource )
    {
      // update user info
      var oldUserInfo = await _userInfoRepository.GetByCondition( u => u.UserId == resource.Id );
      _mapper.Map( resource, oldUserInfo );
      _userInfoRepository.Update( oldUserInfo! );
    }

    public async Task<IEnumerable<UserOption>> GetUserByPosition( long position )
    {
      var users = await _userRepository.GetOption();
      return users.ToList().Where( u => u.Id != Enums.SYSTEM_ID && ( u.Position == Enum.GetName( ( Enums.UserPosition ) position )
        || u.Position == Enums.UserPosition.TeamLead.ToString()
        || u.Position == Enums.UserPosition.SubLead.ToString() ) ).OrderBy( u => u.Name );
    }
    #endregion
  }
}
