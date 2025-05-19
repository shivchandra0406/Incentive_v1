using AutoMapper;
using Incentive.Application.DTOs;
using Incentive.Core.Entities.IncentivePlan;

namespace Incentive.Application.Mappings
{
    public class IncentivePlanMappingProfile : Profile
    {
        public IncentivePlanMappingProfile()
        {
            // Base plan mappings
            CreateMap<IncentivePlanBase, IncentivePlanBaseDto>();
            CreateMap<IncentivePlanBase, IncentivePlanMinimalDto>();

            // Target-based plan mappings
            CreateMap<TargetBasedIncentivePlan, TargetBasedIncentivePlanDto>();
            CreateMap<CreateTargetBasedIncentivePlanDto, TargetBasedIncentivePlan>();
            CreateMap<UpdateTargetBasedIncentivePlanDto, TargetBasedIncentivePlan>();

            // Role-based plan mappings
            CreateMap<RoleBasedIncentivePlan, RoleBasedIncentivePlanDto>();
            CreateMap<CreateRoleBasedIncentivePlanDto, RoleBasedIncentivePlan>();
            CreateMap<UpdateRoleBasedIncentivePlanDto, RoleBasedIncentivePlan>();

            // Project-based plan mappings
            CreateMap<ProjectBasedIncentivePlan, ProjectBasedIncentivePlanDto>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : string.Empty));
            CreateMap<CreateProjectBasedIncentivePlanDto, ProjectBasedIncentivePlan>();
            CreateMap<UpdateProjectBasedIncentivePlanDto, ProjectBasedIncentivePlan>();

            // Kicker-based plan mappings
            CreateMap<KickerIncentivePlan, KickerIncentivePlanDto>();
            CreateMap<CreateKickerIncentivePlanDto, KickerIncentivePlan>();
            CreateMap<UpdateKickerIncentivePlanDto, KickerIncentivePlan>();

            // Tiered-based plan mappings
            CreateMap<TieredIncentivePlan, TieredIncentivePlanDto>();
            CreateMap<CreateTieredIncentivePlanDto, TieredIncentivePlan>();
            CreateMap<UpdateTieredIncentivePlanDto, TieredIncentivePlan>()
                .ForMember(dest => dest.Tiers, opt => opt.Ignore());

            // Tiered incentive tier mappings
            CreateMap<TieredIncentiveTier, TieredIncentiveTierDto>();
            CreateMap<CreateTieredIncentiveTierDto, TieredIncentiveTier>();
            CreateMap<UpdateTieredIncentiveTierDto, TieredIncentiveTier>();
        }
    }
}
