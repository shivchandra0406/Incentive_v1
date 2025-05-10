namespace Incentive.Infrastructure.MultiTenancy
{
    /// <summary>
    /// Interface for tenant provider
    /// </summary>
    public interface ITenantProvider
    {
        /// <summary>
        /// Gets the current tenant ID
        /// </summary>
        /// <returns>The current tenant ID</returns>
        string GetTenantId();
    }
}
