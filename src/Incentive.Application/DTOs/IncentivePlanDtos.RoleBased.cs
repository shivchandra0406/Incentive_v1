using System;
using System.ComponentModel.DataAnnotations;
using Incentive.Core.Enums;

namespace Incentive.Application.DTOs
{
    #region Role-Based Plan DTOs
    public class RoleBasedIncentivePlanDto : IncentivePlanBaseDto
    {
        public string Role { get; set; }
        public bool IsTeamBased { get; set; }
        public Guid? TeamId { get; set; }
        public string TeamName { get; set; }
        public TargetType TargetType { get; set; }
        public decimal? SalaryPercentage { get; set; }
        public MetricType MetricType { get; set; }
        public decimal TargetValue { get; set; }
        public IncentiveCalculationType CalculationType { get; set; }
        public decimal IncentiveValue { get; set; }
        public bool IsCumulative { get; set; }
        public bool IncentiveAfterExceedingTarget { get; set; }
        public bool IncludeSalaryInTarget { get; set; }
    }

    public class CreateRoleBasedIncentivePlanDto : CreateIncentivePlanBaseDto
    {
        [Required]
        [StringLength(100)]
        public string Role { get; set; }

        public bool IsTeamBased { get; set; } = false;

        // Required if IsTeamBased is true
        public Guid? TeamId { get; set; }

        [Required]
        public TargetType TargetType { get; set; }

        // Required if TargetType is SalaryBased
        [Range(0.01, 100)]
        public decimal? SalaryPercentage { get; set; }

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TargetValue { get; set; }

        [Required]
        public IncentiveCalculationType CalculationType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal IncentiveValue { get; set; }

        public bool IsCumulative { get; set; } = false;
        public bool IncentiveAfterExceedingTarget { get; set; } = true;
        public bool IncludeSalaryInTarget { get; set; } = false;
    }

    public class UpdateRoleBasedIncentivePlanDto : UpdateIncentivePlanBaseDto
    {
        [Required]
        [StringLength(100)]
        public string Role { get; set; }

        public bool IsTeamBased { get; set; }

        // Required if IsTeamBased is true
        public Guid? TeamId { get; set; }

        [Required]
        public TargetType TargetType { get; set; }

        // Required if TargetType is SalaryBased
        [Range(0.01, 100)]
        public decimal? SalaryPercentage { get; set; }

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TargetValue { get; set; }

        [Required]
        public IncentiveCalculationType CalculationType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal IncentiveValue { get; set; }

        public bool IsCumulative { get; set; }
        public bool IncentiveAfterExceedingTarget { get; set; }
        public bool IncludeSalaryInTarget { get; set; }
    }
    #endregion
}
