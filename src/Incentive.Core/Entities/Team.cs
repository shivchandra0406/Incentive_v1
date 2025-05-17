using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Incentive.Core.Common;

namespace Incentive.Core.Entities
{
    /// <summary>
    /// Represents a team of users
    /// </summary>
    [Schema("IncentiveManagement")]
    public class Team : MultiTenantEntity
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    }
}
