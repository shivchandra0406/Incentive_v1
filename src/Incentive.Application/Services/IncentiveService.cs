using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incentive.Core.Entities;
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

        public async Task<IncentiveEarning> CalculateIncentiveAsync(Guid bookingId)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new Exception($"Booking with ID {bookingId} not found");
            }

            if (booking.Status != BookingStatus.Confirmed)
            {
                throw new Exception("Incentives can only be calculated for confirmed bookings");
            }

            // Check if incentive already exists for this booking
            var existingIncentive = _unitOfWork.Repository<IncentiveEarning>().AsQueryable()
                .FirstOrDefault(i => i.BookingId == bookingId);

            if (existingIncentive != null)
            {
                return existingIncentive;
            }

            // Find applicable incentive rule
            var incentiveRules = _unitOfWork.Repository<IncentiveRule>().AsQueryable()
                .Where(r => r.IsActive && 
                           (r.ProjectId == booking.ProjectId || r.ProjectId == null) &&
                           r.StartDate <= booking.BookingDate &&
                           (r.EndDate == null || r.EndDate >= booking.BookingDate) &&
                           (r.MinBookingValue == null || r.MinBookingValue <= booking.BookingValue) &&
                           (r.MaxBookingValue == null || r.MaxBookingValue >= booking.BookingValue))
                .OrderByDescending(r => r.ProjectId != null) // Prioritize project-specific rules
                .ThenByDescending(r => r.Value) // Then by highest value
                .ToList();

            if (!incentiveRules.Any())
            {
                throw new Exception("No applicable incentive rules found for this booking");
            }

            var rule = incentiveRules.First();
            
            // Calculate incentive amount
            decimal amount = 0;
            if (rule.Type == IncentiveType.Percentage)
            {
                amount = booking.BookingValue * (rule.Value / 100);
            }
            else // Fixed amount
            {
                amount = rule.Value;
            }

            // Create incentive earning
            var incentiveEarning = new IncentiveEarning
            {
                BookingId = bookingId,
                SalespersonId = booking.SalespersonId,
                IncentiveRuleId = rule.Id,
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
            incentiveEarning.PaidDate = DateTime.UtcNow;
            await _unitOfWork.Repository<IncentiveEarning>().UpdateAsync(incentiveEarning);
            await _unitOfWork.SaveChangesAsync();

            return incentiveEarning;
        }

        public async Task<IEnumerable<IncentiveEarning>> GetIncentiveEarningsBySalespersonAsync(Guid salespersonId)
        {
            return await _unitOfWork.Repository<IncentiveEarning>().GetAsync(i => i.SalespersonId == salespersonId);
        }

        public async Task<IEnumerable<IncentiveEarning>> GetIncentiveEarningsByProjectAsync(Guid projectId)
        {
            var bookings = await _unitOfWork.Repository<Booking>().GetAsync(b => b.ProjectId == projectId);
            var bookingIds = bookings.Select(b => b.Id).ToList();
            
            return await _unitOfWork.Repository<IncentiveEarning>().GetAsync(i => bookingIds.Contains(i.BookingId));
        }

        public async Task<IEnumerable<IncentiveEarning>> GetIncentiveEarningsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _unitOfWork.Repository<IncentiveEarning>().GetAsync(i => 
                i.EarningDate >= startDate && i.EarningDate <= endDate);
        }
    }
}
