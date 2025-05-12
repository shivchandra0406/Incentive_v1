using System;

namespace Incentive.Core.Common
{
    /// <summary>
    /// Base entity with audit information
    /// </summary>
    public abstract class AuditableEntity : BaseEntity, SoftDeletableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
       
    }
    public abstract class UserAuditableEntity:  BaseEntity, UserSoftDeletableEntity
    {
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public Guid LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
