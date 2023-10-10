using DemoRedisCache.AuthenticationServer.Abstract;
using DemoRedisCache.Datas;
using DemoRedisCache.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoRedisCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly MyDBContext _dbContext;
        private readonly ITokenService _tokenService;

        public TokenController(MyDBContext dbContext, ITokenService tokenService)
        {
            this._dbContext = dbContext;
            this._tokenService = tokenService;
        }

        [HttpPost]
        [Route("/Refreshtoken")]
        public IActionResult Refresh(RefreshTokenRequest tokenRequest)
        {
            if (tokenRequest is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = tokenRequest.AccessToken;
            string refreshToken = tokenRequest.RefreshToken;
            var principal = _tokenService.GetPrincipalFormExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = _dbContext.TokenInfos.SingleOrDefault(u => u.Usename == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }
            var newAccessToken = _tokenService.GetToken(principal.Claims);
            var newRefreshToken = _tokenService.GetRefreshToken();
            user.RefreshToken = newRefreshToken;
            _dbContext.SaveChanges();
            return Ok(new RefreshTokenRequest()
            {
                AccessToken = newAccessToken.TokenString,
                RefreshToken = newRefreshToken
            });

        }

        [HttpPost, Authorize]
        [Route("/revoke")]
        public IActionResult Revoke()
        {
            try
            {
                var username = User.Identity.Name;
                var user = _dbContext.TokenInfos.SingleOrDefault(u => u.Usename == username);
                if (user is null)
                    return BadRequest();
                user.RefreshToken = null;
                _dbContext.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }



    }
}
