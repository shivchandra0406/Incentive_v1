using System.Collections.Generic;

namespace Incentive.Infrastructure.Identity
{
    /// <summary>
    /// Static class containing all application permissions
    /// </summary>
    public static class Permissions
    {
        // User permissions
        public const string ViewUsers = "Permissions.Users.View";
        public const string CreateUsers = "Permissions.Users.Create";
        public const string EditUsers = "Permissions.Users.Edit";
        public const string DeleteUsers = "Permissions.Users.Delete";
        
        // Role permissions
        public const string ViewRoles = "Permissions.Roles.View";
        public const string CreateRoles = "Permissions.Roles.Create";
        public const string EditRoles = "Permissions.Roles.Edit";
        public const string DeleteRoles = "Permissions.Roles.Delete";
        
        // Tenant permissions
        public const string ViewTenants = "Permissions.Tenants.View";
        public const string CreateTenants = "Permissions.Tenants.Create";
        public const string EditTenants = "Permissions.Tenants.Edit";
        public const string DeleteTenants = "Permissions.Tenants.Delete";
        
        // IncentiveRule permissions
        public const string ViewIncentiveRules = "Permissions.IncentiveRules.View";
        public const string CreateIncentiveRules = "Permissions.IncentiveRules.Create";
        public const string EditIncentiveRules = "Permissions.IncentiveRules.Edit";
        public const string DeleteIncentiveRules = "Permissions.IncentiveRules.Delete";
        
        // Deal permissions
        public const string ViewDeals = "Permissions.Deals.View";
        public const string CreateDeals = "Permissions.Deals.Create";
        public const string EditDeals = "Permissions.Deals.Edit";
        public const string DeleteDeals = "Permissions.Deals.Delete";
        
        // Payment permissions
        public const string ViewPayments = "Permissions.Payments.View";
        public const string CreatePayments = "Permissions.Payments.Create";
        public const string EditPayments = "Permissions.Payments.Edit";
        public const string DeletePayments = "Permissions.Payments.Delete";
        
        // DealActivity permissions
        public const string ViewDealActivities = "Permissions.DealActivities.View";
        public const string CreateDealActivities = "Permissions.DealActivities.Create";
        public const string EditDealActivities = "Permissions.DealActivities.Edit";
        public const string DeleteDealActivities = "Permissions.DealActivities.Delete";
        
        // IncentiveEarning permissions
        public const string ViewIncentiveEarnings = "Permissions.IncentiveEarnings.View";
        public const string CreateIncentiveEarnings = "Permissions.IncentiveEarnings.Create";
        public const string EditIncentiveEarnings = "Permissions.IncentiveEarnings.Edit";
        public const string DeleteIncentiveEarnings = "Permissions.IncentiveEarnings.Delete";

        /// <summary>
        /// Gets all permissions
        /// </summary>
        public static List<string> GetAllPermissions()
        {
            return new List<string>
            {
                // User permissions
                ViewUsers, CreateUsers, EditUsers, DeleteUsers,
                
                // Role permissions
                ViewRoles, CreateRoles, EditRoles, DeleteRoles,
                
                // Tenant permissions
                ViewTenants, CreateTenants, EditTenants, DeleteTenants,
                
                // IncentiveRule permissions
                ViewIncentiveRules, CreateIncentiveRules, EditIncentiveRules, DeleteIncentiveRules,
                
                // Deal permissions
                ViewDeals, CreateDeals, EditDeals, DeleteDeals,
                
                // Payment permissions
                ViewPayments, CreatePayments, EditPayments, DeletePayments,
                
                // DealActivity permissions
                ViewDealActivities, CreateDealActivities, EditDealActivities, DeleteDealActivities,
                
                // IncentiveEarning permissions
                ViewIncentiveEarnings, CreateIncentiveEarnings, EditIncentiveEarnings, DeleteIncentiveEarnings
            };
        }
    }
}
