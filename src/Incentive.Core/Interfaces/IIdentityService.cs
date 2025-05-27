using System.Security.Claims;
using Incentive.Core.Entities;

namespace Incentive.Core.Interfaces
{
    public interface IIdentityService
    {
        // User Management
        Task<(bool Succeeded, string UserId, string Message)> CreateUserAsync(string userName, string email, string password, string firstName, string lastName, string tenantId);
        Task<(bool Succeeded, string UserId, string Message)> CreateUserAsync(ApplicationUser user, string password);
        Task<(bool Succeeded, string UserId, string Message)> CreateUserWithRolesAsync(string userName, string email, string password, string firstName, string lastName, string tenantId, IEnumerable<string> roles);
        Task<bool> DeleteUserAsync(string userId);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<IList<ApplicationUser>> GetAllUsersAsync();
        Task<IList<ApplicationUser>> GetUsersByTenantIdAsync(string tenantId);
        Task<(bool Succeeded, string Message)> UpdateUserAsync(string userId, string email, string firstName, string lastName, bool? isActive);
        Task<(bool Succeeded, string Message)> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<(bool Succeeded, string Message)> ResetPasswordAsync(string userId, string newPassword);

        /// <summary>
        /// Gets a list of users with minimal data (ID and Name only)
        /// </summary>
        /// <returns>List of users with minimal data</returns>
        Task<IEnumerable<object>> GetUsersMinimalAsync();

        // Authentication
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<bool> AuthorizeAsync(string userId, string policyName);
        Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration, List<string>? userRoles, ApplicationUser? user)> LoginAsync(string userName, string password);
        Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration)> RefreshTokenAsync(string token, string refreshToken);

        // Role Management
        Task<bool> AddUserToRoleAsync(string userId, string role);
        Task<bool> RemoveUserFromRoleAsync(string userId, string role);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<(bool Succeeded, string RoleId, string Message)> CreateRoleAsync(string name, string description, string tenantId);
        Task<(bool Succeeded, string RoleId, string Message)> CreateRoleWithClaimsAsync(string name, string description, string tenantId, IEnumerable<Claim> claims);
        Task<bool> UpdateRoleAsync(string roleId, string name, string description);
        Task<bool> DeleteRoleAsync(string roleId);
        Task<ApplicationRole> GetRoleByIdAsync(string roleId);
        Task<ApplicationRole> GetRoleByNameAsync(string roleName);
        Task<IList<ApplicationRole>> GetAllRolesAsync();

        // Role Claims Management
        Task<bool> AddClaimToRoleAsync(string roleId, string claimType, string claimValue);
        Task<bool> RemoveClaimFromRoleAsync(string roleId, string claimType, string claimValue);
        Task<IList<Claim>> GetRoleClaimsAsync(string roleId);

        // User Claims Management
        Task<bool> AddClaimToUserAsync(string userId, string claimType, string claimValue);
        Task<bool> RemoveClaimFromUserAsync(string userId, string claimType, string claimValue);
        Task<IList<Claim>> GetUserClaimsAsync(string userId);
        Task<bool> UpdateUserClaimsAsync(string userId, IEnumerable<Claim> claims);

        // Permission Management
        Task<Dictionary<string, IList<Claim>>> GetAllRolePermissionsAsync();
        Task<IList<Claim>> GetRolePermissionsByNameAsync(string roleName);
        Task<bool> UpdateRolePermissionsAsync(string roleName, IEnumerable<Claim> permissions);
        Task<(IList<string> Roles, Dictionary<string, IList<Claim>> RolePermissions, IList<Claim> DirectPermissions)> GetUserPermissionsAsync(string userId);
        List<string> GetDefaultPermission();
    }
}
