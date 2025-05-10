using System;
using Incentive.API.Middleware;
using Incentive.Application.Services;
using Incentive.Core.Interfaces;
using Incentive.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Incentive.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Infrastructure services
            services.AddInfrastructure(configuration);

            // Add Application services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IIncentiveService, IncentiveService>();

            // Add HttpContextAccessor
            services.AddHttpContextAccessor();

            // Add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Incentive Management API", Version = "v1" });

                // Add JWT Authentication to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseApplicationMiddleware(this IApplicationBuilder app, bool isDevelopment)
        {
            // Use Exception Middleware
            app.UseMiddleware<ExceptionMiddleware>(isDevelopment);

            // Use Tenant Resolution Middleware
            app.UseMiddleware<TenantResolutionMiddleware>();

            return app;
        }
    }
}
