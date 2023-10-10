
using DemoRedisCache.Models.DTO;
using System.Collections;
using System.Security.Claims;

namespace DemoRedisCache.AuthenticationServer.Abstract
{
    public interface ITokenService
    {
        TokenResponse GetToken(IEnumerable<Claim> claim);

        string GetRefreshToken();

        ClaimsPrincipal GetPrincipalFormExpiredToken(string token);
    }
}
