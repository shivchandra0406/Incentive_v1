using System;

namespace Incentive.Core.Common
{
    /// <summary>
    /// Base entity with soft delete support
    /// </summary>
    public abstract class SoftDeletableEntity : AuditableEntity
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }
    }
}
