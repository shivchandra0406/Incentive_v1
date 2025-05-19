using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.API.Attributes;
using Incentive.Application.Common.Models;
using Incentive.Application.DTOs;
using Incentive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [RequiresTenantId(description: "The tenant ID to access team data")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(ITeamService teamService, ILogger<TeamsController> logger)
        {
            _teamService = teamService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<TeamDto>>>> GetAllTeams()
        {
            try
            {
                var teams = await _teamService.GetAllTeamsAsync();
                return Ok(BaseResponse<List<TeamDto>>.Success(teams, "Teams retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving teams");
                return StatusCode(500, BaseResponse<List<TeamDto>>.Failure("An error occurred while retrieving teams"));
            }
        }

        /// <summary>
        /// Get minimal team data (ID and Name only)
        /// </summary>
        /// <returns>List of teams with minimal data</returns>
        [HttpGet("minimal")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<TeamMinimalDto>>), 200)]
        public async Task<ActionResult<BaseResponse<IEnumerable<TeamMinimalDto>>>> GetTeamsMinimal()
        {
            try
            {
                var teams = await _teamService.GetTeamsMinimalAsync();
                return Ok(BaseResponse<IEnumerable<TeamMinimalDto>>.Success(teams, "Minimal team data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving minimal team data");
                return StatusCode(500, BaseResponse<IEnumerable<TeamMinimalDto>>.Failure("An error occurred while retrieving minimal team data"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<TeamDto>>> GetTeamById(Guid id)
        {
            try
            {
                var team = await _teamService.GetTeamByIdAsync(id);
                if (team == null)
                {
                    return NotFound(BaseResponse<TeamDto>.Failure($"Team with ID {id} not found"));
                }

                return Ok(BaseResponse<TeamDto>.Success(team, "Team retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving team with ID {TeamId}", id);
                return StatusCode(500, BaseResponse<TeamDto>.Failure($"An error occurred while retrieving team with ID {id}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<TeamDto>>> CreateTeam([FromBody] CreateTeamDto createTeamDto)
        {
            try
            {
                var team = await _teamService.CreateTeamAsync(createTeamDto);
                return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, BaseResponse<TeamDto>.Success(team, "Team created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating team");
                return StatusCode(500, BaseResponse<TeamDto>.Failure("An error occurred while creating the team"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<TeamDto>>> UpdateTeam(Guid id, [FromBody] UpdateTeamDto updateTeamDto)
        {
            try
            {
                var team = await _teamService.UpdateTeamAsync(id, updateTeamDto);
                if (team == null)
                {
                    return NotFound(BaseResponse<TeamDto>.Failure($"Team with ID {id} not found"));
                }

                return Ok(BaseResponse<TeamDto>.Success(team, "Team updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating team with ID {TeamId}", id);
                return StatusCode(500, BaseResponse<TeamDto>.Failure($"An error occurred while updating team with ID {id}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> DeleteTeam(Guid id)
        {
            try
            {
                var result = await _teamService.DeleteTeamAsync(id);
                if (!result)
                {
                    return NotFound(BaseResponse<bool>.Failure($"Team with ID {id} not found"));
                }

                return Ok(BaseResponse<bool>.Success(true, "Team deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting team with ID {TeamId}", id);
                return StatusCode(500, BaseResponse<bool>.Failure($"An error occurred while deleting team with ID {id}"));
            }
        }

        [HttpGet("{teamId}/members")]
        public async Task<ActionResult<BaseResponse<List<TeamMemberDto>>>> GetTeamMembers(Guid teamId)
        {
            try
            {
                var members = await _teamService.GetTeamMembersAsync(teamId);
                return Ok(BaseResponse<List<TeamMemberDto>>.Success(members, "Team members retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving members for team with ID {TeamId}", teamId);
                return StatusCode(500, BaseResponse<List<TeamMemberDto>>.Failure($"An error occurred while retrieving members for team with ID {teamId}"));
            }
        }

        [HttpPost("members")]
        public async Task<ActionResult<BaseResponse<TeamMemberDto>>> AddTeamMember([FromBody] AddTeamMemberDto addTeamMemberDto)
        {
            try
            {
                var member = await _teamService.AddTeamMemberAsync(addTeamMemberDto);
                return CreatedAtAction(nameof(GetTeamMembers), new { teamId = member.TeamId }, BaseResponse<TeamMemberDto>.Success(member, "Team member added successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding member to team with ID {TeamId}", addTeamMemberDto.TeamId);
                return StatusCode(500, BaseResponse<TeamMemberDto>.Failure($"An error occurred while adding member to team with ID {addTeamMemberDto.TeamId}"));
            }
        }

        [HttpPut("members/{id}")]
        public async Task<ActionResult<BaseResponse<TeamMemberDto>>> UpdateTeamMember(Guid id, [FromBody] UpdateTeamMemberDto updateTeamMemberDto)
        {
            try
            {
                var member = await _teamService.UpdateTeamMemberAsync(id, updateTeamMemberDto);
                if (member == null)
                {
                    return NotFound(BaseResponse<TeamMemberDto>.Failure($"Team member with ID {id} not found"));
                }

                return Ok(BaseResponse<TeamMemberDto>.Success(member, "Team member updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating team member with ID {MemberId}", id);
                return StatusCode(500, BaseResponse<TeamMemberDto>.Failure($"An error occurred while updating team member with ID {id}"));
            }
        }

        [HttpDelete("members/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> RemoveTeamMember(Guid id)
        {
            try
            {
                var result = await _teamService.RemoveTeamMemberAsync(id);
                if (!result)
                {
                    return NotFound(BaseResponse<bool>.Failure($"Team member with ID {id} not found"));
                }

                return Ok(BaseResponse<bool>.Success(true, "Team member removed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing team member with ID {MemberId}", id);
                return StatusCode(500, BaseResponse<bool>.Failure($"An error occurred while removing team member with ID {id}"));
            }
        }
    }
}
