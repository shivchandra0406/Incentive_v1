using System;
using System.Collections.Generic;

namespace Incentive.Infrastructure.Models;

public partial class TieredIncentiveTier
{
    public Guid Id { get; set; }

    public Guid TieredIncentivePlanId { get; set; }

    public decimal FromValue { get; set; }

    public decimal ToValue { get; set; }

    public decimal IncentiveValue { get; set; }

    public string CalculationType { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public Guid LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public string TenantId { get; set; } = null!;

    public virtual IncentivePlan TieredIncentivePlan { get; set; } = null!;
}
