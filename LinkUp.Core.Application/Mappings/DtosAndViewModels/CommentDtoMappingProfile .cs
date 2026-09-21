using AutoMapper;
using LinkUp.Core.Application.Dtos.Comment;
using LinkUp.Core.Application.ViewModels.Comment;

namespace LinkUp.Core.Application.Mappings.DtosAndViewModels
{
    public class CommentDtoMappingProfile : Profile
    {
        public CommentDtoMappingProfile()
        {
            CreateMap<CommentDto, CommentViewModel>()
                .ReverseMap();

            CreateMap<CommentDto, SaveCommentViewModel>()
                .ReverseMap();

            CreateMap<CommentDto, DeleteCommentViewModel>()
                .ReverseMap();
        }
    }
}
