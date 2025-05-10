using System;
using Incentive.Core.Entities;

namespace Incentive.Application.DTOs
{
    public class IncentiveEarningDto
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public Guid SalespersonId { get; set; }
        public Guid IncentiveRuleId { get; set; }
        public decimal Amount { get; set; }
        public DateTime EarningDate { get; set; }
        public string Status { get; set; }
        public DateTime? PaidDate { get; set; }
        
        // Related data
        public string SalespersonName { get; set; }
        public string ProjectName { get; set; }
        public string IncentiveRuleName { get; set; }
    }

    public class IncentiveRuleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ProjectId { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public decimal? MinBookingValue { get; set; }
        public decimal? MaxBookingValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        
        // Related data
        public string ProjectName { get; set; }
    }

    public class CreateIncentiveRuleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ProjectId { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public decimal? MinBookingValue { get; set; }
        public decimal? MaxBookingValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class UpdateIncentiveRuleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ProjectId { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public decimal? MinBookingValue { get; set; }
        public decimal? MaxBookingValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
