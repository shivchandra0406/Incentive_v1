using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
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

        // Authentication
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<bool> AuthorizeAsync(string userId, string policyName);
        Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration)> LoginAsync(string userName, string password);
        Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration)> RefreshTokenAsync(string token, string refreshToken);

        // Role Management
        Task<bool> AddUserToRoleAsync(string userId, string role);
        Task<bool> RemoveUserFromRoleAsync(string userId, string role);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<(bool Succeeded, string RoleId, string Message)> CreateRoleAsync(string name, string description, string tenantId);
        Task<bool> UpdateRoleAsync(string roleId, string name, string description);
        Task<bool> DeleteRoleAsync(string roleId);
        Task<ApplicationRole> GetRoleByIdAsync(string roleId);
        Task<ApplicationRole> GetRoleByNameAsync(string roleName);
        Task<IList<ApplicationRole>> GetAllRolesAsync();

        // Role Claims Management
        Task<bool> AddClaimToRoleAsync(string roleId, string claimType, string claimValue);
        Task<bool> RemoveClaimFromRoleAsync(string roleId, string claimType, string claimValue);
        Task<IList<Claim>> GetRoleClaimsAsync(string roleId);
    }
}
