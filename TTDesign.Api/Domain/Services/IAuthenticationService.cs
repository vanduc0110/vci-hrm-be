using System.Security.Claims;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IAuthenticationService
  {
    Task<TokenResponse> CreateAccessTokenAsync( string username, string password );
    Task<TokenResponse> RefreshTokenAsync( string refreshToken, string username );
    void RevokeRefreshToken( string refreshToken );
    /// <summary>
    /// Danh sách claim của user
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<IEnumerable<Claim>> GetClaims( string email );
  }
}
