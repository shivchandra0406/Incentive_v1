using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Incentive.Core.Entities;
using Incentive.Core.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserClaimManager _userClaimManager;

        public IdentityService(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IAuthorizationService authorizationService,
            IConfiguration configuration,
            UserClaimManager userClaimManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _configuration = configuration;
            _userClaimManager = userClaimManager;
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
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            var result = await CreateUserAsync(appUser, password);

            // Assign Admin role by default
            if (result.Succeeded)
            {
                await AddUserToRoleAsync(result.UserId, "Admin");
            }

            return result;
        }

        public async Task<(bool Succeeded, string UserId, string Message)> CreateUserWithRolesAsync(string userName, string email, string password, string firstName, string lastName, string tenantId, IEnumerable<string> roles)
        {
            var appUser = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                TenantId = tenantId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            var result = await CreateUserAsync(appUser, password);

            if (result.Succeeded && roles != null)
            {
                foreach (var role in roles)
                {
                    await AddUserToRoleAsync(result.UserId, role);
                }
            }

            return result;
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

        public async Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration,List<string>? userRoles, ApplicationUser? user)> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new UnauthorizedAccessException("Username and password are invalid");
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return (true, token.Token, refreshToken, token.Expiration,userRoles?.ToList(),user.Adapt<ApplicationUser>());
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

            if (result.Succeeded)
            {
                // Synchronize user claims with role permissions
                await _userClaimManager.SynchronizeUserClaimsAsync(userId);
            }

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

            if (result.Succeeded)
            {
                // Synchronize user claims with role permissions
                await _userClaimManager.SynchronizeUserClaimsAsync(userId);
            }

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

        public async Task<IList<ApplicationUser>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(MapToApplicationUser).ToList();
        }

        public async Task<IList<ApplicationUser>> GetUsersByTenantIdAsync(string tenantId)
        {
            var users = await _userManager.Users
                .Where(u => u.TenantId == tenantId)
                .ToListAsync();

            return users.Select(MapToApplicationUser).ToList();
        }

        public async Task<IEnumerable<object>> GetUsersMinimalAsync()
        {
            var users = await _userManager.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.UserName)
                .Select(u => new 
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}"
                })
                .ToListAsync();

            return users;
        }

        public async Task<(bool Succeeded, string Message)> UpdateUserAsync(string userId, string email, string firstName, string lastName, bool? isActive)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, "User not found");
            }

            if (!string.IsNullOrEmpty(email) && user.Email != email)
            {
                // Check if email is already in use
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null && existingUser.Id != userId)
                {
                    return (false, "Email is already in use");
                }

                user.Email = email;
                user.NormalizedEmail = _userManager.NormalizeEmail(email);
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                user.FirstName = firstName;
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                user.LastName = lastName;
            }

            if (isActive.HasValue)
            {
                user.IsActive = isActive.Value;
            }

            user.LastModifiedAt = DateTime.UtcNow;
            user.LastModifiedBy = "system";

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return (true, "User updated successfully");
            }

            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<(bool Succeeded, string Message)> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, "User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result.Succeeded)
            {
                return (true, "Password changed successfully");
            }

            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<(bool Succeeded, string Message)> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, "User not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return (true, "Password reset successfully");
            }

            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<(bool Succeeded, string RoleId, string Message)> CreateRoleAsync(string name, string description, string tenantId)
        {
            var role = new AppRole
            {
                Name = name,
                Description = description,
                TenantId = tenantId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return (true, role.Id, "Role created successfully");
            }

            return (false, string.Empty, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<(bool Succeeded, string RoleId, string Message)> CreateRoleWithClaimsAsync(string name, string description, string tenantId, IEnumerable<Claim> claims)
        {
            // Create the role first
            var createRoleResult = await CreateRoleAsync(name, description, tenantId);

            if (!createRoleResult.Succeeded)
            {
                return createRoleResult;
            }

            var roleId = createRoleResult.RoleId;
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return (false, string.Empty, "Role was created but could not be retrieved");
            }

            // Add claims to the role
            foreach (var claim in claims)
            {
                var addClaimResult = await _roleManager.AddClaimAsync(role, claim);

                if (!addClaimResult.Succeeded)
                {
                    // If adding claims fails, we should still return success for the role creation
                    // but include a warning in the message
                    return (true, roleId, $"Role created successfully but some claims could not be added: {string.Join(", ", addClaimResult.Errors.Select(e => e.Description))}");
                }
            }

            return (true, roleId, "Role created successfully with all claims");
        }

        public async Task<bool> UpdateRoleAsync(string roleId, string name, string description)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return false;
            }

            role.Name = name;
            role.Description = description;
            role.LastModifiedAt = DateTime.UtcNow;
            role.LastModifiedBy = "system";

            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return false;
            }

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<ApplicationRole> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return null;
            }

            return MapToApplicationRole(role);
        }

        public async Task<ApplicationRole> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return null;
            }

            return MapToApplicationRole(role);
        }

        public async Task<IList<ApplicationRole>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(MapToApplicationRole).ToList();
        }

        public async Task<bool> AddClaimToRoleAsync(string roleId, string claimType, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return false;
            }

            var claim = new Claim(claimType, claimValue);
            var result = await _roleManager.AddClaimAsync(role, claim);

            if (result.Succeeded && role.Name != null)
            {
                // Synchronize claims for all users in this role
                await _userClaimManager.SynchronizeUsersInRoleAsync(role.Name);
            }

            return result.Succeeded;
        }

        public async Task<bool> RemoveClaimFromRoleAsync(string roleId, string claimType, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return false;
            }

            var claim = new Claim(claimType, claimValue);
            var result = await _roleManager.RemoveClaimAsync(role, claim);

            if (result.Succeeded && role.Name != null)
            {
                // Synchronize claims for all users in this role
                await _userClaimManager.SynchronizeUsersInRoleAsync(role.Name);
            }

            return result.Succeeded;
        }

        public async Task<IList<Claim>> GetRoleClaimsAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return new List<Claim>();
            }

            return await _roleManager.GetClaimsAsync(role);
        }

        public async Task<bool> AddClaimToUserAsync(string userId, string claimType, string claimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var claim = new Claim(claimType, claimValue);
            var result = await _userManager.AddClaimAsync(user, claim);

            return result.Succeeded;
        }

        public async Task<bool> RemoveClaimFromUserAsync(string userId, string claimType, string claimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var claim = new Claim(claimType, claimValue);
            var result = await _userManager.RemoveClaimAsync(user, claim);

            return result.Succeeded;
        }

        public async Task<IList<Claim>> GetUserClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new List<Claim>();
            }

            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<bool> UpdateUserClaimsAsync(string userId, IEnumerable<Claim> claims)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            // Get current claims
            var currentClaims = await _userManager.GetClaimsAsync(user);

            // Remove all current claims
            foreach (var claim in currentClaims)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }

            // Add new claims
            foreach (var claim in claims)
            {
                await _userManager.AddClaimAsync(user, claim);
            }

            return true;
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

        private ApplicationRole MapToApplicationRole(AppRole role)
        {
            return new ApplicationRole
            {
                Id = role.Id,
                Name = role.Name,
                TenantId = role.TenantId,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                CreatedBy = role.CreatedBy,
                LastModifiedAt = role.LastModifiedAt,
                LastModifiedBy = role.LastModifiedBy
            };
        }

        // Permission Management Implementation
        public async Task<Dictionary<string, IList<Claim>>> GetAllRolePermissionsAsync()
        {
            var result = new Dictionary<string, IList<Claim>>();
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                if (role.Name != null)
                {
                    var claims = await _roleManager.GetClaimsAsync(role);
                    var permissionClaims = claims.Where(c => c.Type == "Permission").ToList();
                    result.Add(role.Name, permissionClaims);
                }
            }

            return result;
        }

        public async Task<IList<Claim>> GetRolePermissionsByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return new List<Claim>();
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            return claims.Where(c => c.Type == "Permission").ToList();
        }

        public async Task<bool> UpdateRolePermissionsAsync(string roleName, IEnumerable<Claim> permissions)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return false;
            }

            // Get current permission claims
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            var currentPermissionClaims = currentClaims.Where(c => c.Type == "Permission").ToList();

            // Remove all current permission claims
            foreach (var claim in currentPermissionClaims)
            {
                var result = await _roleManager.RemoveClaimAsync(role, claim);
                if (!result.Succeeded)
                {
                    return false;
                }
            }

            // Add new permission claims
            foreach (var claim in permissions)
            {
                var newClaim = new Claim(claim.Type, claim.Value);
                var result = await _roleManager.AddClaimAsync(role, newClaim);
                if (!result.Succeeded)
                {
                    return false;
                }
            }

            // Synchronize claims for all users in this role
            if (role.Name != null)
            {
                await _userClaimManager.SynchronizeUsersInRoleAsync(role.Name);
            }

            return true;
        }

        public async Task<(IList<string> Roles, Dictionary<string, IList<Claim>> RolePermissions, IList<Claim> DirectPermissions)> GetUserPermissionsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (new List<string>(), new Dictionary<string, IList<Claim>>(), new List<Claim>());
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Get permissions from each role
            var rolePermissions = new Dictionary<string, IList<Claim>>();
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    var permissionClaims = roleClaims.Where(c => c.Type == "Permission").ToList();
                    rolePermissions.Add(roleName, permissionClaims);
                }
            }

            // Get direct user claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            var directPermissions = userClaims.Where(c => c.Type == "Permission").ToList();

            return (roles.ToList(), rolePermissions, directPermissions);
        }

        public List<string> GetDefaultPermission()
        {
            return Permissions.GetAllPermissions();
        }
    }
}
