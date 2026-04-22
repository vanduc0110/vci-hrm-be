using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Security.Hashing;
using TTDesign.API.Domain.Security.Tokens;

namespace TTDesign.API.Security.Tokens
{
  public class TokenHandler : ITokenHandler
  {
    private static readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();

    private readonly TokenOptionsConfig _tokenOptions;
    private readonly SigningConfigurations _signingConfigurations;
    private readonly IPasswordHasher _passwordHasher;

    public TokenHandler( IOptions<TokenOptionsConfig> tokenOptionSnapshot, SigningConfigurations signingConfigurations, IPasswordHasher passwordHasher )
    {
      _passwordHasher = passwordHasher;
      _tokenOptions = tokenOptionSnapshot.Value;
      _signingConfigurations = signingConfigurations;
    }

    public AccessToken CreateAccessToken( User user, List<Claim> claims, long? expiration = null )
    {
      // [TODO] (support FE) hardcode for FE test expired access token
      //var refreshToken = BuildRefreshToken();
      var refreshToken = user.Email == "n.s.hiep@ttdesignco.com" ? BuildRefreshTokenTest( expiration ) : BuildRefreshToken( expiration );
      var accessToken = BuildAccessToken( user, refreshToken, claims );
      _refreshTokens.Add( refreshToken );

      return accessToken;
    }

    public RefreshToken TakeRefreshToken( string token )
    {
      if ( string.IsNullOrWhiteSpace( token ) )
        return null!;

      var refreshToken = _refreshTokens.SingleOrDefault( t => t.Token == token );
      if ( refreshToken != null )
        _refreshTokens.Remove( refreshToken );

      return refreshToken!;
    }

    public void RevokeRefreshToken( string token )
    {
      TakeRefreshToken( token );
    }

    private RefreshToken BuildRefreshToken( long? expiration = null )
    {
      var refreshToken = new RefreshToken
      (
          token: _passwordHasher.HashPassword( Guid.NewGuid().ToString() ),
          expiration: ( expiration == null ? DateTime.UtcNow.AddSeconds( _tokenOptions.RefreshTokenExpiration ).Ticks : ( long ) expiration )
      );

      return refreshToken;
    }

    // [TODO] (support FE) hardcode for FE test expired access token
    private RefreshToken BuildRefreshTokenTest( long? expiration = null )
    {
      var refreshToken = new RefreshToken
      (
          token: _passwordHasher.HashPassword( Guid.NewGuid().ToString() ),
          expiration: ( expiration == null ? DateTime.UtcNow.AddSeconds( 3 * 60 * 60 ).Ticks : ( long ) expiration )
      );

      return refreshToken;
    }

    private AccessToken BuildAccessToken( User user, RefreshToken refreshToken, List<Claim> claims )
    {
      // [TODO] (support FE) hardcode for FE test expired access token
      //var accessTokenExpiration = DateTime.UtcNow.AddSeconds( _tokenOptions.AccessTokenExpiration );
      var accessTokenExpiration = user.Email == "n.s.hiep@ttdesignco.com" ? DateTime.UtcNow.AddSeconds( 1 * 60 * 60 ) :
        DateTime.UtcNow.AddSeconds( _tokenOptions.AccessTokenExpiration );

      var securityToken = new JwtSecurityToken
      (
          issuer: _tokenOptions.Issuer,
          audience: _tokenOptions.Audience,
          claims: GetClaims( user, claims ),
          expires: accessTokenExpiration,
          notBefore: DateTime.UtcNow,
          signingCredentials: _signingConfigurations.SigningCredentials
      );

      var handler = new JwtSecurityTokenHandler();
      var accessToken = handler.WriteToken( securityToken );

      return new AccessToken( accessToken, accessTokenExpiration.Ticks, refreshToken );
    }

    private IEnumerable<Claim> GetClaims( User user, List<Claim> userClaims )
    {
      var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Aud, user.UserName),
                new(ClaimTypes.Role, user.Position.ToString()),
                new(Enums.CLAIM_TYPE_TEAM, user.TeamUsers is null || !user.TeamUsers.Any()
                    ? "0"
                    : string.Join(",", user.TeamUsers.Select(x => x.TeamId)))
            };

      foreach ( var userClaim in userClaims ) {
        claims.Add( userClaim );
      }

      return claims;
    }
  }
}
