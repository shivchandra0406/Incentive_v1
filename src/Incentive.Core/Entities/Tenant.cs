using System;
using Incentive.Core.Common;

namespace Incentive.Core.Entities
{
    public class Tenant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Identifier { get; set; }
        public string? ConnectionString { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}
