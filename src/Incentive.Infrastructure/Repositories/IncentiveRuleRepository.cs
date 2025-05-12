using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Incentive.Core.Common;
using Incentive.Core.Entities;
using Incentive.Core.Enums;
using Incentive.Core.Interfaces;
using Incentive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Incentive.Infrastructure.Repositories
{
    public class IncentiveRuleRepository : Repository<IncentiveRule>, IIncentiveRuleRepository
    {
        public IncentiveRuleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<IncentiveRule>> GetActiveRulesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.IncentiveRules
                .Where(r => r.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<IncentiveRule>> GetRulesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.IncentiveRules
                .Where(r => r.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<IncentiveRule>> GetRulesByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.IncentiveRules
                .Where(r => r.TeamId == teamId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<IncentiveRule>> GetRulesByFrequencyAsync(TargetFrequency frequency, CancellationToken cancellationToken = default)
        {
            return await _dbContext.IncentiveRules
                .Where(r => r.Frequency == frequency)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<IncentiveRule>> GetRulesByAppliedTypeAsync(AppliedRuleType appliedType, CancellationToken cancellationToken = default)
        {
            return await _dbContext.IncentiveRules
                .Where(r => r.AppliedTo == appliedType)
                .ToListAsync(cancellationToken);
        }

        public async Task SoftDeleteAsync(IncentiveRule entity, CancellationToken cancellationToken = default)
        {
            // Use the DbContext to delete the entity (soft delete is handled by the DbContext)
            _dbContext.IncentiveRules.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
