using System;

namespace Incentive.Domain.Common
{
    /// <summary>
    /// Base entity class with a GUID identifier
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
