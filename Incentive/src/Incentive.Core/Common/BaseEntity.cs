using System;

namespace Incentive.Core.Common
{
    /// <summary>
    /// Base entity with ID
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
