using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Common;

namespace Incentive.Core.Entities
{
    /// <summary>
    /// Represents a member of a team
    /// </summary>
    [Schema("IncentiveManagement")]
    public class TeamMember : MultiTenantEntity
    {
        [Required]
        public Guid TeamId { get; set; }

        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Role { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LeftDate { get; set; }
    }
}
