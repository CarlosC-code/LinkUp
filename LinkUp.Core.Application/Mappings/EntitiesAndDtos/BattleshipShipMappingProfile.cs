using AutoMapper;
using LinkUp.Core.Application.Dtos.BattleshipShip;
using LinkUp.Core.Domain.Entities.Battleship;

namespace LinkUp.Core.Application.Mappings.EntitiesAndDtos
{
    public class BattleshipShipMappingProfile : Profile
    {
        public BattleshipShipMappingProfile()
        {
            CreateMap<BattleshipShip, BattleshipShipDto>()
                .ReverseMap();
        }
    }
}
