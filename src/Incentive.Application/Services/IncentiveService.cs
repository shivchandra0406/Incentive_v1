using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incentive.Core.Entities;
using Incentive.Core.Enums;
using Incentive.Core.Interfaces;

namespace Incentive.Application.Services
{
    public class IncentiveService : IIncentiveService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncentiveService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IncentiveEarning> CalculateIncentiveAsync(Guid dealId, Guid userId)
        {
            var deal = await _unitOfWork.Repository<Deal>().GetByIdAsync(dealId);
            if (deal == null)
            {
                throw new Exception($"Deal with ID {dealId} not found");
            }

            if (deal.Status != DealStatus.Won && deal.Status != DealStatus.FullyPaid)
            {
                throw new Exception("Incentives can only be calculated for won or fully paid deals");
            }

            // Check if incentive already exists for this user and deal
            var existingIncentive = _unitOfWork.Repository<IncentiveEarning>().AsQueryable()
                .FirstOrDefault(i => i.UserId == userId && i.DealId == dealId);

            if (existingIncentive != null)
            {
                return existingIncentive;
            }

            // Find applicable incentive rule
            var incentiveRules = _unitOfWork.Repository<IncentiveRule>().AsQueryable()
                .Where(r => r.IsActive &&
                           (r.StartDate == null || r.StartDate <= deal.DealDate) &&
                           (r.EndDate == null || r.EndDate >= deal.DealDate) &&
                           (r.MinimumSalesThreshold == null || r.MinimumSalesThreshold <= deal.TotalAmount))
                .OrderByDescending(r => r.Commission) // Then by highest commission
                .ToList();

            if (!incentiveRules.Any())
            {
                throw new Exception("No applicable incentive rules found for this deal");
            }

            var rule = incentiveRules.First();

            // Calculate incentive amount
            decimal amount = 0;
            if (rule.Incentive == IncentiveCalculationType.PercentageOnTarget)
            {
                amount = deal.TotalAmount * (rule.Commission ?? 0) / 100;
            }
            else // Fixed amount
            {
                amount = rule.Commission ?? 0;
            }

            // Apply maximum cap if specified
            if (rule.MaximumIncentiveAmount.HasValue && amount > rule.MaximumIncentiveAmount.Value)
            {
                amount = rule.MaximumIncentiveAmount.Value;
            }

            // Create incentive earning
            var incentiveEarning = new IncentiveEarning
            {
                DealId = dealId,
                UserId = userId,
                IncentivePlanId = rule.Id,
                Amount = amount,
                EarningDate = DateTime.UtcNow,
                Status = IncentiveEarningStatus.Pending
            };

            await _unitOfWork.Repository<IncentiveEarning>().AddAsync(incentiveEarning);
            await _unitOfWork.SaveChangesAsync();

            return incentiveEarning;
        }

        public async Task<IncentiveEarning> ApproveIncentiveAsync(Guid incentiveEarningId)
        {
            var incentiveEarning = await _unitOfWork.Repository<IncentiveEarning>().GetByIdAsync(incentiveEarningId);
            if (incentiveEarning == null)
            {
                throw new Exception($"Incentive earning with ID {incentiveEarningId} not found");
            }

            incentiveEarning.Status = IncentiveEarningStatus.Approved;
            await _unitOfWork.Repository<IncentiveEarning>().UpdateAsync(incentiveEarning);
            await _unitOfWork.SaveChangesAsync();

            return incentiveEarning;
        }

        public async Task<IncentiveEarning> RejectIncentiveAsync(Guid incentiveEarningId, string reason)
        {
            var incentiveEarning = await _unitOfWork.Repository<IncentiveEarning>().GetByIdAsync(incentiveEarningId);
            if (incentiveEarning == null)
            {
                throw new Exception($"Incentive earning with ID {incentiveEarningId} not found");
            }

            incentiveEarning.Status = IncentiveEarningStatus.Rejected;
            incentiveEarning.Notes = reason;
            await _unitOfWork.Repository<IncentiveEarning>().UpdateAsync(incentiveEarning);
            await _unitOfWork.SaveChangesAsync();

            return incentiveEarning;
        }

        public async Task<IncentiveEarning> MarkAsPaidAsync(Guid incentiveEarningId)
        {
            var incentiveEarning = await _unitOfWork.Repository<IncentiveEarning>().GetByIdAsync(incentiveEarningId);
            if (incentiveEarning == null)
            {
                throw new Exception($"Incentive earning with ID {incentiveEarningId} not found");
            }

            if (incentiveEarning.Status != IncentiveEarningStatus.Approved)
            {
                throw new Exception("Only approved incentives can be marked as paid");
            }

            incentiveEarning.Status = IncentiveEarningStatus.Paid;
            incentiveEarning.IsPaid = true;
            incentiveEarning.PaidDate = DateTime.UtcNow;
            await _unitOfWork.Repository<IncentiveEarning>().UpdateAsync(incentiveEarning);
            await _unitOfWork.SaveChangesAsync();

            return incentiveEarning;
        }

        public async Task<IEnumerable<IncentiveEarning>> GetIncentiveEarningsByUserAsync(Guid userId)
        {
            return await _unitOfWork.Repository<IncentiveEarning>().GetAsync(i => i.UserId == userId);
        }

        public async Task<IEnumerable<IncentiveEarning>> GetIncentiveEarningsByDealAsync(Guid dealId)
        {
            return await _unitOfWork.Repository<IncentiveEarning>().GetAsync(i => i.DealId == dealId);
        }

        public async Task<IEnumerable<IncentiveEarning>> GetIncentiveEarningsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _unitOfWork.Repository<IncentiveEarning>().GetAsync(i =>
                i.EarningDate >= startDate && i.EarningDate <= endDate);
        }
    }
}
