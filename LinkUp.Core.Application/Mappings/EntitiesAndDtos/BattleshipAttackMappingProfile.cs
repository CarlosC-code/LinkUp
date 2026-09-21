using AutoMapper;
using LinkUp.Core.Application.Dtos.BattleshipAttack;
using LinkUp.Core.Domain.Entities.Battleship;

namespace LinkUp.Core.Application.Mappings.EntitiesAndDtos
{
    public class BattleshipAttackMappingProfile : Profile
    {
        public BattleshipAttackMappingProfile()
        {
            CreateMap<BattleshipAttack, BattleshipAttackDto>()
                .ReverseMap();
        }
    }
}
