using System;

namespace Incentive.Domain.Common
{
    /// <summary>
    /// Base entity with auditing properties
    /// </summary>
    public abstract class AuditableEntity : BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
