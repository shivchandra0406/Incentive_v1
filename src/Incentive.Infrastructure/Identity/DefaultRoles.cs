using System.Collections.Generic;

namespace Incentive.Infrastructure.Identity
{
    /// <summary>
    /// Static class containing default roles and their permissions
    /// </summary>
    public static class DefaultRoles
    {
        // Role names
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";
        public const string ReadOnly = "ReadOnly";
        public const string Employee = "Employee";

        /// <summary>
        /// Gets all default roles
        /// </summary>
        public static List<string> GetAllRoles()
        {
            return new List<string> { Admin, Manager, User, ReadOnly, Employee };
        }

        /// <summary>
        /// Gets the permissions for a specific role
        /// </summary>
        public static List<string> GetPermissionsForRole(string roleName)
        {
            return roleName switch
            {
                Admin => GetAdminPermissions(),
                Manager => GetManagerPermissions(),
                User => GetUserPermissions(),
                ReadOnly => GetReadOnlyPermissions(),
                Employee => GetEmployeePermissions(),
                _ => new List<string>()
            };
        }

        /// <summary>
        /// Gets the description for a specific role
        /// </summary>
        public static string GetRoleDescription(string roleName)
        {
            return roleName switch
            {
                Admin => "Full access to all system features",
                Manager => "Manage users, deals, and incentives",
                User => "Regular user with basic permissions",
                ReadOnly => "Read-only access to system data",
                _ => string.Empty
            };
        }

        /// <summary>
        /// Gets all permissions for the Admin role
        /// </summary>
        private static List<string> GetAdminPermissions()
        {
            // Admin has all permissions
            return Permissions.GetAllPermissions();
        }
        public static List<string> GetDefaultPermission()
        {
            return new List<string>
            {
                Permissions.ViewUsers,
                Permissions.CreateUsers,
                Permissions.EditUsers,
                Permissions.DeleteUsers,
                
                // Role permissions
                Permissions.ViewRoles,
                Permissions.CreateRoles,
                Permissions.EditRoles,
                Permissions.DeleteRoles,
                
                // IncentiveRule permissions
                Permissions.ViewIncentiveRules,
                Permissions.CreateIncentiveRules,
                Permissions.EditIncentiveRules,
                Permissions.DeleteUsers,
                
                // Deal permissions
                Permissions.ViewDeals,
                Permissions.CreateDeals,
                Permissions.EditDeals,
                Permissions.DeleteDeals,
                
                // Payment permissions
                Permissions.ViewPayments,
                Permissions.CreatePayments,
                Permissions.EditPayments,
               
            };

        }
        /// <summary>
        /// Gets all permissions for the Manager role
        /// </summary>
        private static List<string> GetManagerPermissions()
        {
            return new List<string>
            {
                // User permissions
                Permissions.ViewUsers,
                Permissions.CreateUsers,
                Permissions.EditUsers,
                
                // Role permissions
                Permissions.ViewRoles,
               
                // IncentiveRule permissions
                Permissions.ViewIncentiveRules,
                Permissions.CreateIncentiveRules,
                Permissions.EditIncentiveRules,
                
                // Deal permissions
                Permissions.ViewDeals,
                Permissions.CreateDeals,
                Permissions.EditDeals,
                Permissions.DeleteDeals,
                
                // Payment permissions
                Permissions.ViewPayments,
                Permissions.CreatePayments,
                Permissions.EditPayments,
                
                // IncentiveEarning permissions
                Permissions.ViewIncentiveEarnings,
                Permissions.CreateIncentiveEarnings,
                Permissions.EditIncentiveEarnings
            };
        }

        /// <summary>
        /// Gets all permissions for the User role
        /// </summary>
        private static List<string> GetUserPermissions()
        {
            return new List<string>
            {
                // Deal permissions
                Permissions.ViewDeals,
                Permissions.CreateDeals,
                Permissions.EditDeals,
                
                // Payment permissions
                Permissions.ViewPayments,
             
                // IncentiveRule permissions
                Permissions.ViewIncentiveRules,
                
                // IncentiveEarning permissions
                Permissions.ViewIncentiveEarnings
            };
        }

        /// <summary>
        /// Gets all permissions for the ReadOnly role
        /// </summary>
        private static List<string> GetReadOnlyPermissions()
        {
            return new List<string>
            {
                // View-only permissions
                Permissions.ViewUsers,
                Permissions.ViewRoles,
                Permissions.ViewTenants,
                Permissions.ViewIncentiveRules,
                Permissions.ViewDeals,
                Permissions.ViewPayments,
                Permissions.ViewIncentiveEarnings
            };
        }

        private static List<string> GetEmployeePermissions()
        {
            return new List<string>
            {
                Permissions.ViewIncentiveRules,
                Permissions.ViewDeals,
                Permissions.ViewPayments,
                Permissions.ViewIncentiveEarnings
            };
        }
    }
}
