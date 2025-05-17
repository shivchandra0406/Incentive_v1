using System;
using System.ComponentModel.DataAnnotations;
using Incentive.Core.Enums;

namespace Incentive.Application.DTOs
{
    #region Kicker-Based Plan DTOs
    public class KickerIncentivePlanDto : IncentivePlanBaseDto
    {
        public string Location { get; set; }
        public MetricType MetricType { get; set; }
        public decimal TargetValue { get; set; }
        public int ConsistencyMonths { get; set; }
        public AwardType AwardType { get; set; }
        public decimal? AwardValue { get; set; }
        public string GiftDescription { get; set; }
    }

    public class CreateKickerIncentivePlanDto : CreateIncentivePlanBaseDto
    {
        [StringLength(200)]
        public string Location { get; set; }

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TargetValue { get; set; }

        [Required]
        [Range(1, 36)]
        public int ConsistencyMonths { get; set; }

        [Required]
        public AwardType AwardType { get; set; }

        // Required if AwardType is Cash
        [Range(0.01, double.MaxValue)]
        public decimal? AwardValue { get; set; }

        // Required if AwardType is Gift
        [StringLength(500)]
        public string GiftDescription { get; set; }
    }

    public class UpdateKickerIncentivePlanDto : UpdateIncentivePlanBaseDto
    {
        [StringLength(200)]
        public string Location { get; set; }

        [Required]
        public MetricType MetricType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TargetValue { get; set; }

        [Required]
        [Range(1, 36)]
        public int ConsistencyMonths { get; set; }

        [Required]
        public AwardType AwardType { get; set; }

        // Required if AwardType is Cash
        [Range(0.01, double.MaxValue)]
        public decimal? AwardValue { get; set; }

        // Required if AwardType is Gift
        [StringLength(500)]
        public string GiftDescription { get; set; }
    }
    #endregion
}
