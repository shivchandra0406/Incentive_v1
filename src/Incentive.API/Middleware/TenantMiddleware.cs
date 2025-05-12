using System;
using System.Threading.Tasks;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Incentive.API.Middleware
{
    /// <summary>
    /// Middleware to ensure tenant resolution and validation
    /// </summary>
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantMiddleware> _logger;
        private readonly string _tenantHeaderName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="tenantHeaderName">The tenant header name.</param>
        public TenantMiddleware(
            RequestDelegate next,
            ILogger<TenantMiddleware> logger,
            string tenantHeaderName = "tanantId")
        {
            _next = next;
            _logger = logger;
            _tenantHeaderName = tenantHeaderName;
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="tenantService">The tenant service.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
        {
            // Skip tenant validation for specific paths
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && (
                path.StartsWith("/swagger") ||
                path.StartsWith("/health") ||
                path.StartsWith("/api/auth") ||
                path.StartsWith("/api/tenants")))
            {
                await _next(context);
                return;
            }

            // Try to get tenant ID from header
            if (context.Request.Headers.TryGetValue(_tenantHeaderName, out var tenantId))
            {
                var tenantIdValue = tenantId.ToString();
                _logger.LogInformation("Tenant ID {TenantId} found in header", tenantIdValue);

                // Validate tenant ID
                var tenant = await tenantService.GetTenantAsync(tenantIdValue);
                if (tenant == null || !tenant.IsActive)
                {
                    _logger.LogWarning("Invalid or inactive tenant ID {TenantId}", tenantIdValue);
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Invalid or inactive tenant");
                    return;
                }
            }
            else
            {
                // Check if tenant ID is in claims
                var tenantClaim = context.User?.FindFirst("TenantId");
                if (tenantClaim == null)
                {
                    _logger.LogWarning("No tenant ID found in header or claims");
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Tenant ID is required");
                    return;
                }

                // Validate tenant ID from claim
                var tenantIdValue = tenantClaim.Value;
                var tenant = await tenantService.GetTenantAsync(tenantIdValue);
                if (tenant == null || !tenant.IsActive)
                {
                    _logger.LogWarning("Invalid or inactive tenant ID {TenantId} from claim", tenantIdValue);
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Invalid or inactive tenant");
                    return;
                }
            }

            await _next(context);
        }
    }
}
