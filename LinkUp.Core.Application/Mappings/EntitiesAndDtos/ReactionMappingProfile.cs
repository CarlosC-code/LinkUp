using AutoMapper;
using LinkUp.Core.Application.Dtos.Reaction;
using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Application.Mappings.EntitiesAndDtos
{
    public class ReactionMappingProfile : Profile
    {
        public ReactionMappingProfile()
        {
            CreateMap<Reaction, ReactionDto>()
                .ReverseMap()
                .ForMember(dest => dest.Post, opt => opt.Ignore());
        }
    }
}
