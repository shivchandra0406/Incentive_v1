using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Incentive.Core.Common;
using Incentive.Core.Entities;
using Incentive.Core.Interfaces;
using Incentive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Incentive.Infrastructure.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByDealIdAsync(Guid dealId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Payment>()
                .Where(p => p.DealId == dealId)
                .Include(p => p.Deal)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Payment>()
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .Include(p => p.Deal)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByMethodAsync(string paymentMethod, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Payment>()
                .Where(p => p.PaymentMethod == paymentMethod)
                .Include(p => p.Deal)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByReceivedByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Payment>()
                .Where(p => p.ReceivedByUserId == userId)
                .Include(p => p.Deal)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Payment>> GetUnverifiedPaymentsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Payment>()
                .Where(p => !p.IsVerified)
                .Include(p => p.Deal)
                .ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalPaymentsAmountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Payment>()
                .SumAsync(p => p.Amount, cancellationToken);
        }

        public async Task<decimal> GetTotalPaymentsAmountByDealIdAsync(Guid dealId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Payment>()
                .Where(p => p.DealId == dealId)
                .SumAsync(p => p.Amount, cancellationToken);
        }

        public async Task SoftDeleteAsync(Payment entity, CancellationToken cancellationToken = default)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
