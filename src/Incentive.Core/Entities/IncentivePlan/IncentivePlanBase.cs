using System;
using System.ComponentModel.DataAnnotations;
using Incentive.Core.Common;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities.IncentivePlan
{
    /// <summary>
    /// Base class for all incentive plans
    /// </summary>
    [Schema("IncentiveManagement")]
    public abstract class IncentivePlanBase : MultiTenantEntity
    {
        [Required]
        [StringLength(200)]
        public string PlanName { get; set; } = string.Empty;

        [Required]
        public IncentivePlanType PlanType { get; set; }

        [Required]
        public PeriodType PeriodType { get; set; }

        // If PeriodType == Custom
        public DateTime? StartDate { get; set; }

        // If PeriodType == Custom
        public DateTime? EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // Discriminator for EF Core TPH inheritance
        public string PlanDiscriminator { get; set; } = string.Empty;
    }
}
