using Incentive.API.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Incentive.API.Extensions
{
    /// <summary>
    /// Extension methods for the tenant middleware
    /// </summary>
    public static class TenantMiddlewareExtensions
    {
        /// <summary>
        /// Adds the tenant middleware to the application pipeline.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <param name="tenantHeaderName">The tenant header name.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseTenantMiddleware(
            this IApplicationBuilder builder,
            string tenantHeaderName = "tenantId")
        {
            return builder.UseMiddleware<TenantMiddleware>(tenantHeaderName);
        }
    }
}
