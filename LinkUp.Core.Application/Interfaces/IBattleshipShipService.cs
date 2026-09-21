using LinkUp.Core.Application.Dtos.AccionBattleship;
using LinkUp.Core.Application.Dtos.BattleshipShip;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IBattleshipShipService
    {
        Task<List<BattleshipShipDto>> GetShipsByGame(int gameId, string userId);
        Task<bool> PlaceShip(PlaceShipDto dto);
    }
}