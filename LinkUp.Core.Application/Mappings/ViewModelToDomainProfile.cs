using AutoMapper;
using LinkUp.Core.Application.ViewModels.Social;
using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Application.Mappings
{
    public class ViewModelToDomainProfile : Profile
    {

        public ViewModelToDomainProfile()
        {
            CreateMap<PostCreateViewModel, Post>();
            CreateMap<PostEditViewModel, Post>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.MediaType, m => m.MapFrom(s => s.MediaType));
        }

    }
}
