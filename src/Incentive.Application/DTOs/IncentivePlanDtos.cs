using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Enums;

namespace Incentive.Application.DTOs
{
    #region Base DTOs
    public class IncentivePlanBaseDto
    {
        public Guid Id { get; set; }
        public string PlanName { get; set; }
        public IncentivePlanType PlanType { get; set; }
        public PeriodType PeriodType { get; set; }
        public bool IsActive { get; set; }
        public string TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public class CreateIncentivePlanBaseDto
    {
        [Required]
        [StringLength(200)]
        public string PlanName { get; set; }

        [Required]
        public IncentivePlanType PlanType { get; set; }

        [Required]
        public PeriodType PeriodType { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateIncentivePlanBaseDto
    {
        [Required]
        [StringLength(200)]
        public string PlanName { get; set; }

        [Required]
        public PeriodType PeriodType { get; set; }

        public bool IsActive { get; set; }
    }
    #endregion

    #region Target-Based Plan DTOs
    public class TargetBasedIncentivePlanDto : IncentivePlanBaseDto
    {
        public TargetType TargetType { get; set; }
        public decimal? Salary { get; set; }
        public MetricType MetricType { get; set; }
        public decimal TargetValue { get; set; }
        public IncentiveCalculationType CalculationType { get; set; }
        public decimal IncentiveValue { get; set; }
        public bool IncentiveAfterExceedingTarget { get; set; }
        [Column("ProvideAdditionalIncentiveOnExceeding")]
        public bool ProvideAdditionalIncentiveOnExceeding { get; set; }
        public bool IncludeSalaryInTarget { get; set; }
    }

    public class CreateTargetBasedIncentivePlanDto : CreateIncentivePlanBaseDto
    {
        [Required]
        public TargetType TargetType { get; set; }

        // Required if TargetType is SalaryBased
        public decimal? Salary { get; set; }

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        public decimal TargetValue { get; set; }

        [Required]
        public IncentiveCalculationType CalculationType { get; set; }

        [Required]
        public decimal IncentiveValue { get; set; }

        public bool IncentiveAfterExceedingTarget { get; set; } = true;
        public bool ProvideAdditionalIncentiveOnExceeding { get; set; } = false;
        public bool IncludeSalaryInTarget { get; set; } = false;
    }

    public class UpdateTargetBasedIncentivePlanDto : UpdateIncentivePlanBaseDto
    {
        [Required]
        public TargetType TargetType { get; set; }

        // Required if TargetType is SalaryBased
        public decimal? Salary { get; set; }

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        public decimal TargetValue { get; set; }

        [Required]
        public IncentiveCalculationType CalculationType { get; set; }

        [Required]
        public decimal IncentiveValue { get; set; }

        public bool IncentiveAfterExceedingTarget { get; set; }
        public bool ProvideAdditionalIncentiveOnExceeding { get; set; }
        public bool IncludeSalaryInTarget { get; set; }
    }
    #endregion
}
