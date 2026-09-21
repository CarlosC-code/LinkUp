using AutoMapper;
using LinkUp.Core.Application.Dtos.Reaction;
using LinkUp.Core.Application.ViewModels.Reaction;

namespace LinkUp.Core.Application.Mappings.DtosAndViewModels
{
    public class ReactionDtoMappingProfile : Profile
    {
        public ReactionDtoMappingProfile()
        {
            CreateMap<ReactionDto, ReactionViewModel>()
                .ReverseMap();
        }
    }
}
