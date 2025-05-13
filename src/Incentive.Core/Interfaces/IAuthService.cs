using Incentive.Core.Entities;

namespace Incentive.Core.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration, List<string>? userRoles, ApplicationUser? user)> LoginAsync(string userName, string password);
        Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration)> RefreshTokenAsync(string token, string refreshToken);
        Task<(bool Succeeded, string UserId, string Message)> RegisterAsync(string userName, string email, string password, string firstName, string lastName, string tenantId);
        Task<bool> ValidateTokenAsync(string token);
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<(bool Succeeded, string UserId, string Message)> RegisterWithRolesAsync(string userName, string email, string password, string firstName, string lastName, string tenantId, IEnumerable<string> roles);
    }
}
