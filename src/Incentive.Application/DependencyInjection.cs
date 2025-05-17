using System.Reflection;
using Incentive.Application.Interfaces;
using Incentive.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Incentive.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register services
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IIncentivePlanService, IncentivePlanService>();
            services.AddScoped<IProjectService, ProjectService>();

            return services;
        }
    }
}
