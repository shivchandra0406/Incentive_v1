# Incentive Management System Documentation

This folder contains documentation for the Incentive Management System API.

## Files

- `api-documentation.json`: Comprehensive API documentation in JSON format, including endpoints, request/response formats, and change history.

## Recent Changes

### 2025-05-15: Added Entity Configurations for Incentive Plan Types

- **Added Configuration Files**:
  - Created configuration files for all incentive plan types:
    - `RoleBasedIncentivePlanConfiguration`
    - `ProjectBasedIncentivePlanConfiguration`
    - `KickerIncentivePlanConfiguration`
    - `TieredIncentivePlanConfiguration`
  - Configured proper relationships between entities
  - Added proper column types and constraints
  - Configured enum conversions to use string representation in the database

- **Migration**: Created and applied `AddIncentivePlanConfigurations` migration
  - Changed enum columns to use string representation
  - Added foreign key constraints for relationships
  - Updated delete behavior for relationships

- **Table-Per-Hierarchy (TPH) Inheritance**:
  - The application uses TPH inheritance for incentive plans
  - All incentive plan types are stored in a single table (`IncentivePlans`)
  - A discriminator column (`PlanDiscriminator`) is used to distinguish between different types
  - This is why you don't see separate tables for each incentive plan type
  - The `DbSet<>` properties in `AppDbContext` are used to query specific incentive plan types

### 2025-05-15: Updated TargetBasedIncentivePlan Entity

- **Renamed Field**: `SalaryPercentage` → `Salary`
  - Improved clarity by using a more descriptive name
  - The field still stores the same data (decimal value representing salary percentage)

- **Added Field**: `ProvideAdditionalIncentiveOnExceeding` (boolean)
  - Controls whether additional incentives are provided when targets are exceeded
  - Default value: `false`
  - This replaces the previous `IsCumulative` field with a more descriptive name

- **Migration**: Created and applied `UpdateTargetBasedIncentivePlan` migration

## Entity Structure

### IncentivePlanBase (Abstract Base Class)

```csharp
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
```

### TargetBasedIncentivePlan

```csharp
public class TargetBasedIncentivePlan : IncentivePlanBase
{
    [Required]
    public TargetType TargetType { get; set; }

    // If TargetType == SalaryBased
    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Salary { get; set; }

    [Required]
    public MetricType MetricType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TargetValue { get; set; }

    [Required]
    public IncentiveCalculationType CalculationType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal IncentiveValue { get; set; }

    [Required]
    public bool IncentiveAfterExceedingTarget { get; set; } = true;

    public bool ProvideAdditionalIncentiveOnExceeding { get; set; } = false;

    [Required]
    public bool IncludeSalaryInTarget { get; set; } = false;
}
```

### RoleBasedIncentivePlan

```csharp
public class RoleBasedIncentivePlan : IncentivePlanBase
{
    [Required]
    [StringLength(100)]
    public string Role { get; set; } = string.Empty;

    [Required]
    public bool IsTeamBased { get; set; } = false;

    // If IsTeamBased == true
    public Guid? TeamId { get; set; }

    [Required]
    public TargetType TargetType { get; set; }

    // If TargetType == SalaryBased
    [Column(TypeName = "decimal(5, 2)")]
    public decimal? SalaryPercentage { get; set; }

    [Required]
    public MetricType MetricType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TargetValue { get; set; }

    [Required]
    public IncentiveCalculationType CalculationType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal IncentiveValue { get; set; }

    [Required]
    public bool IsCumulative { get; set; } = false;

    [Required]
    public bool IncentiveAfterExceedingTarget { get; set; } = true;

    [Required]
    public bool IncludeSalaryInTarget { get; set; } = false;
}
```

### ProjectBasedIncentivePlan

```csharp
public class ProjectBasedIncentivePlan : IncentivePlanBase
{
    [Required]
    public Guid ProjectId { get; set; }

    [ForeignKey("ProjectId")]
    public virtual Project Project { get; set; }

    [Required]
    public MetricType MetricType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TargetValue { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal IncentiveValue { get; set; }

    [Required]
    public IncentiveCalculationType CalculationType { get; set; }

    [Required]
    public bool IncentiveAfterExceedingTarget { get; set; } = true;
}
```

### KickerIncentivePlan

```csharp
public class KickerIncentivePlan : IncentivePlanBase
{
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;

    [Required]
    public MetricType MetricType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TargetValue { get; set; }

    [Required]
    public int ConsistencyMonths { get; set; } // 3, 6, 12 etc.

    [Required]
    public AwardType AwardType { get; set; }

    // If AwardType == Cash
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AwardValue { get; set; }
}
```

### TieredIncentivePlan

```csharp
public class TieredIncentivePlan : IncentivePlanBase
{
    [Required]
    public MetricType MetricType { get; set; }

    public virtual ICollection<TieredIncentiveTier> Tiers { get; set; } = new List<TieredIncentiveTier>();
}
```

### TieredIncentiveTier

```csharp
public class TieredIncentiveTier : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string TenantId { get; set; }

    [Required]
    public Guid TieredIncentivePlanId { get; set; }

    [ForeignKey("TieredIncentivePlanId")]
    public virtual TieredIncentivePlan TieredIncentivePlan { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal FromValue { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ToValue { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal IncentiveValue { get; set; }

    [Required]
    public IncentiveCalculationType CalculationType { get; set; }
}
```

## API Usage

When creating or updating a target-based incentive plan, use the following properties:

```json
{
  "planName": "Q2 Sales Target",
  "planType": "Target",
  "periodType": "Quarterly",
  "targetType": "SalaryBased",
  "salary": 10.5,
  "metricType": "Revenue",
  "targetValue": 100000,
  "calculationType": "Percentage",
  "incentiveValue": 5,
  "incentiveAfterExceedingTarget": true,
  "provideAdditionalIncentiveOnExceeding": true,
  "includeSalaryInTarget": false
}
```
