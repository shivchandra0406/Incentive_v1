using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Incentive.Core.Entities;
using Incentive.Core.Enums;

namespace Incentive.Core.Interfaces
{
    public interface IIncentiveRuleRepository : IRepository<IncentiveRule>
    {
        Task<IReadOnlyList<IncentiveRule>> GetActiveRulesAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IncentiveRule>> GetRulesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IncentiveRule>> GetRulesByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IncentiveRule>> GetRulesByFrequencyAsync(TargetFrequency frequency, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IncentiveRule>> GetRulesByAppliedTypeAsync(AppliedRuleType appliedType, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(IncentiveRule entity, CancellationToken cancellationToken = default);
    }
}
