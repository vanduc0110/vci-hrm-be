using System.Security.Claims;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Security.Tokens
{
    public interface ITokenHandler
    {
        AccessToken CreateAccessToken(User user, List<Claim> claims, long? expiration = null );
        RefreshToken TakeRefreshToken(string token);
        void RevokeRefreshToken(string token);
    }
}
