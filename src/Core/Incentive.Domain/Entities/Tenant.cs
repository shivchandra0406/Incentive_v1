using System;
using Incentive.Domain.Common;

namespace Incentive.Domain.Entities
{
    public class Tenant : AuditableEntity
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string ConnectionString { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
