using AutoMapper;
using LinkUp.Core.Application.Dtos.Friendship;
using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Application.Mappings.EntitiesAndDtos
{
    public class FriendshipMappingProfile : Profile
    {
        public FriendshipMappingProfile()
        {
            CreateMap<Friendship, FriendshipDto>()
                .ReverseMap();
        }
    }
}
