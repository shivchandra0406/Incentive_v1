using System;
using Incentive.Core.Entities;

namespace Incentive.Application.DTOs
{
    public class IncentiveEarningDto
    {
        public Guid Id { get; set; }
        public Guid DealId { get; set; }
        public Guid UserId { get; set; }
        public Guid IncnetivePlanId { get; set; }
        public decimal Amount { get; set; }
        public DateTime EarningDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? PaidDate { get; set; }

        // Related data
        public string? UserName { get; set; }
        public string? DealName { get; set; }
    }
}
