using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Common;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities
{
    /// <summary>
    /// Activities and interactions related to deals
    /// </summary>
    public class DealActivity : SoftDeletableEntity
    {
        public Guid DealId { get; set; }
        
        [ForeignKey("DealId")]
        public virtual Deal Deal { get; set; } = null!;
        
        [Required]
        public ActivityType Type { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        public string? UserId { get; set; }
        
        public DateTime ActivityDate { get; set; } = DateTime.UtcNow;
    }
}
