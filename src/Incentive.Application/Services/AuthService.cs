using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Core.Entities;
using Incentive.Core.Interfaces;

namespace Incentive.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IIdentityService _identityService;

        public AuthService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration)> LoginAsync(string userName, string password)
        {
            return await _identityService.LoginAsync(userName, password);
        }

        public async Task<(bool Succeeded, string Token, string RefreshToken, DateTime Expiration)> RefreshTokenAsync(string token, string refreshToken)
        {
            return await _identityService.RefreshTokenAsync(token, refreshToken);
        }

        public async Task<(bool Succeeded, string UserId, string Message)> RegisterAsync(string userName, string email, string password, string firstName, string lastName, string tenantId)
        {
            return await _identityService.CreateUserAsync(userName, email, password, firstName, lastName, tenantId);
        }

        public async Task<(bool Succeeded, string UserId, string Message)> RegisterWithRolesAsync(string userName, string email, string password, string firstName, string lastName, string tenantId, IEnumerable<string> roles)
        {
            return await _identityService.CreateUserWithRolesAsync(userName, email, password, firstName, lastName, tenantId, roles);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            // This would typically validate the token with the identity service
            // For now, we'll just return true
            return true;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            // This would typically get the current user from the identity service
            // For now, we'll just return null
            return null;
        }
    }
}
