using AutoMapper;
using LinkUp.Core.Application.Dtos.Post;
using LinkUp.Core.Application.ViewModels.Post;

namespace LinkUp.Core.Application.Mappings.DtosAndViewModels
{
    public class PostDtoMappingProfile : Profile
    {
        public PostDtoMappingProfile()
        {
            CreateMap<PostDto, PostViewModel>()
                .ReverseMap();

            CreateMap<PostDto, SavePostViewModel>()
                .ReverseMap();

            CreateMap<PostDto, DeletePostViewModel>()
                .ReverseMap();
        }
    }
}
