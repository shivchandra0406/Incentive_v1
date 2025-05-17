using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Incentive.Core.Common;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities.IncentivePlan
{
    
    /// <summary>
    /// Tiered incentive plan where incentives are calculated based on different tiers of performance
    /// </summary>
    public class TieredIncentivePlan : IncentivePlanBase
    {
        [Required]
        public MetricType MetricType { get; set; }

        public virtual ICollection<TieredIncentiveTier> Tiers { get; set; } = new List<TieredIncentiveTier>();
    }
}
