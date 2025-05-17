using AutoMapper;
using Incentive.Application.DTOs;
using Incentive.Core.Entities;

namespace Incentive.Application.Mappings
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            // Project mappings
            CreateMap<Project, ProjectDto>();
            CreateMap<CreateProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>();
            CreateMap<Project, ProjectMinimalDto>();
        }
    }
}
