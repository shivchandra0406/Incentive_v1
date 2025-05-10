using System;
using System.ComponentModel.DataAnnotations;
using Incentive.Domain.Enums;

namespace Incentive.WebAPI.Models
{
    public class CreateIncentiveRuleRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? ProjectId { get; set; }

        [Required]
        public IncentiveType Type { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Value { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinBookingValue { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxBookingValue { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class UpdateIncentiveRuleRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? ProjectId { get; set; }

        [Required]
        public IncentiveType Type { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Value { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinBookingValue { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxBookingValue { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
