using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Incentive.Infrastructure.Identity
{
    /// <summary>
    /// Manages user claims based on role assignments
    /// </summary>
    public class UserClaimManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILogger<UserClaimManager> _logger;

        public UserClaimManager(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ILogger<UserClaimManager> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Synchronizes a user's claims with their assigned roles
        /// </summary>
        public async Task SynchronizeUserClaimsAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("Cannot synchronize claims for non-existent user with ID {UserId}", userId);
                    return;
                }

                // Get all roles for the user
                var userRoles = await _userManager.GetRolesAsync(user);
                
                // Get all permission claims from these roles
                var rolePermissions = new HashSet<string>();
                foreach (var roleName in userRoles)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        var roleClaims = await _roleManager.GetClaimsAsync(role);
                        var permissions = roleClaims
                            .Where(c => c.Type == "Permission")
                            .Select(c => c.Value);
                        
                        foreach (var permission in permissions)
                        {
                            rolePermissions.Add(permission);
                        }
                    }
                }

                // Get current user permission claims
                var userClaims = await _userManager.GetClaimsAsync(user);
                var userPermissions = userClaims
                    .Where(c => c.Type == "Permission")
                    .Select(c => c.Value)
                    .ToHashSet();

                // Claims to add (in roles but not in user claims)
                var claimsToAdd = rolePermissions
                    .Where(permission => !userPermissions.Contains(permission))
                    .Select(permission => new Claim("Permission", permission))
                    .ToList();

                // Claims to remove (in user claims but not in any role)
                var claimsToRemove = userClaims
                    .Where(claim => claim.Type == "Permission" && !rolePermissions.Contains(claim.Value))
                    .ToList();

                // Update user claims
                if (claimsToRemove.Any())
                {
                    var removeResult = await _userManager.RemoveClaimsAsync(user, claimsToRemove);
                    if (!removeResult.Succeeded)
                    {
                        var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                        _logger.LogError("Failed to remove claims from user {UserId}: {Errors}", userId, errors);
                    }
                    else
                    {
                        _logger.LogInformation("Removed {Count} claims from user {UserId}", claimsToRemove.Count, userId);
                    }
                }

                if (claimsToAdd.Any())
                {
                    var addResult = await _userManager.AddClaimsAsync(user, claimsToAdd);
                    if (!addResult.Succeeded)
                    {
                        var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                        _logger.LogError("Failed to add claims to user {UserId}: {Errors}", userId, errors);
                    }
                    else
                    {
                        _logger.LogInformation("Added {Count} claims to user {UserId}", claimsToAdd.Count, userId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error synchronizing claims for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Synchronizes claims for all users with a specific role
        /// </summary>
        public async Task SynchronizeUsersInRoleAsync(string roleName)
        {
            try
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                
                foreach (var user in usersInRole)
                {
                    await SynchronizeUserClaimsAsync(user.Id);
                }
                
                _logger.LogInformation("Synchronized claims for {Count} users in role {RoleName}", usersInRole.Count, roleName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error synchronizing claims for users in role {RoleName}", roleName);
                throw;
            }
        }

        /// <summary>
        /// Synchronizes claims for all users in the system
        /// </summary>
        public async Task SynchronizeAllUsersAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                
                foreach (var user in users)
                {
                    await SynchronizeUserClaimsAsync(user.Id);
                }
                
                _logger.LogInformation("Synchronized claims for all {Count} users", users.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error synchronizing claims for all users");
                throw;
            }
        }
    }
}
