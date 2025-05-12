using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Incentive.Infrastructure.Identity
{
    /// <summary>
    /// Provides authorization policies for the application
    /// </summary>
    public class AuthorizationPolicyProvider
    {
        private readonly Dictionary<string, Action<AuthorizationPolicyBuilder>> _policyBuilders = new();
        private readonly ILogger<AuthorizationPolicyProvider> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationPolicyProvider"/> class.
        /// </summary>
        public AuthorizationPolicyProvider(ILogger<AuthorizationPolicyProvider> logger)
        {
            _logger = logger;
            ConfigureRolePolicies();
            ConfigurePermissionPolicies();
        }

        /// <summary>
        /// Configures the authorization options with all defined policies
        /// </summary>
        /// <param name="options">The authorization options to configure</param>
        public void ConfigurePolicies(AuthorizationOptions options)
        {
            foreach (var policy in _policyBuilders)
            {
                options.AddPolicy(policy.Key, policy.Value);
                _logger?.LogDebug("Added authorization policy: {PolicyName}", policy.Key);
            }
        }

        /// <summary>
        /// Configures role-based policies
        /// </summary>
        private void ConfigureRolePolicies()
        {
            // Role-based policies
            AddPolicy("RequireAdminRole", policy => policy.RequireRole(DefaultRoles.Admin));
            AddPolicy("RequireManagerRole", policy => policy.RequireRole(DefaultRoles.Admin, DefaultRoles.Manager));
            AddPolicy("RequireFinanceRole", policy => policy.RequireRole(DefaultRoles.Admin, "Finance"));
            AddPolicy("RequireUserRole", policy => policy.RequireRole(DefaultRoles.Admin, DefaultRoles.Manager, DefaultRoles.User));
            AddPolicy("RequireEmployeeRole", policy => policy.RequireRole(DefaultRoles.Admin, DefaultRoles.Manager, DefaultRoles.Employee));

            // Add policies for all roles defined in DefaultRoles
            foreach (var role in DefaultRoles.GetAllRoles())
            {
                AddPolicy($"Require{role}Role", policy => policy.RequireRole(role));
            }
        }

        /// <summary>
        /// Configures permission-based policies
        /// </summary>
        private void ConfigurePermissionPolicies()
        {
            // Get all permission constants from the Permissions class using reflection
            var permissionFields = typeof(Permissions)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .ToList();

            // Create a policy for each permission
            foreach (var field in permissionFields)
            {
                var permissionValue = field.GetValue(null)?.ToString();
                if (!string.IsNullOrEmpty(permissionValue))
                {
                    // Use the field name as the policy name (e.g., "ViewUsers")
                    var policyName = field.Name;
                    AddPolicy(policyName, policy => policy.RequireClaim("Permission", permissionValue));
                }
            }

            // Add combined policies for common operations
            AddCombinedPolicies();
        }

        /// <summary>
        /// Adds combined policies for common operations
        /// </summary>
        private void AddCombinedPolicies()
        {
            // Example: ManageUsers policy requires any of the user management permissions
            AddPolicy("ManageUsers", policy => policy.RequireAssertion(context =>
                HasPermission(context, Permissions.ViewUsers) ||
                HasPermission(context, Permissions.CreateUsers) ||
                HasPermission(context, Permissions.EditUsers) ||
                HasPermission(context, Permissions.DeleteUsers)));

            // Example: ManageRoles policy requires any of the role management permissions
            AddPolicy("ManageRoles", policy => policy.RequireAssertion(context =>
                HasPermission(context, Permissions.ViewRoles) ||
                HasPermission(context, Permissions.CreateRoles) ||
                HasPermission(context, Permissions.EditRoles) ||
                HasPermission(context, Permissions.DeleteRoles)));

            // Example: FullAccess policy requires admin role
            AddPolicy("FullAccess", policy => policy.RequireRole(DefaultRoles.Admin));
        }

        /// <summary>
        /// Checks if the user has a specific permission
        /// </summary>
        private static bool HasPermission(AuthorizationHandlerContext context, string permission)
        {
            return context.User.HasClaim(c =>
                c.Type == "Permission" && c.Value == permission);
        }

        /// <summary>
        /// Adds a policy to the collection
        /// </summary>
        /// <param name="name">The name of the policy</param>
        /// <param name="configurePolicy">The action to configure the policy</param>
        private void AddPolicy(string name, Action<AuthorizationPolicyBuilder> configurePolicy)
        {
            _policyBuilders[name] = configurePolicy;
        }
    }

    /// <summary>
    /// Extension methods for registering the authorization policy provider
    /// </summary>
    public static class AuthorizationPolicyProviderExtensions
    {
        /// <summary>
        /// Adds the authorization policy provider to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddAuthorizationPolicyProvider(this IServiceCollection services)
        {
            // Register the policy provider as a singleton
            services.AddSingleton<AuthorizationPolicyProvider>(provider =>
                new AuthorizationPolicyProvider(provider.GetRequiredService<ILogger<AuthorizationPolicyProvider>>())
            );

            return services;
        }

        /// <summary>
        /// Adds and configures authorization with policies from the provider
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddAuthorizationWithPolicies(this IServiceCollection services)
        {
            // Register the policy provider
            services.AddAuthorizationPolicyProvider();

            // Configure authorization with policies from the provider
            services.AddAuthorization(options =>
            {
                // Create a temporary service provider to resolve the policy provider
                var serviceProvider = services.BuildServiceProvider();
                var policyProvider = serviceProvider.GetRequiredService<AuthorizationPolicyProvider>();

                // Configure policies
                policyProvider.ConfigurePolicies(options);
            });

            return services;
        }
    }
}
