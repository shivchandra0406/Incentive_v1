using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities.IncentivePlan
{
    /// <summary>
    /// Role-based incentive plan where incentives are calculated based on user roles
    /// </summary>
    public class RoleBasedIncentivePlan : IncentivePlanBase
    {
        [Required]
        [StringLength(100)]
        public string Role { get; set; } = string.Empty;

        [Required]
        public bool IsTeamBased { get; set; } = false;

        // If IsTeamBased == true
        public Guid? TeamId { get; set; }

        [Required]
        public TargetType TargetType { get; set; }

        // If TargetType == SalaryBased
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? SalaryPercentage { get; set; }

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

        [Required]
        public bool IsCumulative { get; set; } = false;

        [Required]
        public bool IncentiveAfterExceedingTarget { get; set; } = true;

        [Required]
        public bool IncludeSalaryInTarget { get; set; } = false;
    }
}
