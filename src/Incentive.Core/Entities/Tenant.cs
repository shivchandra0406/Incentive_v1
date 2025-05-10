using System;
using Incentive.Core.Common;

namespace Incentive.Core.Entities
{
    public class Tenant : AuditableEntity
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string ConnectionString { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
