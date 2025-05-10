using System.Threading.Tasks;
using Incentive.Application.DTOs;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto.UserName, loginDto.Password);

            if (!result.Succeeded)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Succeeded = false,
                    Message = "Invalid username or password"
                });
            }

            return Ok(new AuthResponseDto
            {
                Succeeded = true,
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                Expiration = result.Expiration
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return BadRequest(new AuthResponseDto
                {
                    Succeeded = false,
                    Message = "Passwords do not match"
                });
            }

            var result = await _authService.RegisterAsync(
                registerDto.UserName,
                registerDto.Email,
                registerDto.Password,
                registerDto.FirstName,
                registerDto.LastName,
                registerDto.TenantId);

            if (!result.Succeeded)
            {
                return BadRequest(new AuthResponseDto
                {
                    Succeeded = false,
                    Message = result.Message
                });
            }

            return Ok(new AuthResponseDto
            {
                Succeeded = true,
                UserId = result.UserId,
                Message = "User registered successfully"
            });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenDto.Token, refreshTokenDto.RefreshToken);

            if (!result.Succeeded)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Succeeded = false,
                    Message = "Invalid token or refresh token"
                });
            }

            return Ok(new AuthResponseDto
            {
                Succeeded = true,
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                Expiration = result.Expiration
            });
        }
    }
}
