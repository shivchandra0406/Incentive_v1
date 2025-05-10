using System;
using System.Collections.Generic;
using Incentive.Core.Common;

namespace Incentive.Core.Entities
{
    public class IncentiveRule : SoftDeletableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ProjectId { get; set; }
        public IncentiveType Type { get; set; }
        public decimal Value { get; set; }
        public decimal? MinBookingValue { get; set; }
        public decimal? MaxBookingValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual ICollection<IncentiveEarning> IncentiveEarnings { get; set; } = new List<IncentiveEarning>();
    }

    public enum IncentiveType
    {
        Percentage,
        FixedAmount
    }
}
