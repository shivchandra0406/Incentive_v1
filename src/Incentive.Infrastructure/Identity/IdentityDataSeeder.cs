using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Incentive.Infrastructure.Identity
{
    /// <summary>
    /// Seeds initial identity data (roles, claims, admin user)
    /// </summary>
    public class IdentityDataSeeder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<IdentityDataSeeder> _logger;
        private readonly UserClaimManager _userClaimManager;

        public IdentityDataSeeder(
            IServiceProvider serviceProvider,
            ILogger<IdentityDataSeeder> logger,
            UserClaimManager userClaimManager)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _userClaimManager = userClaimManager;
        }

        /// <summary>
        /// Seeds the initial identity data
        /// </summary>
        public async Task SeedAsync(string defaultTenantId = "default")
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                _logger.LogInformation("Starting identity data seeding...");

                // Seed roles and their claims
                await SeedRolesAsync(roleManager, defaultTenantId);

                // Seed default admin user if it doesn't exist
                await SeedDefaultAdminUserAsync(userManager, defaultTenantId);

                // Synchronize user claims with role permissions
                _logger.LogInformation("Synchronizing user claims with role permissions...");
                await _userClaimManager.SynchronizeAllUsersAsync();

                _logger.LogInformation("Identity data seeding completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding identity data");
                throw;
            }
        }

        /// <summary>
        /// Seeds the default roles and their claims
        /// </summary>
        private async Task SeedRolesAsync(RoleManager<AppRole> roleManager, string tenantId)
        {
            _logger.LogInformation("Seeding roles and claims...");

            foreach (var roleName in DefaultRoles.GetAllRoles())
            {
                // Check if role exists
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                AppRole role;

                if (!roleExists)
                {
                    // Create the role
                    role = new AppRole
                    {
                        Name = roleName,
                        Description = DefaultRoles.GetRoleDescription(roleName),
                        TenantId = tenantId,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "system"
                    };

                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        _logger.LogError("Failed to create role {RoleName}: {Errors}", roleName, errors);
                        continue;
                    }

                    _logger.LogInformation("Created role {RoleName}", roleName);
                }
                else
                {
                    // Get existing role
                    role = await roleManager.FindByNameAsync(roleName);
                }

                // Get permissions for this role
                var permissions = DefaultRoles.GetPermissionsForRole(roleName);

                // Get existing claims for the role
                var existingClaims = await roleManager.GetClaimsAsync(role);
                var existingPermissions = existingClaims
                    .Where(c => c.Type == "Permission")
                    .Select(c => c.Value)
                    .ToList();

                // Add missing permissions
                foreach (var permission in permissions)
                {
                    if (!existingPermissions.Contains(permission))
                    {
                        await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                        _logger.LogInformation("Added permission {Permission} to role {RoleName}", permission, roleName);
                    }
                }
            }

            _logger.LogInformation("Roles and claims seeding completed");
        }

        /// <summary>
        /// Seeds the default admin user
        /// </summary>
        private async Task SeedDefaultAdminUserAsync(UserManager<AppUser> userManager, string tenantId)
        {
            _logger.LogInformation("Seeding default admin user...");

            const string adminEmail = "admin@incentive.com";
            const string adminUserName = "admin";
            const string adminPassword = "Admin123!";

            // Check if admin user exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // Create admin user
                adminUser = new AppUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Administrator",
                    TenantId = tenantId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to create admin user: {Errors}", errors);
                    return;
                }

                _logger.LogInformation("Created admin user with email {Email}", adminEmail);

                // Assign admin role
                result = await userManager.AddToRoleAsync(adminUser, DefaultRoles.Admin);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to assign Admin role to admin user: {Errors}", errors);
                    return;
                }

                _logger.LogInformation("Assigned Admin role to admin user");
            }
            else
            {
                _logger.LogInformation("Admin user already exists");

                // Ensure admin user has Admin role
                if (!await userManager.IsInRoleAsync(adminUser, DefaultRoles.Admin))
                {
                    var result = await userManager.AddToRoleAsync(adminUser, DefaultRoles.Admin);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        _logger.LogError("Failed to assign Admin role to existing admin user: {Errors}", errors);
                        return;
                    }

                    _logger.LogInformation("Assigned Admin role to existing admin user");
                }
            }

            _logger.LogInformation("Default admin user seeding completed");
        }
    }
}
