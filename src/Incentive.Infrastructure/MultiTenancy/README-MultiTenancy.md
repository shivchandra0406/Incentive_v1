# Multi-Tenancy Implementation

This document explains how multi-tenancy is implemented in the Incentive Management System.

## Overview

The system uses a multi-tenant architecture where data is segregated by tenant. Each tenant has its own set of data, but the application and database are shared across all tenants.

## Key Components

### 1. Entity Hierarchy

The entity hierarchy is designed to support multi-tenancy:

```
BaseEntity
└── AuditableEntity
    └── MultiTenantEntity
        └── SoftDeletableEntity
            └── Business Entities (Deal, IncentiveRule, etc.)
```

All business entities inherit from `SoftDeletableEntity`, which inherits from `MultiTenantEntity`, ensuring that all entities have a `TenantId` property.

### 2. Tenant Resolution

Tenant resolution is handled by the `TenantProvider` class, which implements the `ITenantProvider` interface. It resolves the tenant ID from:

1. HTTP header (`X-Tenant-ID`)
2. User claims (`TenantId`)
3. Default tenant ID (for system operations)

### 3. Tenant Middleware

The `TenantMiddleware` ensures that all requests have a valid tenant ID. It:

1. Extracts the tenant ID from the HTTP header or user claims
2. Validates the tenant ID against the database
3. Rejects requests without a valid tenant ID

### 4. Global Query Filters

Entity Framework Core global query filters are used to automatically filter data by tenant:

```csharp
// Apply multi-tenancy filter
if (typeof(MultiTenantEntity).IsAssignableFrom(entityType.ClrType))
{
    var currentTenantId = GetCurrentTenantId();

    var parameter = Expression.Parameter(entityType.ClrType, "e");
    var tenantIdProperty = Expression.Property(parameter, "TenantId");
    var tenantIdConstant = Expression.Constant(currentTenantId);
    var tenantIdComparison = Expression.Equal(tenantIdProperty, tenantIdConstant);

    var lambda = Expression.Lambda(tenantIdComparison, parameter);
    builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
}
```

This ensures that queries only return data for the current tenant.

### 5. Automatic Tenant ID Assignment

The `SaveChangesAsync` method in the `AppDbContext` automatically assigns the current tenant ID to new entities:

```csharp
// Apply multi-tenancy
foreach (var entry in ChangeTracker.Entries<MultiTenantEntity>())
{
    if (entry.State == EntityState.Added)
    {
        entry.Entity.TenantId = tenantId;
    }
}
```

## Usage

### 1. Creating Entities

When creating new entities, you don't need to explicitly set the `TenantId` property. It will be automatically set by the `SaveChangesAsync` method.

```csharp
var deal = new Deal
{
    DealName = "New Deal",
    CustomerName = "Customer Name",
    // ...
};

await _dbContext.Deals.AddAsync(deal);
await _dbContext.SaveChangesAsync();
```

### 2. Querying Entities

When querying entities, you don't need to explicitly filter by tenant ID. The global query filter will automatically apply the filter.

```csharp
var deals = await _dbContext.Deals.ToListAsync();
```

This will only return deals for the current tenant.

### 3. Setting the Tenant ID in Requests

To set the tenant ID in requests, include the `X-Tenant-ID` header:

```http
GET /api/deals HTTP/1.1
Host: example.com
X-Tenant-ID: 12345
Authorization: Bearer <token>
```

### 4. Getting the Tenant ID in Controllers

You can use the `GetTenantId` extension method to get the tenant ID in controllers:

```csharp
[HttpGet("deals")]
public async Task<ActionResult<IEnumerable<Deal>>> GetDeals()
{
    // Get tenant ID from request
    var tenantId = this.GetTenantId();

    // Log tenant ID
    _logger.LogInformation("Getting deals for tenant: {TenantId}", tenantId);

    // Get deals for tenant (tenant filter is automatically applied)
    var deals = await _dbContext.Deals.ToListAsync();

    return Ok(deals);
}
```

### 5. Setting the Tenant ID in User Claims

The tenant ID can also be included in the user's claims:

```csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.UserName),
    new Claim(ClaimTypes.NameIdentifier, user.Id),
    new Claim("TenantId", user.TenantId)
};
```

## Bypassing Tenant Filtering

In some cases, you may need to bypass the tenant filter, for example, when performing cross-tenant operations. You can do this using the `IgnoreQueryFilters` method:

```csharp
var allDeals = await _dbContext.Deals.IgnoreQueryFilters().ToListAsync();
```

This will return all deals across all tenants.

## Tenant Management

Tenants are managed through the `TenantService` class, which implements the `ITenantService` interface. It provides methods for:

1. Creating tenants
2. Updating tenants
3. Deleting tenants
4. Getting tenant information

## Security Considerations

1. Always validate tenant IDs to prevent tenant data leakage
2. Use the tenant middleware to ensure all requests have a valid tenant ID
3. Use global query filters to automatically filter data by tenant
4. Be careful when bypassing tenant filters
