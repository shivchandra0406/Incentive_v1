using System;
using System.Linq;
using System.Threading.Tasks;
using Incentive.Application.Common.Models;
using Incentive.Domain.Entities;
using Incentive.Domain.Enums;
using Incentive.Ports.Repositories;
using Incentive.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Incentive.WebAPI.Controllers
{
    [Authorize]
    public class BookingController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<PaginatedList<Booking>>>> GetBookings([FromQuery] PaginationRequest request)
        {
            var query = _unitOfWork.Repository<Booking>().AsQueryable()
                .Where(b => !b.IsDeleted);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(b => 
                    b.CustomerName.Contains(request.SearchTerm) || 
                    b.CustomerEmail.Contains(request.SearchTerm) || 
                    b.CustomerPhone.Contains(request.SearchTerm) ||
                    b.Notes.Contains(request.SearchTerm));
            }

            var paginatedList = await PaginatedList<Booking>.CreateAsync(
                query, request.PageNumber, request.PageSize);

            return Ok(BaseResponse<PaginatedList<Booking>>.Success(paginatedList));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<Booking>>> GetBooking(Guid id)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(id);
            if (booking == null || booking.IsDeleted)
            {
                return NotFound(BaseResponse<Booking>.Failure($"Booking with ID {id} not found"));
            }

            return Ok(BaseResponse<Booking>.Success(booking));
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<Booking>>> CreateBooking(CreateBookingRequest request)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId);
            if (project == null || project.IsDeleted)
            {
                return BadRequest(BaseResponse<Booking>.Failure($"Project with ID {request.ProjectId} not found"));
            }

            var salesperson = await _unitOfWork.Repository<Salesperson>().GetByIdAsync(request.SalespersonId);
            if (salesperson == null || salesperson.IsDeleted)
            {
                return BadRequest(BaseResponse<Booking>.Failure($"Salesperson with ID {request.SalespersonId} not found"));
            }

            var booking = new Booking
            {
                ProjectId = request.ProjectId,
                SalespersonId = request.SalespersonId,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone,
                BookingDate = request.BookingDate,
                BookingValue = request.BookingValue,
                Status = BookingStatus.Pending,
                Notes = request.Notes
            };

            await _unitOfWork.Repository<Booking>().AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            // Calculate incentive if booking is confirmed
            if (request.Status == BookingStatus.Confirmed)
            {
                await CalculateIncentiveAsync(booking);
            }

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, BaseResponse<Booking>.Success(booking));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<Booking>>> UpdateBooking(Guid id, UpdateBookingRequest request)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(id);
            if (booking == null || booking.IsDeleted)
            {
                return NotFound(BaseResponse<Booking>.Failure($"Booking with ID {id} not found"));
            }

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId);
            if (project == null || project.IsDeleted)
            {
                return BadRequest(BaseResponse<Booking>.Failure($"Project with ID {request.ProjectId} not found"));
            }

            var salesperson = await _unitOfWork.Repository<Salesperson>().GetByIdAsync(request.SalespersonId);
            if (salesperson == null || salesperson.IsDeleted)
            {
                return BadRequest(BaseResponse<Booking>.Failure($"Salesperson with ID {request.SalespersonId} not found"));
            }

            var oldStatus = booking.Status;
            
            booking.ProjectId = request.ProjectId;
            booking.SalespersonId = request.SalespersonId;
            booking.CustomerName = request.CustomerName;
            booking.CustomerEmail = request.CustomerEmail;
            booking.CustomerPhone = request.CustomerPhone;
            booking.BookingDate = request.BookingDate;
            booking.BookingValue = request.BookingValue;
            booking.Status = request.Status;
            booking.Notes = request.Notes;

            await _unitOfWork.Repository<Booking>().UpdateAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            // Calculate incentive if status changed to confirmed
            if (oldStatus != BookingStatus.Confirmed && request.Status == BookingStatus.Confirmed)
            {
                await CalculateIncentiveAsync(booking);
            }

            return Ok(BaseResponse<Booking>.Success(booking));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<object>>> DeleteBooking(Guid id)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(id);
            if (booking == null || booking.IsDeleted)
            {
                return NotFound(BaseResponse<object>.Failure($"Booking with ID {id} not found"));
            }

            await _unitOfWork.Repository<Booking>().DeleteAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<object>.Success(null, "Booking deleted successfully"));
        }

        [HttpPost("{id}/calculate-incentive")]
        public async Task<ActionResult<BaseResponse<IncentiveEarning>>> CalculateIncentive(Guid id)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(id);
            if (booking == null || booking.IsDeleted)
            {
                return NotFound(BaseResponse<IncentiveEarning>.Failure($"Booking with ID {id} not found"));
            }

            if (booking.Status != BookingStatus.Confirmed)
            {
                return BadRequest(BaseResponse<IncentiveEarning>.Failure("Incentive can only be calculated for confirmed bookings"));
            }

            var incentiveEarning = await CalculateIncentiveAsync(booking);
            if (incentiveEarning == null)
            {
                return BadRequest(BaseResponse<IncentiveEarning>.Failure("No applicable incentive rule found for this booking"));
            }

            return Ok(BaseResponse<IncentiveEarning>.Success(incentiveEarning));
        }

        private async Task<IncentiveEarning> CalculateIncentiveAsync(Booking booking)
        {
            // Find applicable incentive rules
            var incentiveRules = await _unitOfWork.Repository<IncentiveRule>().AsQueryable()
                .Where(r => !r.IsDeleted && r.IsActive)
                .Where(r => r.ProjectId == null || r.ProjectId == booking.ProjectId)
                .Where(r => r.StartDate <= booking.BookingDate && (r.EndDate == null || r.EndDate >= booking.BookingDate))
                .Where(r => r.MinBookingValue == null || r.MinBookingValue <= booking.BookingValue)
                .Where(r => r.MaxBookingValue == null || r.MaxBookingValue >= booking.BookingValue)
                .OrderByDescending(r => r.Value)
                .ToListAsync();

            if (!incentiveRules.Any())
            {
                return null;
            }

            // Get the best incentive rule
            var bestRule = incentiveRules.First();

            // Calculate incentive amount
            decimal incentiveAmount = 0;
            switch (bestRule.Type)
            {
                case IncentiveType.FixedAmount:
                    incentiveAmount = bestRule.Value;
                    break;
                case IncentiveType.Percentage:
                    incentiveAmount = booking.BookingValue * (bestRule.Value / 100);
                    break;
                case IncentiveType.Tiered:
                    // For tiered, we assume the value is the percentage
                    incentiveAmount = booking.BookingValue * (bestRule.Value / 100);
                    break;
            }

            // Check if there's already an incentive earning for this booking
            var existingEarning = await _unitOfWork.Repository<IncentiveEarning>().AsQueryable()
                .FirstOrDefaultAsync(e => e.BookingId == booking.Id);

            if (existingEarning != null)
            {
                existingEarning.IncentiveRuleId = bestRule.Id;
                existingEarning.Amount = incentiveAmount;
                existingEarning.EarningDate = DateTime.UtcNow;
                existingEarning.Status = IncentiveEarningStatus.Pending;

                await _unitOfWork.Repository<IncentiveEarning>().UpdateAsync(existingEarning);
                await _unitOfWork.SaveChangesAsync();

                return existingEarning;
            }
            else
            {
                var incentiveEarning = new IncentiveEarning
                {
                    BookingId = booking.Id,
                    SalespersonId = booking.SalespersonId,
                    IncentiveRuleId = bestRule.Id,
                    Amount = incentiveAmount,
                    EarningDate = DateTime.UtcNow,
                    Status = IncentiveEarningStatus.Pending
                };

                await _unitOfWork.Repository<IncentiveEarning>().AddAsync(incentiveEarning);
                await _unitOfWork.SaveChangesAsync();

                return incentiveEarning;
            }
        }
    }
}
