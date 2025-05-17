using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities.IncentivePlan
{
    /// <summary>
    /// Kicker incentive plan where additional incentives are provided for consistent performance
    /// </summary>
    public class KickerIncentivePlan : IncentivePlanBase
    {
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TargetValue { get; set; }

        [Required]
        public int ConsistencyMonths { get; set; } // 3, 6, 12 etc.

        [Required]
        public AwardType AwardType { get; set; }

        // If AwardType == Cash
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AwardValue { get; set; }

        // If AwardType == Gift
        [StringLength(500)]
        public string GiftDescription { get; set; } = string.Empty;
    }
}
