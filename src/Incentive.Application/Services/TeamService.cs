using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Incentive.Application.DTOs;
using Incentive.Core.Entities;
using Incentive.Application.Interfaces;
using Incentive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Incentive.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;
        private readonly IIdentityService _identityService;
        private readonly ILogger<TeamService> _logger;

        public TeamService(
            AppDbContext dbContext,
            IMapper mapper,
            ITenantService tenantService,
            IIdentityService identityService,
            ILogger<TeamService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tenantService = tenantService;
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<List<TeamDto>> GetAllTeamsAsync()
        {
            var teams = await _dbContext.Teams
                .Include(t => t.Members)
                .ToListAsync();

            return _mapper.Map<List<TeamDto>>(teams);
        }

        public async Task<TeamDto> GetTeamByIdAsync(Guid id)
        {
            var team = await _dbContext.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
            {
                return null;
            }

            var teamDto = _mapper.Map<TeamDto>(team);

            // Enrich with user names
            foreach (var member in teamDto.Members)
            {
                var user = await _identityService.GetUserByIdAsync(member.UserId);
                if (user != null)
                {
                    member.UserName = user.UserName;
                }
            }

            return teamDto;
        }

        public async Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto)
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            var team = _mapper.Map<Team>(createTeamDto);
            team.TenantId = tenantId;

            _dbContext.Teams.Add(team);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TeamDto>(team);
        }

        public async Task<TeamDto> UpdateTeamAsync(Guid id, UpdateTeamDto updateTeamDto)
        {
            var team = await _dbContext.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
            {
                return null;
            }

            _mapper.Map(updateTeamDto, team);
            await _dbContext.SaveChangesAsync();

            return await GetTeamByIdAsync(id);
        }

        public async Task<bool> DeleteTeamAsync(Guid id)
        {
            var team = await _dbContext.Teams.FindAsync(id);
            if (team == null)
            {
                return false;
            }

            _dbContext.Teams.Remove(team);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<TeamMemberDto>> GetTeamMembersAsync(Guid teamId)
        {
            var team = await _dbContext.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == teamId);

            if (team == null)
            {
                return new List<TeamMemberDto>();
            }

            var memberDtos = _mapper.Map<List<TeamMemberDto>>(team.Members);

            // Enrich with user names
            foreach (var member in memberDtos)
            {
                var user = await _identityService.GetUserByIdAsync(member.UserId);
                if (user != null)
                {
                    member.UserName = user.UserName;
                }
                member.TeamName = team.Name;
            }

            return memberDtos;
        }

        public async Task<TeamMemberDto> AddTeamMemberAsync(AddTeamMemberDto addTeamMemberDto)
        {
            var team = await _dbContext.Teams.FindAsync(addTeamMemberDto.TeamId);
            if (team == null)
            {
                throw new ArgumentException($"Team with ID {addTeamMemberDto.TeamId} not found");
            }

            var user = await _identityService.GetUserByIdAsync(addTeamMemberDto.UserId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {addTeamMemberDto.UserId} not found");
            }

            var tenantId = _tenantService.GetCurrentTenantId();

            var teamMember = _mapper.Map<TeamMember>(addTeamMemberDto);
            teamMember.TenantId = tenantId;

            _dbContext.TeamMembers.Add(teamMember);
            await _dbContext.SaveChangesAsync();

            var memberDto = _mapper.Map<TeamMemberDto>(teamMember);
            memberDto.UserName = user.UserName;
            memberDto.TeamName = team.Name;

            return memberDto;
        }

        public async Task<TeamMemberDto> UpdateTeamMemberAsync(Guid id, UpdateTeamMemberDto updateTeamMemberDto)
        {
            var teamMember = await _dbContext.TeamMembers
                .Include(tm => tm.Team)
                .FirstOrDefaultAsync(tm => tm.Id == id);

            if (teamMember == null)
            {
                return null;
            }

            _mapper.Map(updateTeamMemberDto, teamMember);
            await _dbContext.SaveChangesAsync();

            var memberDto = _mapper.Map<TeamMemberDto>(teamMember);
            var user = await _identityService.GetUserByIdAsync(teamMember.UserId);
            if (user != null)
            {
                memberDto.UserName = user.UserName;
            }
            memberDto.TeamName = teamMember.Team.Name;

            return memberDto;
        }

        public async Task<bool> RemoveTeamMemberAsync(Guid id)
        {
            var teamMember = await _dbContext.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return false;
            }

            _dbContext.TeamMembers.Remove(teamMember);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<TeamMinimalDto>> GetTeamsMinimalAsync()
        {
            var teams = await _dbContext.Teams
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .Select(t => new TeamMinimalDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();

            return teams;
        }
    }
}
