using AutoMapper;
using LinkUp.Core.Application.Dtos.BattleshipGame;
using LinkUp.Core.Domain.Entities.Battleship;


namespace LinkUp.Core.Application.Mappings.EntitiesAndDtos
{
    public class BattleshipGameMappingProfile : Profile
    {
        public BattleshipGameMappingProfile()
        {
            CreateMap<BattleshipGame, BattleshipGameDto>()
                .ReverseMap();
        }
    }
}
