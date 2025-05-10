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
    public class DealActivityRepository : Repository<DealActivity>, IDealActivityRepository
    {
        public DealActivityRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<DealActivity>> GetActivitiesByDealIdAsync(Guid dealId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<DealActivity>()
                .Where(a => a.DealId == dealId)
                .Include(a => a.Deal)
                .OrderByDescending(a => a.ActivityDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<DealActivity>> GetActivitiesByTypeAsync(ActivityType type, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<DealActivity>()
                .Where(a => a.Type == type)
                .Include(a => a.Deal)
                .OrderByDescending(a => a.ActivityDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<DealActivity>> GetActivitiesByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<DealActivity>()
                .Where(a => a.UserId == userId)
                .Include(a => a.Deal)
                .OrderByDescending(a => a.ActivityDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<DealActivity>> GetActivitiesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<DealActivity>()
                .Where(a => a.ActivityDate >= startDate && a.ActivityDate <= endDate)
                .Include(a => a.Deal)
                .OrderByDescending(a => a.ActivityDate)
                .ToListAsync(cancellationToken);
        }

        public async Task SoftDeleteAsync(DealActivity entity, CancellationToken cancellationToken = default)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
