using System;
using System.Collections.Generic;

namespace Incentive.Infrastructure.Models;

public partial class IncentivePlan
{
    public Guid Id { get; set; }

    public string PlanName { get; set; } = null!;

    public string PlanType { get; set; } = null!;

    public string PeriodType { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    public string PlanDiscriminator { get; set; } = null!;

    public string? Location { get; set; }

    public string? MetricType { get; set; }

    public decimal? TargetValue { get; set; }

    public int? ConsistencyMonths { get; set; }

    public string? AwardType { get; set; }

    public decimal? AwardValue { get; set; }

    public string? GiftDescription { get; set; }

    public Guid? ProjectId { get; set; }

    public string? ProjectBasedIncentivePlanMetricType { get; set; }

    public decimal? ProjectBasedIncentivePlanTargetValue { get; set; }

    public decimal? IncentiveValue { get; set; }

    public string? CalculationType { get; set; }

    public bool? IncentiveAfterExceedingTarget { get; set; }

    public string? Role { get; set; }

    public bool? IsTeamBased { get; set; }

    public Guid? TeamId { get; set; }

    public string? TargetType { get; set; }

    public decimal? SalaryPercentage { get; set; }

    public string? RoleBasedIncentivePlanMetricType { get; set; }

    public decimal? RoleBasedIncentivePlanTargetValue { get; set; }

    public string? RoleBasedIncentivePlanCalculationType { get; set; }

    public decimal? RoleBasedIncentivePlanIncentiveValue { get; set; }

    public bool? IsCumulative { get; set; }

    public bool? RoleBasedIncentivePlanIncentiveAfterExceedingTarget { get; set; }

    public bool? IncludeSalaryInTarget { get; set; }

    public int? TargetBasedIncentivePlanTargetType { get; set; }

    public decimal? Salary { get; set; }

    public int? TargetBasedIncentivePlanMetricType { get; set; }

    public decimal? TargetBasedIncentivePlanTargetValue { get; set; }

    public int? TargetBasedIncentivePlanCalculationType { get; set; }

    public decimal? TargetBasedIncentivePlanIncentiveValue { get; set; }

    public bool? ProvideAdditionalIncentiveOnExceeding { get; set; }

    public bool? TargetBasedIncentivePlanIncentiveAfterExceedingTarget { get; set; }

    public bool? TargetBasedIncentivePlanIncludeSalaryInTarget { get; set; }

    public string? TieredIncentivePlanMetricType { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public Guid LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public string TenantId { get; set; } = null!;

    public virtual Project? Project { get; set; }

    public virtual Team? Team { get; set; }

    public virtual ICollection<TieredIncentiveTier> TieredIncentiveTiers { get; set; } = new List<TieredIncentiveTier>();
}
