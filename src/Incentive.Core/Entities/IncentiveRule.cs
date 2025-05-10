using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Common;

namespace Incentive.Core.Entities
{
    /// <summary>
    /// Represents an incentive rule that can be applied to users or teams
    /// </summary>
    public class IncentiveRule : SoftDeletableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Original properties
        public Guid? ProjectId { get; set; }
        public IncentiveType Type { get; set; }
        public decimal Value { get; set; }
        public decimal? MinBookingValue { get; set; }
        public decimal? MaxBookingValue { get; set; }

        // New properties from the provided model
        public string Frequency { get; set; } // Will be mapped to TargetFrequency enum
        public string AppliedTo { get; set; } // Will be mapped to AppliedRuleType enum
        public string CurrencyType { get; set; }
        public string TargetType { get; set; } // Will be mapped to TargetType enum
        public string IncentiveType { get; set; } // Will be mapped to IncentiveCalculationType enum

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual ICollection<IncentiveEarning> IncentiveEarnings { get; set; } = new List<IncentiveEarning>();

        // User relationship
        public string UserId { get; set; }

        // Team relationship (if we implement teams later)
        public Guid? TeamId { get; set; }
    }

    // Moving this enum to Enums folder
    public enum IncentiveType
    {
        Percentage,
        FixedAmount
    }
}
