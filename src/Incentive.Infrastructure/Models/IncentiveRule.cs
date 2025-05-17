using System;
using System.Collections.Generic;

namespace Incentive.Infrastructure.Models;

public partial class IncentiveRule
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Frequency { get; set; } = null!;

    public string AppliedTo { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string Target { get; set; } = null!;

    public string Incentive { get; set; } = null!;

    public decimal? Salary { get; set; }

    public decimal? TargetValue { get; set; }

    public int? TargetDealCount { get; set; }

    public decimal? Commission { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsIncludeSalary { get; set; }

    public decimal? MinimumSalesThreshold { get; set; }

    public int? MinimumDealCountThreshold { get; set; }

    public decimal? MaximumIncentiveAmount { get; set; }

    public Guid? TeamId { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public Guid LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public string TenantId { get; set; } = null!;

    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    public virtual ICollection<IncentiveEarning> IncentiveEarnings { get; set; } = new List<IncentiveEarning>();
}
