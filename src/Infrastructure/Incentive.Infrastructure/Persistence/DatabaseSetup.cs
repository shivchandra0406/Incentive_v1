using System;
using System.Linq;
using System.Threading.Tasks;
using Incentive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Incentive.Infrastructure.Persistence
{
    public static class DatabaseSetup
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                
                // Apply migrations
                await context.Database.MigrateAsync();
                
                // Seed data
                await SeedDataAsync(context, services);
                
                logger.LogInformation("Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }

        private static async Task SeedDataAsync(ApplicationDbContext context, IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

            // Seed default tenant if none exists
            if (!await context.Tenants.AnyAsync())
            {
                var defaultTenant = new Tenant
                {
                    Id = Guid.NewGuid(),
                    Name = "Default Tenant",
                    Identifier = "default",
                    ConnectionString = null, // Use the default connection string
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                };

                await context.Tenants.AddAsync(defaultTenant);
                await context.SaveChangesAsync();

                // Seed roles
                var roles = new[] { "Admin", "Manager", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new ApplicationRole
                        {
                            Name = role,
                            TenantId = defaultTenant.Id.ToString(),
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = "system",
                            Description = $"Default {role} role"
                        });
                    }
                }

                // Seed admin user
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    TenantId = defaultTenant.Id.ToString(),
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                };

                if (await userManager.FindByEmailAsync(adminUser.Email) == null)
                {
                    await userManager.CreateAsync(adminUser, "Admin123!");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
