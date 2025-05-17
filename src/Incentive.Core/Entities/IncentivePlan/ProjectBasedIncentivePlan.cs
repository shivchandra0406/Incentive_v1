using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Entities;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities.IncentivePlan
{
    /// <summary>
    /// Project-based incentive plan where incentives are calculated based on project performance
    /// </summary>
    public class ProjectBasedIncentivePlan : IncentivePlanBase
    {
        [Required]
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TargetValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal IncentiveValue { get; set; }

        [Required]
        public IncentiveCalculationType CalculationType { get; set; }

        [Required]
        public bool IncentiveAfterExceedingTarget { get; set; } = true;
    }
}
