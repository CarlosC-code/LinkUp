using AutoMapper;
using LinkUp.Core.Application.Dtos.BattleshipAttack;
using LinkUp.Core.Application.ViewModels.Battleship;

namespace LinkUp.Core.Application.Mappings.DtosAndViewModels
{
    public class BattleshipAttackDtoMappingProfile : Profile
    {
        public BattleshipAttackDtoMappingProfile()
        {
            CreateMap<BattleshipAttackDto, BattleshipAttackViewModel>()
                .ReverseMap();
        }
    }
}
