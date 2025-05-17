using System;
using System.ComponentModel.DataAnnotations;
using Incentive.Core.Enums;

namespace Incentive.Application.DTOs
{
    #region Project-Based Plan DTOs
    public class ProjectBasedIncentivePlanDto : IncentivePlanBaseDto
    {
        public string ProjectName { get; set; }
        public MetricType MetricType { get; set; }
        public decimal TargetValue { get; set; }
        public decimal IncentiveValue { get; set; }
        public IncentiveCalculationType CalculationType { get; set; }
        public bool IsCumulative { get; set; } = true;
    }

    public class CreateProjectBasedIncentivePlanDto : CreateIncentivePlanBaseDto
    {

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        public decimal TargetValue { get; set; }

        [Required]
        public decimal IncentiveValue { get; set; }

        [Required]
        public IncentiveCalculationType CalculationType { get; set; }

        public bool IsCumulative { get; set; } = true;
    }

    public class UpdateProjectBasedIncentivePlanDto : UpdateIncentivePlanBaseDto
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        public decimal TargetValue { get; set; }

        [Required]
        public decimal IncentiveValue { get; set; }

        [Required]
        public IncentiveCalculationType CalculationType { get; set; }

        [Required]
        public bool IsCumulative { get; set; } = true;
    }
    #endregion
}
