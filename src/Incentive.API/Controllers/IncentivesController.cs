using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Application.DTOs;
using Incentive.Core.Interfaces;
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

        public IncentivesController(IIncentiveService incentiveService)
        {
            _incentiveService = incentiveService;
        }

        [HttpPost("calculate/{bookingId}")]
        public async Task<IActionResult> CalculateIncentive(Guid bookingId)
        {
            try
            {
                var incentiveEarning = await _incentiveService.CalculateIncentiveAsync(bookingId);
                
                var incentiveDto = new IncentiveEarningDto
                {
                    Id = incentiveEarning.Id,
                    BookingId = incentiveEarning.BookingId,
                    SalespersonId = incentiveEarning.SalespersonId,
                    IncentiveRuleId = incentiveEarning.IncentiveRuleId,
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
                    BookingId = incentiveEarning.BookingId,
                    SalespersonId = incentiveEarning.SalespersonId,
                    IncentiveRuleId = incentiveEarning.IncentiveRuleId,
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
                    BookingId = incentiveEarning.BookingId,
                    SalespersonId = incentiveEarning.SalespersonId,
                    IncentiveRuleId = incentiveEarning.IncentiveRuleId,
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
                    BookingId = incentiveEarning.BookingId,
                    SalespersonId = incentiveEarning.SalespersonId,
                    IncentiveRuleId = incentiveEarning.IncentiveRuleId,
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

        [HttpGet("by-salesperson/{salespersonId}")]
        public async Task<IActionResult> GetBySalesperson(Guid salespersonId)
        {
            var incentiveEarnings = await _incentiveService.GetIncentiveEarningsBySalespersonAsync(salespersonId);
            
            var incentiveDtos = new List<IncentiveEarningDto>();
            foreach (var incentiveEarning in incentiveEarnings)
            {
                incentiveDtos.Add(new IncentiveEarningDto
                {
                    Id = incentiveEarning.Id,
                    BookingId = incentiveEarning.BookingId,
                    SalespersonId = incentiveEarning.SalespersonId,
                    IncentiveRuleId = incentiveEarning.IncentiveRuleId,
                    Amount = incentiveEarning.Amount,
                    EarningDate = incentiveEarning.EarningDate,
                    Status = incentiveEarning.Status.ToString(),
                    PaidDate = incentiveEarning.PaidDate
                });
            }
            
            return Ok(incentiveDtos);
        }

        [HttpGet("by-project/{projectId}")]
        public async Task<IActionResult> GetByProject(Guid projectId)
        {
            var incentiveEarnings = await _incentiveService.GetIncentiveEarningsByProjectAsync(projectId);
            
            var incentiveDtos = new List<IncentiveEarningDto>();
            foreach (var incentiveEarning in incentiveEarnings)
            {
                incentiveDtos.Add(new IncentiveEarningDto
                {
                    Id = incentiveEarning.Id,
                    BookingId = incentiveEarning.BookingId,
                    SalespersonId = incentiveEarning.SalespersonId,
                    IncentiveRuleId = incentiveEarning.IncentiveRuleId,
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
                    BookingId = incentiveEarning.BookingId,
                    SalespersonId = incentiveEarning.SalespersonId,
                    IncentiveRuleId = incentiveEarning.IncentiveRuleId,
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
