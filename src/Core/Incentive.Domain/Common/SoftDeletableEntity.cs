using System;

namespace Incentive.Domain.Common
{
    /// <summary>
    /// Base entity with soft delete support
    /// </summary>
    public abstract class SoftDeletableEntity : MultiTenantEntity
    {
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
