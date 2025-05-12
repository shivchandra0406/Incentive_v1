using System.Collections.Generic;
using System.Linq;
using Incentive.API.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Incentive.API.Swagger
{
    /// <summary>
    /// Operation filter to add tenant ID header parameter to Swagger operations.
    /// </summary>
    public class TenantIdOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the operation has the RequiresTenantId attribute
            var requiresTenantIdAttribute = context.MethodInfo.GetCustomAttribute<RequiresTenantIdAttribute>() ??
                                           context.MethodInfo.DeclaringType?.GetCustomAttribute<RequiresTenantIdAttribute>();

            // If the attribute is not found, check if the controller has a route that starts with "api/tenants" or other excluded paths
            if (requiresTenantIdAttribute == null)
            {
                var controllerRoute = context.MethodInfo.DeclaringType?.GetCustomAttributes<Microsoft.AspNetCore.Mvc.RouteAttribute>()
                    .FirstOrDefault()?.Template?.ToLower();

                // Skip adding tenant ID header for excluded paths
                if (controllerRoute != null && 
                    (controllerRoute.StartsWith("api/tenants") || 
                     controllerRoute.StartsWith("api/auth") || 
                     controllerRoute.StartsWith("health")))
                {
                    return;
                }

                // Default values if attribute is not found but tenant ID is still required
                requiresTenantIdAttribute = new RequiresTenantIdAttribute();
            }

            // Add tenant ID header parameter to the operation
            operation.Parameters ??= new List<OpenApiParameter>();

            // Check if the parameter already exists
            if (!operation.Parameters.Any(p => p.Name == requiresTenantIdAttribute.HeaderName && p.In == ParameterLocation.Header))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = requiresTenantIdAttribute.HeaderName,
                    In = ParameterLocation.Header,
                    Description = requiresTenantIdAttribute.Description,
                    Required = requiresTenantIdAttribute.IsRequired,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
            }
        }
    }
}
