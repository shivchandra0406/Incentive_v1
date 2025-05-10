using System;
using System.Collections.Generic;
using Incentive.Domain.Common;

namespace Incentive.Domain.Entities
{
    public class Project : SoftDeletableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalValue { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<IncentiveRule> IncentiveRules { get; set; } = new List<IncentiveRule>();
    }
}
