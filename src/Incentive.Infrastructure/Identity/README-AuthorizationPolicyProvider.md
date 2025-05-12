# Authorization Policy Provider

This document explains how to use the `AuthorizationPolicyProvider` to manage authorization policies in your application.

## Overview

The `AuthorizationPolicyProvider` centralizes the definition and configuration of authorization policies, making it easier to maintain and extend your application's authorization rules.

## Features

- **Automatic Policy Generation**: Automatically creates policies for all permissions defined in the `Permissions` class
- **Role-Based Policies**: Configures policies based on roles defined in the `DefaultRoles` class
- **Combined Policies**: Creates higher-level policies that combine multiple permissions
- **Reflection-Based**: Uses reflection to discover permissions, reducing the need for manual updates
- **Extensible**: Easy to add new policies or modify existing ones in a single location

## Usage

### 1. Register the Provider

The provider is automatically registered when you call `AddInfrastructure` in your `Startup.cs` or `Program.cs` file:

```csharp
services.AddInfrastructure(Configuration);
```

### 2. Use Policies in Controllers

Use the policies in your controllers with the `[Authorize]` attribute:

```csharp
[Authorize(Policy = "ViewUsers")]
public IActionResult GetUsers()
{
    // Implementation
}

[Authorize(Policy = "ManageUsers")]
public IActionResult ManageUsers()
{
    // Implementation
}

[Authorize(Policy = "RequireAdminRole")]
public IActionResult AdminOnly()
{
    // Implementation
}
```

### 3. Check Policies in Code

You can also check policies programmatically using the `IAuthorizationService`:

```csharp
public class SomeService
{
    private readonly IAuthorizationService _authorizationService;
    
    public SomeService(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }
    
    public async Task DoSomethingAsync(ClaimsPrincipal user)
    {
        var authResult = await _authorizationService.AuthorizeAsync(user, "ViewUsers");
        
        if (authResult.Succeeded)
        {
            // User has permission
        }
        else
        {
            // User doesn't have permission
        }
    }
}
```

## Available Policies

### Role-Based Policies

- `RequireAdminRole`: Requires the user to have the Admin role
- `RequireManagerRole`: Requires the user to have either the Admin or Manager role
- `RequireFinanceRole`: Requires the user to have either the Admin or Finance role
- `RequireUserRole`: Requires the user to have either the Admin, Manager, or User role
- `RequireEmployeeRole`: Requires the user to have either the Admin, Manager, or Employee role

### Permission-Based Policies

For each permission defined in the `Permissions` class, a corresponding policy is created with the same name:

- `ViewUsers`: Requires the user to have the "Permissions.Users.View" permission
- `CreateUsers`: Requires the user to have the "Permissions.Users.Create" permission
- `EditUsers`: Requires the user to have the "Permissions.Users.Edit" permission
- `DeleteUsers`: Requires the user to have the "Permissions.Users.Delete" permission
- ... and so on for all permissions

### Combined Policies

- `ManageUsers`: Requires the user to have any of the user management permissions
- `ManageRoles`: Requires the user to have any of the role management permissions
- `FullAccess`: Requires the user to have the Admin role

## Extending the Provider

### Adding New Policies

To add new policies, modify the `AuthorizationPolicyProvider` class:

```csharp
private void ConfigureCustomPolicies()
{
    // Add a new policy
    AddPolicy("CustomPolicy", policy => policy.RequireClaim("CustomClaim", "CustomValue"));
    
    // Add a policy with multiple requirements
    AddPolicy("ComplexPolicy", policy => 
    {
        policy.RequireRole("Admin");
        policy.RequireClaim("Permission", "CustomPermission");
    });
}
```

### Adding New Combined Policies

To add new combined policies, modify the `AddCombinedPolicies` method:

```csharp
private void AddCombinedPolicies()
{
    // Existing combined policies...
    
    // Add a new combined policy
    AddPolicy("ManageIncentives", policy => policy.RequireAssertion(context =>
        HasPermission(context, Permissions.ViewIncentiveRules) ||
        HasPermission(context, Permissions.CreateIncentiveRules) ||
        HasPermission(context, Permissions.EditIncentiveRules) ||
        HasPermission(context, Permissions.DeleteIncentiveRules)));
}
```

## Benefits

- **Centralized Management**: All authorization policies are defined in one place
- **Reduced Duplication**: No need to repeat policy definitions across the application
- **Automatic Discovery**: New permissions are automatically converted to policies
- **Consistency**: Ensures consistent policy naming and behavior
- **Testability**: Makes it easier to test authorization logic
