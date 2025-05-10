using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Incentive.Application.DTOs;
using Incentive.Core.Entities;
using Incentive.Core.Enums;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Incentive.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DealActivitiesController : ControllerBase
    {
        private readonly IDealActivityRepository _dealActivityRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DealActivitiesController> _logger;

        public DealActivitiesController(
            IDealActivityRepository dealActivityRepository,
            IDealRepository dealRepository,
            IMapper mapper,
            ILogger<DealActivitiesController> logger)
        {
            _dealActivityRepository = dealActivityRepository;
            _dealRepository = dealRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DealActivityDto>>> GetAllDealActivities()
        {
            var activities = await _dealActivityRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<DealActivityDto>>(activities));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DealActivityDto>> GetDealActivityById(Guid id)
        {
            var activity = await _dealActivityRepository.GetByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DealActivityDto>(activity));
        }

        [HttpGet("deal/{dealId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DealActivityDto>>> GetActivitiesByDealId(Guid dealId)
        {
            var activities = await _dealActivityRepository.GetActivitiesByDealIdAsync(dealId);
            return Ok(_mapper.Map<IEnumerable<DealActivityDto>>(activities));
        }

        [HttpGet("type/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DealActivityDto>>> GetActivitiesByType(ActivityType type)
        {
            var activities = await _dealActivityRepository.GetActivitiesByTypeAsync(type);
            return Ok(_mapper.Map<IEnumerable<DealActivityDto>>(activities));
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DealActivityDto>>> GetActivitiesByUserId(string userId)
        {
            var activities = await _dealActivityRepository.GetActivitiesByUserIdAsync(userId);
            return Ok(_mapper.Map<IEnumerable<DealActivityDto>>(activities));
        }

        [HttpGet("date-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DealActivityDto>>> GetActivitiesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be later than end date");
            }

            var activities = await _dealActivityRepository.GetActivitiesByDateRangeAsync(startDate, endDate);
            return Ok(_mapper.Map<IEnumerable<DealActivityDto>>(activities));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DealActivityDto>> CreateDealActivity(CreateDealActivityDto createDealActivityDto)
        {
            // Check if the deal exists
            var deal = await _dealRepository.GetByIdAsync(createDealActivityDto.DealId);
            if (deal == null)
            {
                return NotFound($"Deal with ID {createDealActivityDto.DealId} not found");
            }

            var activity = _mapper.Map<DealActivity>(createDealActivityDto);
            
            // Set the current user as the creator
            activity.CreatedBy = User.Identity?.Name;
            activity.UserId = User.Identity?.Name;
            activity.ActivityDate = DateTime.UtcNow;
            
            var createdActivity = await _dealActivityRepository.AddAsync(activity);
            
            return CreatedAtAction(nameof(GetDealActivityById), new { id = createdActivity.Id }, _mapper.Map<DealActivityDto>(createdActivity));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDealActivity(Guid id, UpdateDealActivityDto updateDealActivityDto)
        {
            var activity = await _dealActivityRepository.GetByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDealActivityDto, activity);
            
            // Set the current user as the modifier
            activity.LastModifiedBy = User.Identity?.Name;
            activity.LastModifiedAt = DateTime.UtcNow;
            
            await _dealActivityRepository.UpdateAsync(activity);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDealActivity(Guid id)
        {
            var activity = await _dealActivityRepository.GetByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            await _dealActivityRepository.SoftDeleteAsync(activity);
            
            return NoContent();
        }
    }
}
