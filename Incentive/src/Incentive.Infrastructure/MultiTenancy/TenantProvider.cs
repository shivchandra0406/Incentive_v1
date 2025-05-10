using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Incentive.Infrastructure.MultiTenancy
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _tenantHeaderName;

        public TenantProvider(IHttpContextAccessor httpContextAccessor, string tenantHeaderName = "X-Tenant-ID")
        {
            _httpContextAccessor = httpContextAccessor;
            _tenantHeaderName = tenantHeaderName;
        }

        public string GetTenantId()
        {
            // Try to get tenant ID from header
            if (_httpContextAccessor.HttpContext != null &&
                _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(_tenantHeaderName, out var tenantId))
            {
                return tenantId.FirstOrDefault();
            }

            // Try to get tenant ID from claim
            var tenantClaim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "TenantId");

            if (tenantClaim != null)
            {
                return tenantClaim.Value;
            }

            // Default tenant ID (for system operations or when no tenant is specified)
            return Guid.Empty.ToString();
        }
    }

}
