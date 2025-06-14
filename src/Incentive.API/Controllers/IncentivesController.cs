using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Application.DTOs;
using Incentive.Core.Interfaces;
using Incentive.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IncentivesController : ControllerBase
    {
        private readonly IIncentiveService _incentiveService;
        private readonly ICurrentUserService _currentUserService;

        public IncentivesController(IIncentiveService incentiveService,ICurrentUserService currentUserService)
        {
            _incentiveService = incentiveService;
            _currentUserService = currentUserService;
        }

        [HttpPost("calculate/{dealId}")]
        public async Task<IActionResult> CalculateIncentive(Guid dealId)
        {
            try
            {
                // Get the current user ID from the claims
                var userId = _currentUserService?.GetUserId() ?? "00000000-0000-0000-0000-000000000000";
                _ = Guid.TryParse(userId, out Guid GuidUserId);
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID could not be determined");
                }

                var incentiveEarning = await _incentiveService.CalculateIncentiveAsync(dealId, GuidUserId);

                var incentiveDto = new IncentiveEarningDto
                {
                    Id = incentiveEarning.Id,
                    DealId = incentiveEarning.DealId,
                    UserId = incentiveEarning.UserId,
                    Amount = incentiveEarning.Amount,
                    EarningDate = incentiveEarning.EarningDate,
                    Status = incentiveEarning.Status.ToString(),
                    PaidDate = incentiveEarning.PaidDate
                };

                return Ok(incentiveDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("approve/{incentiveEarningId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ApproveIncentive(Guid incentiveEarningId)
        {
            try
            {
                var incentiveEarning = await _incentiveService.ApproveIncentiveAsync(incentiveEarningId);

                var incentiveDto = new IncentiveEarningDto
                {
                    Id = incentiveEarning.Id,
                    DealId = incentiveEarning.DealId,
                    UserId = incentiveEarning.UserId,
                    IncnetivePlanId = incentiveEarning.IncentivePlanId,
                    Amount = incentiveEarning.Amount,
                    EarningDate = incentiveEarning.EarningDate,
                    Status = incentiveEarning.Status.ToString(),
                    PaidDate = incentiveEarning.PaidDate
                };

                return Ok(incentiveDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reject/{incentiveEarningId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> RejectIncentive(Guid incentiveEarningId, [FromBody] string reason)
        {
            try
            {
                var incentiveEarning = await _incentiveService.RejectIncentiveAsync(incentiveEarningId, reason);

                var incentiveDto = new IncentiveEarningDto
                {
                    Id = incentiveEarning.Id,
                    DealId = incentiveEarning.DealId,
                    UserId = incentiveEarning.UserId,
                    IncnetivePlanId = incentiveEarning.IncentivePlanId,
                    Amount = incentiveEarning.Amount,
                    EarningDate = incentiveEarning.EarningDate,
                    Status = incentiveEarning.Status.ToString(),
                    PaidDate = incentiveEarning.PaidDate
                };

                return Ok(incentiveDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("mark-as-paid/{incentiveEarningId}")]
        [Authorize(Roles = "Admin,Finance")]
        public async Task<IActionResult> MarkAsPaid(Guid incentiveEarningId)
        {
            try
            {
                var incentiveEarning = await _incentiveService.MarkAsPaidAsync(incentiveEarningId);

                var incentiveDto = new IncentiveEarningDto
                {
                    Id = incentiveEarning.Id,
                    DealId = incentiveEarning.DealId,
                    UserId = incentiveEarning.UserId,
                    IncnetivePlanId = incentiveEarning.IncentivePlanId,
                    Amount = incentiveEarning.Amount,
                    EarningDate = incentiveEarning.EarningDate,
                    Status = incentiveEarning.Status.ToString(),
                    PaidDate = incentiveEarning.PaidDate
                };

                return Ok(incentiveDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var incentiveEarnings = await _incentiveService.GetIncentiveEarningsByUserAsync(userId);

            var incentiveDtos = new List<IncentiveEarningDto>();
            foreach (var incentiveEarning in incentiveEarnings)
            {
                incentiveDtos.Add(new IncentiveEarningDto
                {
                    Id = incentiveEarning.Id,
                    DealId = incentiveEarning.DealId,
                    UserId = incentiveEarning.UserId,
                    IncnetivePlanId = incentiveEarning.IncentivePlanId,
                    Amount = incentiveEarning.Amount,
                    EarningDate = incentiveEarning.EarningDate,
                    Status = incentiveEarning.Status.ToString(),
                    PaidDate = incentiveEarning.PaidDate
                });
            }

            return Ok(incentiveDtos);
        }

        [HttpGet("by-deal/{dealId}")]
        public async Task<IActionResult> GetByDeal(Guid dealId)
        {
            var incentiveEarnings = await _incentiveService.GetIncentiveEarningsByDealAsync(dealId);

            var incentiveDtos = new List<IncentiveEarningDto>();
            foreach (var incentiveEarning in incentiveEarnings)
            {
                incentiveDtos.Add(new IncentiveEarningDto
                {
                    Id = incentiveEarning.Id,
                    DealId = incentiveEarning.DealId,
                    UserId = incentiveEarning.UserId,
                    IncnetivePlanId = incentiveEarning.IncentivePlanId,
                    Amount = incentiveEarning.Amount,
                    EarningDate = incentiveEarning.EarningDate,
                    Status = incentiveEarning.Status.ToString(),
                    PaidDate = incentiveEarning.PaidDate
                });
            }

            return Ok(incentiveDtos);
        }

        [HttpGet("by-date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var incentiveEarnings = await _incentiveService.GetIncentiveEarningsByDateRangeAsync(startDate, endDate);

            var incentiveDtos = new List<IncentiveEarningDto>();
            foreach (var incentiveEarning in incentiveEarnings)
            {
                incentiveDtos.Add(new IncentiveEarningDto
                {
                    Id = incentiveEarning.Id,
                    DealId = incentiveEarning.DealId,
                    UserId = incentiveEarning.UserId,
                    IncnetivePlanId = incentiveEarning.IncentivePlanId,
                    Amount = incentiveEarning.Amount,
                    EarningDate = incentiveEarning.EarningDate,
                    Status = incentiveEarning.Status.ToString(),
                    PaidDate = incentiveEarning.PaidDate
                });
            }

            return Ok(incentiveDtos);
        }
    }
}
