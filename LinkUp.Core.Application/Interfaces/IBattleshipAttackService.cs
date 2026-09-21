

using LinkUp.Core.Application.Dtos.BattleshipAttack;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IBattleshipAttackService : IGenericService<BattleshipAttackDto>
    {
        Task<List<BattleshipAttackDto>> GetByGame(int gameId);
    }
}
