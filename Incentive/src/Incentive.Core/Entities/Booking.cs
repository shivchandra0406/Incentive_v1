using System;
using Incentive.Core.Common;

namespace Incentive.Core.Entities
{
    public class Booking : SoftDeletableEntity
    {
        public Guid ProjectId { get; set; }
        public Guid SalespersonId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal BookingValue { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public string Notes { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual Salesperson Salesperson { get; set; }
        public virtual IncentiveEarning IncentiveEarning { get; set; }
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}
