using AutoMapper;
using LinkUp.Core.Application.Dtos.Comment;
using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Application.Mappings.EntitiesAndDtos
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.Replies,
                           opt => opt.MapFrom(src => src.Replies))
                .ReverseMap()
                .ForMember(dest => dest.Post, opt => opt.Ignore())
                .ForMember(dest => dest.ParentComment, opt => opt.Ignore())
                .ForMember(dest => dest.Replies, opt => opt.Ignore());
        }
    }
}
