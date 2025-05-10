using AutoMapper;
using Incentive.Application.DTOs;
using Incentive.Core.Entities;

namespace Incentive.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // IncentiveRule mappings
            CreateMap<IncentiveRule, IncentiveRuleDto>();
            CreateMap<CreateIncentiveRuleDto, IncentiveRule>();
            CreateMap<UpdateIncentiveRuleDto, IncentiveRule>();

            // Deal mappings
            CreateMap<Deal, DealDto>()
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments))
                .ForMember(dest => dest.Activities, opt => opt.MapFrom(src => src.Activities));
            CreateMap<CreateDealDto, Deal>();
            CreateMap<UpdateDealDto, Deal>();

            // Payment mappings
            CreateMap<Payment, PaymentDto>();
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<UpdatePaymentDto, Payment>();

            // DealActivity mappings
            CreateMap<DealActivity, DealActivityDto>();
            CreateMap<CreateDealActivityDto, DealActivity>();
            CreateMap<UpdateDealActivityDto, DealActivity>();
        }
    }
}
