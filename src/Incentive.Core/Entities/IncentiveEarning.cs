using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Common;
using Incentive.Core.Entities.IncentivePlan;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities
{
    [Schema("IncentiveManagement")]
    public class IncentiveEarning : MultiTenantEntity
    {
        [Required]
        public Guid IncentivePlanId { get; set; }

        [Required]
        public Guid DealId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime EarningDate { get; set; }

        [Required]
        public IncentiveEarningStatus Status { get; set; } = IncentiveEarningStatus.Pending;

        public bool IsPaid { get; set; } = false;

        public DateTime? PaidDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        //public virtual IncentiveRule? IncentiveRule { get; set; }
        [ForeignKey("IncentivePlanId")]
        public virtual IncentivePlanBase? IncentivePlan { get; set; }
        public virtual Deal? Deal { get; set; }
    }
}
