using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Incentive.Core.Enums;

namespace Incentive.Application.DTOs
{
    #region Tiered-Based Plan DTOs
    public class TieredIncentiveTierDto
    {
        public Guid Id { get; set; }
        public Guid TieredIncentivePlanId { get; set; }
        public decimal FromValue { get; set; }
        public decimal ToValue { get; set; }
        public decimal IncentiveValue { get; set; }
        public IncentiveCalculationType CalculationType { get; set; }
    }

    public class CreateTieredIncentiveTierDto
    {
        [Required]
        [Range(0, double.MaxValue)]
        public decimal FromValue { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal ToValue { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal IncentiveValue { get; set; }

        [Required]
        public IncentiveCalculationType CalculationType { get; set; }
    }

    public class UpdateTieredIncentiveTierDto
    {
        [Required]
        public decimal FromValue { get; set; }

        [Required]
        public decimal ToValue { get; set; }
        [Required]
        public decimal IncentiveValue { get; set; }
        [Required]
        public IncentiveCalculationType CalculationType { get; set; }
    }

    public class TieredIncentivePlanDto : IncentivePlanBaseDto
    {
        public MetricType MetricType { get; set; }
        public List<TieredIncentiveTierDto> Tiers { get; set; } = new List<TieredIncentiveTierDto>();
    }

    public class CreateTieredIncentivePlanDto : CreateIncentivePlanBaseDto
    {
        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one tier must be specified")]
        public List<CreateTieredIncentiveTierDto> Tiers { get; set; } = new List<CreateTieredIncentiveTierDto>();
    }

    public class UpdateTieredIncentivePlanDto : UpdateIncentivePlanBaseDto
    {
        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one tier must be specified")]
        public List<UpdateTieredIncentiveTierDto> Tiers { get; set; } = new List<UpdateTieredIncentiveTierDto>();
    }
    #endregion
}
