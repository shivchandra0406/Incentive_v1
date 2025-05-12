using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Incentive.Infrastructure.MultiTenancy
{
    /// <summary>
    /// Provides tenant resolution from HTTP context
    /// </summary>
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TenantProvider> _logger;
        private readonly string _tenantHeaderName;
        private readonly string _tenantClaimType;
        private readonly string _defaultTenantId;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantProvider"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="tenantHeaderName">The tenant header name.</param>
        /// <param name="tenantClaimType">The tenant claim type.</param>
        /// <param name="defaultTenantId">The default tenant ID.</param>
        public TenantProvider(
            IHttpContextAccessor httpContextAccessor,
            ILogger<TenantProvider> logger,
            string tenantHeaderName = "tenantId",
            string tenantClaimType = "TenantId",
            string defaultTenantId = "00000000-0000-0000-0000-000000000000")
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _tenantHeaderName = tenantHeaderName;
            _tenantClaimType = tenantClaimType;
            _defaultTenantId = defaultTenantId;
        }

        /// <summary>
        /// Gets the current tenant ID from the HTTP context.
        /// </summary>
        /// <returns>The current tenant ID.</returns>
        public string GetTenantId()
        {
            string tenantId = null;

            // Try to get tenant ID from header
            if (_httpContextAccessor.HttpContext != null &&
                _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(_tenantHeaderName, out var tenantIdHeader))
            {
                tenantId = tenantIdHeader.FirstOrDefault();
                if (!string.IsNullOrEmpty(tenantId))
                {
                    _logger.LogDebug("Tenant ID {TenantId} resolved from header", tenantId);
                    return tenantId;
                }
            }

            // Try to get tenant ID from claim
            var tenantClaim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == _tenantClaimType);

            if (tenantClaim != null && !string.IsNullOrEmpty(tenantClaim.Value))
            {
                tenantId = tenantClaim.Value;
                _logger.LogDebug("Tenant ID {TenantId} resolved from claim", tenantId);
                return tenantId;
            }

            // Use default tenant ID if no tenant is specified
            _logger.LogWarning("No tenant ID found in header or claims, using default tenant ID {DefaultTenantId}", _defaultTenantId);
            return _defaultTenantId;
        }
    }
}
