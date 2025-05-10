using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Incentive.Infrastructure.Identity
{
    /// <summary>
    /// Extension methods for identity data seeding
    /// </summary>
    public static class IdentityDataSeederExtensions
    {
        /// <summary>
        /// Adds the identity data seeder to the service collection
        /// </summary>
        public static IServiceCollection AddIdentityDataSeeder(this IServiceCollection services)
        {
            services.AddScoped<IdentityDataSeeder>();
            return services;
        }

        /// <summary>
        /// Seeds the identity data during application startup
        /// </summary>
        public static async Task<IApplicationBuilder> SeedIdentityDataAsync(
            this IApplicationBuilder app, 
            string defaultTenantId = "default")
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var seeder = serviceScope.ServiceProvider.GetRequiredService<IdentityDataSeeder>();
            var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<IdentityDataSeeder>>();

            try
            {
                await seeder.SeedAsync(defaultTenantId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding identity data");
            }

            return app;
        }
    }
}
