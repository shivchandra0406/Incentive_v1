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

            // Add Authorization Policies
            services.AddAuthorization(options =>
            {
                // Role-based policies
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Admin", "Manager"));
                options.AddPolicy("RequireFinanceRole", policy => policy.RequireRole("Admin", "Finance"));

                // Permission-based policies
                options.AddPolicy("ViewUsers", policy => policy.RequireClaim("Permission", Permissions.ViewUsers));
                options.AddPolicy("CreateUsers", policy => policy.RequireClaim("Permission", Permissions.CreateUsers));
                options.AddPolicy("EditUsers", policy => policy.RequireClaim("Permission", Permissions.EditUsers));
                options.AddPolicy("DeleteUsers", policy => policy.RequireClaim("Permission", Permissions.DeleteUsers));

                options.AddPolicy("ViewRoles", policy => policy.RequireClaim("Permission", Permissions.ViewRoles));
                options.AddPolicy("CreateRoles", policy => policy.RequireClaim("Permission", Permissions.CreateRoles));
                options.AddPolicy("EditRoles", policy => policy.RequireClaim("Permission", Permissions.EditRoles));
                options.AddPolicy("DeleteRoles", policy => policy.RequireClaim("Permission", Permissions.DeleteRoles));

                options.AddPolicy("ViewTenants", policy => policy.RequireClaim("Permission", Permissions.ViewTenants));
                options.AddPolicy("CreateTenants", policy => policy.RequireClaim("Permission", Permissions.CreateTenants));
                options.AddPolicy("EditTenants", policy => policy.RequireClaim("Permission", Permissions.EditTenants));
                options.AddPolicy("DeleteTenants", policy => policy.RequireClaim("Permission", Permissions.DeleteTenants));

                options.AddPolicy("ViewIncentiveRules", policy => policy.RequireClaim("Permission", Permissions.ViewIncentiveRules));
                options.AddPolicy("CreateIncentiveRules", policy => policy.RequireClaim("Permission", Permissions.CreateIncentiveRules));
                options.AddPolicy("EditIncentiveRules", policy => policy.RequireClaim("Permission", Permissions.EditIncentiveRules));
                options.AddPolicy("DeleteIncentiveRules", policy => policy.RequireClaim("Permission", Permissions.DeleteIncentiveRules));

                options.AddPolicy("ViewDeals", policy => policy.RequireClaim("Permission", Permissions.ViewDeals));
                options.AddPolicy("CreateDeals", policy => policy.RequireClaim("Permission", Permissions.CreateDeals));
                options.AddPolicy("EditDeals", policy => policy.RequireClaim("Permission", Permissions.EditDeals));
                options.AddPolicy("DeleteDeals", policy => policy.RequireClaim("Permission", Permissions.DeleteDeals));
            });

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
            services.AddScoped<ITenantProvider, TenantProvider>();

            // Add HttpContextAccessor
            services.AddHttpContextAccessor();

            // Add Identity Data Seeder
            services.AddIdentityDataSeeder();

            return services;
        }
    }
}
