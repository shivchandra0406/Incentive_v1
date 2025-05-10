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
    public class IncentiveEarningController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncentiveEarningController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<PaginatedList<IncentiveEarning>>>> GetIncentiveEarnings([FromQuery] PaginationRequest request)
        {
            var query = _unitOfWork.Repository<IncentiveEarning>().AsQueryable()
                .Where(e => !e.IsDeleted);

            var paginatedList = await PaginatedList<IncentiveEarning>.CreateAsync(
                query, request.PageNumber, request.PageSize);

            return Ok(BaseResponse<PaginatedList<IncentiveEarning>>.Success(paginatedList));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<IncentiveEarning>>> GetIncentiveEarning(Guid id)
        {
            var incentiveEarning = await _unitOfWork.Repository<IncentiveEarning>().GetByIdAsync(id);
            if (incentiveEarning == null || incentiveEarning.IsDeleted)
            {
                return NotFound(BaseResponse<IncentiveEarning>.Failure($"Incentive earning with ID {id} not found"));
            }

            return Ok(BaseResponse<IncentiveEarning>.Success(incentiveEarning));
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse<IncentiveEarning>>> UpdateIncentiveEarningStatus(Guid id, UpdateIncentiveEarningStatusRequest request)
        {
            var incentiveEarning = await _unitOfWork.Repository<IncentiveEarning>().GetByIdAsync(id);
            if (incentiveEarning == null || incentiveEarning.IsDeleted)
            {
                return NotFound(BaseResponse<IncentiveEarning>.Failure($"Incentive earning with ID {id} not found"));
            }

            incentiveEarning.Status = request.Status;
            
            if (request.Status == IncentiveEarningStatus.Paid)
            {
                incentiveEarning.PaidDate = DateTime.UtcNow;
            }
            else
            {
                incentiveEarning.PaidDate = null;
            }

            await _unitOfWork.Repository<IncentiveEarning>().UpdateAsync(incentiveEarning);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<IncentiveEarning>.Success(incentiveEarning));
        }

        [HttpGet("report")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse<IncentiveReport>>> GenerateReport([FromQuery] IncentiveReportRequest request)
        {
            var query = _unitOfWork.Repository<IncentiveEarning>().AsQueryable()
                .Where(e => !e.IsDeleted);

            // Apply filters
            if (request.SalespersonId.HasValue)
            {
                query = query.Where(e => e.SalespersonId == request.SalespersonId.Value);
            }

            if (request.ProjectId.HasValue)
            {
                query = query.Where(e => e.Booking.ProjectId == request.ProjectId.Value);
            }

            if (request.Status.HasValue)
            {
                query = query.Where(e => e.Status == request.Status.Value);
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(e => e.EarningDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(e => e.EarningDate <= request.EndDate.Value);
            }

            // Get the data
            var earnings = await query
                .Include(e => e.Salesperson)
                .Include(e => e.Booking)
                .Include(e => e.IncentiveRule)
                .ToListAsync();

            // Generate the report
            var report = new IncentiveReport
            {
                TotalEarnings = earnings.Count,
                TotalAmount = earnings.Sum(e => e.Amount),
                PendingAmount = earnings.Where(e => e.Status == IncentiveEarningStatus.Pending).Sum(e => e.Amount),
                ApprovedAmount = earnings.Where(e => e.Status == IncentiveEarningStatus.Approved).Sum(e => e.Amount),
                PaidAmount = earnings.Where(e => e.Status == IncentiveEarningStatus.Paid).Sum(e => e.Amount),
                RejectedAmount = earnings.Where(e => e.Status == IncentiveEarningStatus.Rejected).Sum(e => e.Amount),
                Earnings = earnings
            };

            return Ok(BaseResponse<IncentiveReport>.Success(report));
        }
    }
}
