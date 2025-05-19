using Incentive.Application.Interfaces;
using Incentive.Core.Entities;
using Incentive.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Incentive.Application.DTOs;
using System.Collections.Generic;

namespace Incentive.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null)
                return null;

            return new ApplicationUser
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                TenantId = appUser.TenantId,
                IsActive = appUser.IsActive,
                CreatedAt = appUser.CreatedAt,
                CreatedBy = appUser.CreatedBy,
                LastModifiedAt = appUser.LastModifiedAt,
                LastModifiedBy = appUser.LastModifiedBy
            };
        }

        public async Task<IEnumerable<UserMinimalDto>> GetUsersMinimalAsync()
        {
            var users = await _userManager.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.UserName)
                .Select(u => new UserMinimalDto
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}"
                })
                .ToListAsync();

            return users;
        }
    }
}
