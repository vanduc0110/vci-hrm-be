using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Security.Tokens;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers
{
  /// <summary>
  /// API liên quan tới login
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    private readonly ITokenHandler _tokenHandler;
    private readonly ITeamService _teamService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

    public AuthController( IAuthenticationService authenticationService,
      IMapper mapper,
      IUserService userService,
      ITokenHandler tokenHandler,
      ILogger<AuthController> logger,
      IOptions<ApiBehaviorOptions> apiBehaviorOptions,
      RoleManager<Role> roleManager,
      UserManager<User> userManager,
      ITeamService teamService )
    {
      _userService = userService;
      _mapper = mapper;
      _authenticationService = authenticationService;
      _tokenHandler = tokenHandler;
      _logger = logger;
      _userManager = userManager;
      _apiBehaviorOptions = apiBehaviorOptions;
      _roleManager = roleManager;
      _teamService = teamService;
    }

    [Route( "/api/login" )]
    [HttpPost]
    public async Task<IActionResult> LoginAsync( [FromBody] UserCredentialResource userCredentials )
    {
      if ( !ModelState.IsValid ) {
        return BadRequest( ModelState.GetErrorMessages() );
      }

      var response = await _authenticationService.CreateAccessTokenAsync( userCredentials.UserName, userCredentials.Password );
      if ( !response.Success ) {
        foreach ( var error in response.MessageValid ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }

      var accessTokenResource = _mapper.Map<AccessToken, AccessTokenResponse>( response.Token );
      accessTokenResource.EnviromentConfig = Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" ) ?? "none";
      return Ok( accessTokenResource );
    }

    [Route( "/api/LoginByMicrosoft365Account" )]
    [HttpPost]
    public async Task<IActionResult> LoginByMicrosoft365AccountAsync( string token )
    {
      JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
      JwtSecurityToken jsonToken = handler.ReadJwtToken( token );
      var claims = jsonToken.Claims;
      var tid = claims.FirstOrDefault( c => c.Type == "tid" );
      if ( tid == null || string.IsNullOrEmpty( tid.Value ) || !tid.Value.Contains( Enums.AZURE_TENANT_ID ) ) {
        return BadRequest( ErrorMessageResource.ErrorTenant );
      }

      var nbf = claims.FirstOrDefault( c => c.Type == "nbf" );
      var exp = claims.FirstOrDefault( c => c.Type == "exp" );
      if ( nbf == null || string.IsNullOrEmpty( nbf.Value ) )
        return BadRequest( ErrorMessageResource.Nbf );
      if ( exp == null || string.IsNullOrEmpty( exp.Value ) )
        return BadRequest( ErrorMessageResource.Exp );

      // unixタイム変換
      DateTime nbfDateTime = DateTimeOffset.FromUnixTimeSeconds( long.Parse( nbf.Value ) ).UtcDateTime;
      DateTime expDateTime = DateTimeOffset.FromUnixTimeSeconds( long.Parse( exp.Value ) ).UtcDateTime;

      if ( DateTime.UtcNow < nbfDateTime || DateTime.UtcNow > expDateTime )
        return BadRequest( ErrorMessageResource.VerifiedTokenError );

      var email = claims.FirstOrDefault( c => c.Type == "unique_name" );
      var user = await _userManager.FindByEmailAsync( email!.Value );
      user.TeamUsers = ( await _teamService.GetTeamUserByUserId( user.Id ) )!;
      if ( user == null || user.IsActive ) {
        return BadRequest( ErrorMessageResource.LoginEmailNotFound );
      }
      var roleStr = await _userManager.GetRolesAsync( user );
      var claimSources = roleStr is null || roleStr.Count == 0 ? new List<Claim>() :
        ( List<Claim> ) await _roleManager.GetClaimsAsync( await _roleManager.FindByNameAsync( roleStr [ 0 ] ) );
      var accessToken = _tokenHandler.CreateAccessToken( user, claimSources.ToList() );
      var accessTokenResource = _mapper.Map<AccessToken, AccessTokenResponse>( accessToken );
      return Ok( accessTokenResource );
    }
    [Route( "/api/token/refresh" )]
    [HttpPost]
    public async Task<IActionResult> RefreshTokenAsync( [FromBody] RefreshTokenResource refreshTokenResource )
    {
      if ( !ModelState.IsValid ) {
        return BadRequest( ModelState.GetErrorMessages() );
      }

      var response = await _authenticationService.RefreshTokenAsync( refreshTokenResource.Token, refreshTokenResource.UserEmail );
      if ( !response.Success ) {
        foreach ( var error in response.MessageValid ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }

      var tokenResource = _mapper.Map<AccessToken, AccessTokenResponse>( response.Token );
      return Ok( tokenResource );
    }

    [Route( "/api/token/revoke" )]
    [HttpPost]
    public IActionResult RevokeToken( [FromBody] RevokeTokenResource revokeTokenResource )
    {
      if ( !ModelState.IsValid ) {
        return BadRequest( ModelState.GetErrorMessages() );
      }

      _authenticationService.RevokeRefreshToken( revokeTokenResource.Token );
      return NoContent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email" example="user@ttdesignco.com">email của user request</param>
    /// <returns></returns>
    [Route( "/api/RequestResetPassword" )]
    [HttpPost]
    public async Task<IActionResult> RequestResetPassword( [FromBody] string email )
    {
      var user = await _userManager.FindByEmailAsync( email );
      if ( user == null ) {
        return BadRequest( ErrorMessageResource.LoginEmailNotFound );
      }
      if ( user == null || !user.IsActive)  {
        return BadRequest( ErrorMessageResource.LoginEmailInactive );
      }
      await _userService.RequestResetPassword( user );
      return NoContent();
    }

    /// <summary>
    /// lấy danh sách quyền truy cập của user login tới các menu và tab menu
    /// </summary>
    /// <returns></returns>
    [HttpGet( "GetMenuAccess" )]
    [Authorize]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( MenuAccess ) )]
    public async Task<IActionResult> GetMenuAccess()
    {
      var email = HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Email )!.Value;
      var claims = await _authenticationService.GetClaims( email );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var menuAccess = new MenuAccess()
      {
        Claims = claims.ToList(),
        TeamHR = parts.Any( x => x == Enums.TEAM_HR.ToString() ),
      };
      return Ok( menuAccess );
    }
  }
}
