using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Common;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities
{
    /// <summary>
    /// Represents an incentive rule that can be applied to users or teams
    /// </summary>
    [Schema("IncentiveManagement")]
    public class IncentiveRule : MultiTenantEntity
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public TargetFrequency Frequency { get; set; }

        [Required]
        public AppliedRuleType AppliedTo { get; set; }

        [Required]
        public CurrencyType Currency { get; set; }

        [Required]
        public TargetType Target { get; set; }

        [Required]
        public IncentiveCalculationType Incentive { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Salary { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TargetValue { get; set; }

        public int? TargetDealCount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Commission { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsIncludeSalary { get; set; } = true;

        // Minimum requirements to qualify for incentive
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? MinimumSalesThreshold { get; set; }

        public int? MinimumDealCountThreshold { get; set; }

        // Maximum incentive caps
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? MaximumIncentiveAmount { get; set; }

        public Guid? TeamId { get; set; }

        // Navigation properties
        public virtual ICollection<IncentiveEarning> IncentiveEarnings { get; set; } = new List<IncentiveEarning>();
        public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();
    }
}
