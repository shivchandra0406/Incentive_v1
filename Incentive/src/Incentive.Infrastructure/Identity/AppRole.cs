using System;
using Microsoft.AspNetCore.Identity;

namespace Incentive.Infrastructure.Identity
{
    public class AppRole : IdentityRole
    {
        public string TenantId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
