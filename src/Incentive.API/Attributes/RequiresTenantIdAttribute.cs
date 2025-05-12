using System;

namespace Incentive.API.Attributes
{
    /// <summary>
    /// Indicates that an API endpoint requires a tenant ID in the request header.
    /// This attribute is used by Swagger to document the requirement for a tenant ID.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class RequiresTenantIdAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the tenant ID header.
        /// </summary>
        public string HeaderName { get; }

        /// <summary>
        /// Gets the description of the tenant ID header.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the tenant ID is required.
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresTenantIdAttribute"/> class.
        /// </summary>
        /// <param name="headerName">The name of the tenant ID header. Default is "X-Tenant-ID".</param>
        /// <param name="description">The description of the tenant ID header. Default is "The ID of the tenant to access."</param>
        /// <param name="isRequired">Whether the tenant ID is required. Default is true.</param>
        public RequiresTenantIdAttribute(
            string headerName = "tenantId",
            string description = "The ID of the tenant to access.",
            bool isRequired = true)
        {
            HeaderName = headerName;
            Description = description;
            IsRequired = isRequired;
        }
    }
}
