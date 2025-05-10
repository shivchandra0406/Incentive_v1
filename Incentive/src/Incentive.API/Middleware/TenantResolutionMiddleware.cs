using System.Threading.Tasks;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Incentive.API.Middleware
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantResolutionMiddleware> _logger;
        private readonly string _tenantHeaderName;

        public TenantResolutionMiddleware(
            RequestDelegate next,
            ILogger<TenantResolutionMiddleware> logger,
            string tenantHeaderName = "X-Tenant-ID")
        {
            _next = next;
            _logger = logger;
            _tenantHeaderName = tenantHeaderName;
        }

        public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
        {
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
                // Try to get tenant ID from claim
                var tenantClaim = context.User?.FindFirst("TenantId");
                if (tenantClaim != null)
                {
                    _logger.LogInformation("Tenant ID {TenantId} found in claim", tenantClaim.Value);
                }
                else
                {
                    _logger.LogInformation("No tenant ID found in header or claim");
                }
            }

            await _next(context);
        }
    }
}
