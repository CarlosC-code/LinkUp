using AutoMapper;
using LinkUp.Core.Application.Dtos.Post;
using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Application.Mappings.EntitiesAndDtos
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Comments,
                    opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Reactions,
                    opt => opt.MapFrom(src => src.Reactions))
                .ReverseMap()
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Reactions, opt => opt.Ignore());
        }
    }
}
