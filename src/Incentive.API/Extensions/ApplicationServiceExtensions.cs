using Incentive.API.Swagger;
using Incentive.Application.Mappings;
using Incentive.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Incentive.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddNewApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Infrastructure services
            services.AddInfrastructure(configuration);

            // Add Application services
            services.AddScoped<Incentive.Core.Interfaces.IAuthService, Incentive.Application.Services.AuthService>();
            services.AddScoped<Incentive.Core.Interfaces.IIncentiveService, Incentive.Application.Services.IncentiveService>();

            // Add AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

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
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Incentive Management API",
                    Version = "v1",
                    Description = "API for managing incentives, deals, and payments",
                    Contact = new OpenApiContact
                    {
                        Name = "Incentive Management Team",
                        Email = "support@incentivemanagement.com"
                    }
                });

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

                // Set the comments path for the Swagger JSON and UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // Add tenant ID operation filter
                c.OperationFilter<TenantIdOperationFilter>();
            });

            return services;
        }
    }
}
