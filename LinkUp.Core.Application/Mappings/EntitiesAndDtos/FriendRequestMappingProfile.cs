using AutoMapper;
using LinkUp.Core.Application.Dtos.FriendRequest;
using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Application.Mappings.EntitiesAndDtos
{
    public class FriendRequestMappingProfile : Profile
    {
        public FriendRequestMappingProfile()
        {
            CreateMap<FriendRequest, FriendRequestDto>()
                .ReverseMap();
        }
    }
}
