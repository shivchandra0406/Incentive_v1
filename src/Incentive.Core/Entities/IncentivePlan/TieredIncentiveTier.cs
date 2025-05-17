using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Common;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities.IncentivePlan
{
    /// <summary>
    /// Represents a tier in a tiered incentive plan
    /// </summary>
    [Schema("IncentiveManagement")]
    public class TieredIncentiveTier : MultiTenantEntity
    {

        [Required]
        public Guid TieredIncentivePlanId { get; set; }

        [ForeignKey("TieredIncentivePlanId")]
        public virtual TieredIncentivePlan TieredIncentivePlan { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal FromValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ToValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal IncentiveValue { get; set; }

        [Required]
        public IncentiveCalculationType CalculationType { get; set; }
    }
}
