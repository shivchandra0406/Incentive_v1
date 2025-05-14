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
    public class DealRepository : Repository<Deal>, IDealRepository
    {
        public DealRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Deal>> GetDealsByStatusAsync(DealStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Deals
                .Where(d => d.Status == status)
                .Include(d => d.Payments)
                .Include(d => d.Activities)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Deal>> GetDealsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Deals
                .Where(d => d.UserId == userId.ToString() || d.ClosedByUserId == userId.ToString())
                .Include(d => d.Payments)
                .Include(d => d.Activities)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Deal>> GetDealsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Deals
                .Where(d => d.DealDate >= startDate && d.DealDate <= endDate)
                .Include(d => d.Payments)
                .Include(d => d.Activities)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Deal>> GetDealsByIncentiveRuleIdAsync(Guid incentiveRuleId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Deals
                .Where(d => d.IncentiveRuleId == incentiveRuleId)
                .Include(d => d.Payments)
                .Include(d => d.Activities)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Deal>> GetDealsByCustomerNameAsync(string customerName, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Deals
                .Where(d => d.CustomerName.Contains(customerName))
                .Include(d => d.Payments)
                .Include(d => d.Activities)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Deal>> GetDealsBySourceAsync(string source, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Deals
                .Where(d => d.Source == source)
                .Include(d => d.Payments)
                .Include(d => d.Activities)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Deal>> GetDealsWithPendingPaymentsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Deals
                .Where(d => d.RemainingAmount > 0)
                .Include(d => d.Payments)
                .Include(d => d.Activities)
                .ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalDealAmountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Deals
                .SumAsync(d => d.TotalAmount, cancellationToken);
        }

        public async Task<decimal> GetTotalPaidAmountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Deals
                .SumAsync(d => d.PaidAmount, cancellationToken);
        }

        public async Task<decimal> GetTotalRemainingAmountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Deals
                .SumAsync(d => d.RemainingAmount, cancellationToken);
        }

        public async Task SoftDeleteAsync(Deal entity, CancellationToken cancellationToken = default)
        {
            // Use the DbContext to delete the entity (soft delete is handled by the DbContext)
            _dbContext.Deals.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
