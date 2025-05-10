using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Incentive.Core.Entities;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Incentive.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConfiguration _configuration;

        public IdentityService(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IAuthorizationService authorizationService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _configuration = configuration;
        }

        public async Task<(bool Succeeded, string UserId, string Message)> CreateUserAsync(string userName, string email, string password, string firstName, string lastName, string tenantId)
        {
            var appUser = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                TenantId = tenantId,
                CreatedAt = DateTime.UtcNow
            };

            return await CreateUserAsync(appUser, password);
        }

        public async Task<(bool Succeeded, string UserId, string Message)> CreateUserAsync(ApplicationUser appUser, string password)
        {
            var user = new AppUser
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                TenantId = appUser.TenantId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = appUser.CreatedBy
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return (true, user.Id, "User created successfully");
            }

            return (false, string.Empty, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return MapToApplicationUser(user);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            return MapToApplicationUser(user);
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            // Create a ClaimsPrincipal manually
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("TenantId", user.TenantId)
            };

            // Add roles
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Add user claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var identity = new ClaimsIdentity(claims, "Bearer");
            var principal = new ClaimsPrincipal(identity);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration)> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return (false, string.Empty, string.Empty, DateTime.MinValue);
            }

            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return (true, token.Token, refreshToken, token.Expiration);
        }

        public async Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration)> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                return (false, string.Empty, string.Empty, DateTime.MinValue);
            }

            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return (false, string.Empty, string.Empty, DateTime.MinValue);
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return (false, string.Empty, string.Empty, DateTime.MinValue);
            }

            var newToken = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return (true, newToken.Token, newRefreshToken, newToken.Expiration);
        }

        public async Task<bool> AddUserToRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new AppRole { Name = role });
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new List<string>();
            }

            return await _userManager.GetRolesAsync(user);
        }

        private async Task<(string Token, DateTime Expiration)> GenerateJwtToken(AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("TenantId", user.TenantId)
            };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(userClaims);

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured");
            var expirationMinutes = jwtSettings["ExpirationInMinutes"] ?? "60";

            var key = Encoding.ASCII.GetBytes(secretKey);
            var expiration = DateTime.UtcNow.AddMinutes(double.Parse(expirationMinutes));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), expiration);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];

            if (string.IsNullOrEmpty(secretKey))
            {
                return null;
            }

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = false // Don't validate lifetime here
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private ApplicationUser MapToApplicationUser(AppUser user)
        {
            return new ApplicationUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TenantId = user.TenantId,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                CreatedBy = user.CreatedBy,
                LastModifiedAt = user.LastModifiedAt,
                LastModifiedBy = user.LastModifiedBy,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
            };
        }
    }
}
