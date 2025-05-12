using System;

namespace Incentive.Core.Common
{
    /// <summary>
    /// Base entity with soft delete support
    /// </summary>
    public interface SoftDeletableEntity 
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        
    }
    public interface UserSoftDeletableEntity: SoftDeletableEntity
    {
        public Guid? DeletedBy { get; set; }
    }
}
