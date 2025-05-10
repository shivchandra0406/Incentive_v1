using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Incentive.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Incentive.API.Extensions
{
    public static class ApplicationMiddlewareExtensions
    {
        public static IApplicationBuilder UseNewApplicationMiddleware(this IApplicationBuilder app, bool isDevelopment)
        {
            // Global exception handler
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        // Log the error
                        var logger = context.RequestServices.GetService(typeof(ILogger<Program>)) as ILogger<Program>;
                        logger?.LogError(contextFeature.Error, "Unhandled exception");

                        // Return error details in development, generic message in production
                        var response = new
                        {
                            statusCode = context.Response.StatusCode,
                            message = isDevelopment ? contextFeature.Error.Message : "An unexpected error occurred.",
                            details = isDevelopment ? contextFeature.Error.StackTrace : null
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });

            // Seed identity data (roles, claims, admin user)
            app.SeedIdentityDataAsync("default").GetAwaiter().GetResult();

            return app;
        }
    }
}
