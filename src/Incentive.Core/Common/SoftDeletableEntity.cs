using System;

namespace Incentive.Core.Common
{
    /// <summary>
    /// Base entity with soft delete support
    /// </summary>
    public abstract class SoftDeletableEntity : MultiTenantEntity
    {
        public new bool IsDeleted { get; set; } = false;
        public new DateTime? DeletedAt { get; set; }
        public new string? DeletedBy { get; set; }
    }
}
