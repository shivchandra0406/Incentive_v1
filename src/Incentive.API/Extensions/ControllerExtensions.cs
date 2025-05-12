using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Incentive.API.Extensions
{
    /// <summary>
    /// Extension methods for controllers
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Gets the tenant ID from the request
        /// </summary>
        /// <param name="controller">The controller</param>
        /// <param name="headerName">The tenant header name</param>
        /// <returns>The tenant ID</returns>
        public static string GetTenantId(this ControllerBase controller, string headerName = "tenantId")
        {
            // Try to get tenant ID from header
            if (controller.Request.Headers.TryGetValue(headerName, out var tenantId))
            {
                return tenantId.FirstOrDefault();
            }

            // Try to get tenant ID from claim
            var tenantClaim = controller.User?.Claims?.FirstOrDefault(c => c.Type == "TenantId");
            if (tenantClaim != null)
            {
                return tenantClaim.Value;
            }

            return null;
        }
    }
}
