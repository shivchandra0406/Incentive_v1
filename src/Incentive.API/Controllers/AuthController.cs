using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.API.Attributes;
using Incentive.Application.Common.Interfaces;
using Incentive.Application.Common.Models;
using Incentive.Application.DTOs;
using Incentive.Core.Interfaces;
using Incentive.Infrastructure.MultiTenancy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IIdentityService _identityService;
        private readonly ITenantProvider _tenantProvider;

        public AuthController(IAuthService authService,IIdentityService identityService, ITenantProvider tenantProvider)
        {
            _authService = authService;
            _identityService = identityService;
            _tenantProvider = tenantProvider;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [RequiresTenantId]
        public async Task<ActionResult<BaseResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto.UserName, loginDto.Password);
            AuthResponseDto response = new AuthResponseDto() 
            { 
                Token = result.Token,
                Expiration = result.Expiration,
                RefreshToken = result.RefreshToken,
                Email = result.user?.Email ?? string.Empty,
                FirstName = result.user?.FirstName ?? string.Empty,
                LastName = result.user?.LastName ?? string.Empty,
                Roles = result.userRoles ?? new(),
                TenantId = result.user?.TenantId ?? _tenantProvider.GetTenantId(),
                UserId = result.user?.Id ?? Guid.Empty.ToString(),
                UserName = result.user?.UserName ?? string.Empty,
            };
            return Ok(BaseResponse<AuthResponseDto>.Success(response, "Login successful"));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [RequiresTenantId]
        public async Task<ActionResult<BaseResponse<AuthResponseDto>>> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return BadRequest(BaseResponse<AuthResponseDto>.Failure("Passwords do not match"));
            }

            var result = await _authService.RegisterWithRolesAsync(
                registerDto.UserName,
                registerDto.Email,
                registerDto.Password,
                registerDto.FirstName,
                registerDto.LastName,
                registerDto.TenantId,
                registerDto.Roles.Count > 0 ? registerDto.Roles : new List<string> { "Admin" });

            if (!result.Succeeded)
            {
                return BadRequest(BaseResponse<AuthResponseDto>.Failure(result.Message));
            }

            var response = new AuthResponseDto
            {
                UserId = result.UserId
            };

            return Ok(BaseResponse<AuthResponseDto>.Success(response, "User registered successfully"));
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [RequiresTenantId]
        public async Task<ActionResult<BaseResponse<AuthResponseDto>>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenDto.Token, refreshTokenDto.RefreshToken);

            if (!result.Succeeded)
            {
                return Unauthorized(BaseResponse<AuthResponseDto>.Failure("Invalid token or refresh token"));
            }

            var response = new AuthResponseDto
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                Expiration = result.Expiration
            };

            return Ok(BaseResponse<AuthResponseDto>.Success(response, "Token refreshed successfully"));
        }
    }
}
