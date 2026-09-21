
using LinkUp.Core.Application.Dtos.AccionBattleship;
using LinkUp.Core.Application.Dtos.BattleshipGame;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IBattleshipGameService : IGenericService<BattleshipGameDto>
    {
        

        Task<BattleshipGameDto?> CreateGame(CreateBattleshipGameDto dto);

        Task<bool> SurrenderGame(int gameId, string userId);
    }
}
