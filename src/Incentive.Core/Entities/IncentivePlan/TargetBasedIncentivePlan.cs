using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities.IncentivePlan
{
    /// <summary>
    /// Target-based incentive plan where incentives are calculated based on achieving specific targets
    /// </summary>
    public class TargetBasedIncentivePlan : IncentivePlanBase
    {
        [Required]
        public TargetType TargetType { get; set; }

        // If TargetType == SalaryBased
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Salary { get; set; }

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TargetValue { get; set; }

        [Required]
        public IncentiveCalculationType CalculationType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal IncentiveValue { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? AdditionalIncentiveOnExceeding { get; set; }

        [Required]
        public bool IncentiveAfterExceedingTarget { get; set; } = true;
        [Column("ProvideAdditionalIncentiveOnExceeding")]
        public bool ProvideAdditionalIncentiveOnExceeding { get; set; } = false;

        [Required]
        public bool IncludeSalaryInTarget { get; set; } = false;
    }
}
