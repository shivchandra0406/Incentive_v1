using System;

namespace Incentive.Core.Common
{
    /// <summary>
    /// Base entity with audit information
    /// </summary>
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
