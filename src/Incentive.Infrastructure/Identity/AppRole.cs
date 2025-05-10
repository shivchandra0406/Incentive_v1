using System;
using Microsoft.AspNetCore.Identity;

namespace Incentive.Infrastructure.Identity
{
    public class AppRole : IdentityRole
    {
        public string TenantId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "system";
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
    }
}
