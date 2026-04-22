using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Security.Hashing;
using TTDesign.API.Domain.Security.Tokens;
using TTDesign.API.Domain.Services;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly IUserService _userService;
    private readonly ITeamService _teamService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenHandler _tokenHandler;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public AuthenticationService( IUserService userService, ITeamService teamService, IPasswordHasher passwordHasher, ITokenHandler tokenHandler, UserManager<User> userManager, RoleManager<Role> roleManager )
    {
      _tokenHandler = tokenHandler;
      _passwordHasher = passwordHasher;
      _userService = userService;
      _teamService = teamService;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public async Task<TokenResponse> CreateAccessTokenAsync( string email, string password )
    {
      var user = await _userManager.FindByEmailAsync( email );
      if ( user == null ) {
        var message = new Dictionary<string, string>() { { nameof( UserCredentialResource.UserName ), ErrorMessageResource.LoginEmailNotFound } };
        return new TokenResponse( false, message, null! );
      }
      if ( !user.IsActive ) {
        var message = new Dictionary<string, string>() { { nameof( UserCredentialResource.UserName ), ErrorMessageResource.LoginEmailInactive } };
        return new TokenResponse( false, message, null! );
      }
      if ( !await _userManager.CheckPasswordAsync( user, password ) ) {
        var message = new Dictionary<string, string>() { { nameof( UserCredentialResource.Password ), ErrorMessageResource.LoginWrongPassword } };
        return new TokenResponse( false, message, null! );
      }
      user.TeamUsers = ( await _teamService.GetTeamUserByUserId( user.Id ) )!;
      var roleStr = await _userManager.GetRolesAsync( user );
      var claims = roleStr is null || roleStr.Count == 0 ? new List<Claim>() :
        ( List<Claim> ) await _roleManager.GetClaimsAsync( await _roleManager.FindByNameAsync( roleStr [ 0 ] ) );
      var token = _tokenHandler.CreateAccessToken( user, claims );

      return new TokenResponse( true, null!, token );
    }

    public async Task<TokenResponse> RefreshTokenAsync( string refreshToken, string userEmail )
    {
      var token = _tokenHandler.TakeRefreshToken( refreshToken );

      if ( token == null ) {
        var message = new Dictionary<string, string>() { { Enums.ERROR_TEXT, MessageContents.AUTH_REFRESH_INVALID } };
        return new TokenResponse( false, message, null! );
      }

      if ( token.IsExpired() ) {
        var message = new Dictionary<string, string>() { { Enums.ERROR_TEXT, MessageContents.AUTH_REFRESH_EXPIRED } };
        return new TokenResponse( false, message, null! );
      }

      var user = await _userManager.FindByEmailAsync( userEmail );
      if ( user == null || !user.IsActive ) {
        var message = new Dictionary<string, string>() { { Enums.ERROR_TEXT, MessageContents.AUTH_REFRESH_INVALID } };
        return new TokenResponse( false, message, null! );
      }
      var roleStr = await _userManager.GetRolesAsync( user );
      var claims = roleStr is null || roleStr.Count == 0 ? new List<Claim>() :
        ( List<Claim> ) await _roleManager.GetClaimsAsync( await _roleManager.FindByNameAsync( roleStr [ 0 ] ) );
      var accessToken = _tokenHandler.CreateAccessToken( user, claims, token.Expiration );
      return new TokenResponse( true, null!, accessToken );
    }

    public void RevokeRefreshToken( string refreshToken )
    {
      _tokenHandler.RevokeRefreshToken( refreshToken );
    }

    public async Task<IEnumerable<Claim>> GetClaims( string email )
    {
      var user = await _userManager.FindByEmailAsync( email );
      var roleStr = await _userManager.GetRolesAsync( user );
      return roleStr is null || roleStr.Count == 0 ? new List<Claim>() :
        await _roleManager.GetClaimsAsync( await _roleManager.FindByNameAsync( roleStr [ 0 ] ) );
    }
  }
}
