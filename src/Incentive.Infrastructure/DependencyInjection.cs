using System;
using System.Text;
using Incentive.Core.Interfaces;
using Incentive.Infrastructure.Data;
using Incentive.Infrastructure.Identity;
using Incentive.Infrastructure.MultiTenancy;
using Incentive.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Incentive.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            // Add Identity
            services.AddIdentity<AppUser, AppRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Add JWT Authentication
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT Secret is not configured");
            }

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Add Authorization with Policies from the Provider
            services.AddAuthorizationWithPolicies();

            // Add Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIncentiveRuleRepository, IncentiveRuleRepository>();
            services.AddScoped<IDealRepository, DealRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IDealActivityRepository, DealActivityRepository>();

            // Add Services
            services.AddScoped<UserClaimManager>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // Register TenantProvider with logger
            services.AddScoped<ITenantProvider>(provider =>
                new TenantProvider(
                    provider.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                    provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<TenantProvider>>(),
                    tenantHeaderName: "tenantId",
                    tenantClaimType: "TenantId",
                    defaultTenantId: "00000000-0000-0000-0000-000000000000"
                )
            );

            // Add HttpContextAccessor
            services.AddHttpContextAccessor();

            // Add Identity Data Seeder
            services.AddIdentityDataSeeder();

            return services;
        }
    }
}
