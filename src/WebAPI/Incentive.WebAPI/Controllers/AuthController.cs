using System.Threading.Tasks;
using Incentive.Application.Common.Models;
using Incentive.Ports.Services;
using Incentive.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incentive.WebAPI.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Login(LoginRequest request)
        {
            var result = await _identityService.LoginAsync(request.UserName, request.Password);

            if (!result.Succeeded)
            {
                return Unauthorized(BaseResponse<LoginResponse>.Failure("Invalid username or password"));
            }

            var response = new LoginResponse
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                Expiration = result.Expiration
            };

            return Ok(BaseResponse<LoginResponse>.Success(response));
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> RefreshToken(RefreshTokenRequest request)
        {
            var result = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!result.Succeeded)
            {
                return Unauthorized(BaseResponse<LoginResponse>.Failure("Invalid token or refresh token"));
            }

            var response = new LoginResponse
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                Expiration = result.Expiration
            };

            return Ok(BaseResponse<LoginResponse>.Success(response));
        }
    }
}
