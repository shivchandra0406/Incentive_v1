using System;
using System.Collections.Generic;

namespace Incentive.Infrastructure.Models;

public partial class IncentiveEarning
{
    public Guid Id { get; set; }

    public Guid IncentiveRuleId { get; set; }

    public string UserId { get; set; } = null!;

    public Guid DealId { get; set; }

    public decimal Amount { get; set; }

    public DateTime EarningDate { get; set; }

    public string Status { get; set; } = null!;

    public bool IsPaid { get; set; }

    public DateTime? PaidDate { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public Guid LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public string TenantId { get; set; } = null!;

    public virtual Deal Deal { get; set; } = null!;

    public virtual IncentiveRule IncentiveRule { get; set; } = null!;
}
