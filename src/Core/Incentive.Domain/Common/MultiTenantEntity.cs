using System;

namespace Incentive.Domain.Common
{
    /// <summary>
    /// Base entity with multi-tenant support
    /// </summary>
    public abstract class MultiTenantEntity : AuditableEntity
    {
        public string TenantId { get; set; }
    }
}
