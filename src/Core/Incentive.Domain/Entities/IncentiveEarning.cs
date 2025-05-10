using System;
using Incentive.Domain.Common;
using Incentive.Domain.Enums;

namespace Incentive.Domain.Entities
{
    public class IncentiveEarning : SoftDeletableEntity
    {
        public Guid BookingId { get; set; }
        public Guid SalespersonId { get; set; }
        public Guid IncentiveRuleId { get; set; }
        public decimal Amount { get; set; }
        public DateTime EarningDate { get; set; }
        public IncentiveEarningStatus Status { get; set; } = IncentiveEarningStatus.Pending;
        public DateTime? PaidDate { get; set; }
        
        // Navigation properties
        public virtual Booking Booking { get; set; }
        public virtual Salesperson Salesperson { get; set; }
        public virtual IncentiveRule IncentiveRule { get; set; }
    }
}
