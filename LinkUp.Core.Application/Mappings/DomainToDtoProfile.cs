using AutoMapper;
using LinkUp.Core.Application.Dtos.Battleship;
using LinkUp.Core.Application.Dtos.Social;
using LinkUp.Core.Domain.Entities.Battleship;
using LinkUp.Core.Domain.Entities.Social;


namespace LinkUp.Core.Application.Mappings
{
    public class DomainToDtoProfile : Profile
    {

        public DomainToDtoProfile()
        {
            // Social
            CreateMap<Post, PostDto>();
            CreateMap<Comment, CommentDto>();
            CreateMap<Reaction, ReactionDto>();
            CreateMap<FriendRequest, FriendRequestDto>();
            CreateMap<Friendship, FriendshipDto>();

            // Battleship
            CreateMap<BattleshipGame, BattleshipGameDto>();
            CreateMap<BattleshipShip, BattleshipShipDto>();
            CreateMap<BattleshipAttack, BattleshipAttackDto>();
        }

    }
}
