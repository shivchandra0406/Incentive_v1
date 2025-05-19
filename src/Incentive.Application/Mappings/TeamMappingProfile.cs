using AutoMapper;
using Incentive.Application.DTOs;
using Incentive.Core.Entities;

namespace Incentive.Application.Mappings
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            // Team mappings
            CreateMap<Team, TeamDto>();
            CreateMap<Team, TeamMinimalDto>();
            CreateMap<CreateTeamDto, Team>();
            CreateMap<UpdateTeamDto, Team>();

            // Team member mappings
            CreateMap<TeamMember, TeamMemberDto>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : string.Empty));
            CreateMap<AddTeamMemberDto, TeamMember>();
            CreateMap<UpdateTeamMemberDto, TeamMember>();
        }
    }
}
