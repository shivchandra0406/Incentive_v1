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
    public class IncentiveRulesController : ControllerBase
    {
        private readonly IIncentiveRuleRepository _incentiveRuleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IncentiveRulesController> _logger;

        public IncentiveRulesController(
            IIncentiveRuleRepository incentiveRuleRepository,
            IMapper mapper,
            ILogger<IncentiveRulesController> logger)
        {
            _incentiveRuleRepository = incentiveRuleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IncentiveRuleDto>>> GetAllIncentiveRules()
        {
            var incentiveRules = await _incentiveRuleRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<IncentiveRuleDto>>(incentiveRules));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IncentiveRuleDto>> GetIncentiveRuleById(Guid id)
        {
            var incentiveRule = await _incentiveRuleRepository.GetByIdAsync(id);
            if (incentiveRule == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IncentiveRuleDto>(incentiveRule));
        }

        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IncentiveRuleDto>>> GetActiveIncentiveRules()
        {
            var incentiveRules = await _incentiveRuleRepository.GetActiveRulesAsync();
            return Ok(_mapper.Map<IEnumerable<IncentiveRuleDto>>(incentiveRules));
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IncentiveRuleDto>>> GetIncentiveRulesByUserId(Guid userId)
        {
            var incentiveRules = await _incentiveRuleRepository.GetRulesByUserIdAsync(userId);
            return Ok(_mapper.Map<IEnumerable<IncentiveRuleDto>>(incentiveRules));
        }

        [HttpGet("team/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IncentiveRuleDto>>> GetIncentiveRulesByTeamId(Guid teamId)
        {
            var incentiveRules = await _incentiveRuleRepository.GetRulesByTeamIdAsync(teamId);
            return Ok(_mapper.Map<IEnumerable<IncentiveRuleDto>>(incentiveRules));
        }

        [HttpGet("frequency/{frequency}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IncentiveRuleDto>>> GetIncentiveRulesByFrequency(TargetFrequency frequency)
        {
            var incentiveRules = await _incentiveRuleRepository.GetRulesByFrequencyAsync(frequency);
            return Ok(_mapper.Map<IEnumerable<IncentiveRuleDto>>(incentiveRules));
        }

        [HttpGet("applied-type/{appliedType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IncentiveRuleDto>>> GetIncentiveRulesByAppliedType(AppliedRuleType appliedType)
        {
            var incentiveRules = await _incentiveRuleRepository.GetRulesByAppliedTypeAsync(appliedType);
            return Ok(_mapper.Map<IEnumerable<IncentiveRuleDto>>(incentiveRules));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IncentiveRuleDto>> CreateIncentiveRule(CreateIncentiveRuleDto createIncentiveRuleDto)
        {
            var incentiveRule = _mapper.Map<IncentiveRule>(createIncentiveRuleDto);
            
            var createdIncentiveRule = await _incentiveRuleRepository.AddAsync(incentiveRule);
            
            return CreatedAtAction(nameof(GetIncentiveRuleById), new { id = createdIncentiveRule.Id }, _mapper.Map<IncentiveRuleDto>(createdIncentiveRule));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateIncentiveRule(Guid id, UpdateIncentiveRuleDto updateIncentiveRuleDto)
        {
            var incentiveRule = await _incentiveRuleRepository.GetByIdAsync(id);
            if (incentiveRule == null)
            {
                return NotFound();
            }

            _mapper.Map(updateIncentiveRuleDto, incentiveRule);
            
            await _incentiveRuleRepository.UpdateAsync(incentiveRule);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIncentiveRule(Guid id)
        {
            var incentiveRule = await _incentiveRuleRepository.GetByIdAsync(id);
            if (incentiveRule == null)
            {
                return NotFound();
            }

            await _incentiveRuleRepository.SoftDeleteAsync(incentiveRule);
            
            return NoContent();
        }
    }
}
