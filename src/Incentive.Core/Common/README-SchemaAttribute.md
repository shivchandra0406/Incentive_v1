# Schema Attribute for Entity Framework Core

This document explains how to use the `SchemaAttribute` to automatically set database schemas for your entity classes.

## Overview

The `SchemaAttribute` allows you to specify the database schema for an entity class using a simple attribute, eliminating the need to manually call `.ToTable("TableName", "SchemaName")` for each entity in your `DbContext`.

## Usage

### 1. Apply the attribute to your entity classes

```csharp
using Incentive.Core.Common;

namespace Incentive.Core.Entities
{
    [Schema("IncentiveManagement")]
    public class Deal : SoftDeletableEntity
    {
        // Properties...
    }
}
```

### 2. The DbContext automatically applies the schema

The `AppDbContext` class has been modified to automatically read the `SchemaAttribute` from your entity classes and apply the appropriate schema during model configuration.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // Set default schema
    modelBuilder.HasDefaultSchema("dbo");
    
    // Apply entity configurations
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    
    // Apply schema attributes
    ApplySchemaAttributes(modelBuilder);
    
    // Other configurations...
}
```

### 3. Entity configurations still work

You can still use `IEntityTypeConfiguration<T>` classes to configure your entities. The schema from the `SchemaAttribute` will be applied after your entity configurations, so it will take precedence.

## How It Works

The `ApplySchemaAttributes` method uses reflection to:

1. Iterate through all entity types in the model
2. Check if the entity class has the `SchemaAttribute`
3. If it does, get the current table name (which might have been set by a configuration)
4. Apply the schema from the attribute using `.ToTable(tableName, schemaAttribute.Name)`

## Benefits

- **Cleaner code**: No need to manually specify the schema for each entity in the `DbContext`
- **Single source of truth**: The schema is defined directly on the entity class
- **Maintainability**: Easier to change schemas in the future
- **Consistency**: Ensures all entities use the same schema naming convention

## Default Schema

If no `SchemaAttribute` is found on an entity class, it will use the default schema specified in the `DbContext`:

```csharp
modelBuilder.HasDefaultSchema("dbo");
```

## Identity Tables

For Identity tables that don't use our entity classes, we still need to manually specify the schema:

```csharp
builder.Entity<AppUser>().ToTable("Users", "Identity");
builder.Entity<AppRole>().ToTable("Roles", "Identity");
// etc.
```
