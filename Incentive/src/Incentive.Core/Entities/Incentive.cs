using System;
using System.Collections.Generic;
using Incentive.Core.Common;

namespace Incentive.Core.Entities
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

    public enum IncentiveEarningStatus
    {
        Pending,
        Approved,
        Rejected,
        Paid
    }
}
