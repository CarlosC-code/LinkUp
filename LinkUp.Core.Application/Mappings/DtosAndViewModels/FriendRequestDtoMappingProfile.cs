using AutoMapper;
using LinkUp.Core.Application.Dtos.FriendRequest;
using LinkUp.Core.Application.ViewModels.FriendRequest;

namespace LinkUp.Core.Application.Mappings.DtosAndViewModels
{
    public class FriendRequestDtoMappingProfile : Profile
    {
        public FriendRequestDtoMappingProfile()
        {
            CreateMap<FriendRequestDto, FriendRequestViewModel>()
                .ReverseMap();
        }
    }
}
