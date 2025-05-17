using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Application.DTOs;

namespace Incentive.Application.Interfaces
{
    public interface ITeamService
    {
        Task<List<TeamDto>> GetAllTeamsAsync();
        Task<TeamDto> GetTeamByIdAsync(Guid id);
        Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto);
        Task<TeamDto> UpdateTeamAsync(Guid id, UpdateTeamDto updateTeamDto);
        Task<bool> DeleteTeamAsync(Guid id);
        
        Task<List<TeamMemberDto>> GetTeamMembersAsync(Guid teamId);
        Task<TeamMemberDto> AddTeamMemberAsync(AddTeamMemberDto addTeamMemberDto);
        Task<TeamMemberDto> UpdateTeamMemberAsync(Guid id, UpdateTeamMemberDto updateTeamMemberDto);
        Task<bool> RemoveTeamMemberAsync(Guid id);
    }
}
