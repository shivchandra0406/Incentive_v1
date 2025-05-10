using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Core.Entities;

namespace Incentive.Core.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, string UserId, string Message)> CreateUserAsync(string userName, string email, string password, string firstName, string lastName, string tenantId);
        Task<(bool Succeeded, string UserId, string Message)> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> DeleteUserAsync(string userId);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<bool> AuthorizeAsync(string userId, string policyName);
        Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration)> LoginAsync(string userName, string password);
        Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration)> RefreshTokenAsync(string token, string refreshToken);
        Task<bool> AddUserToRoleAsync(string userId, string role);
        Task<bool> RemoveUserFromRoleAsync(string userId, string role);
        Task<IList<string>> GetUserRolesAsync(string userId);
    }
}
