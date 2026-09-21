using LinkUp.Core.Domain.Entities.Battleship;

namespace LinkUp.Core.Domain.Interface
{
    public interface IBattleshipAttackRepository : IGenericRepository<BattleshipAttack>
    {
        Task<List<BattleshipAttack>> GetAttacksByGameAndAttackerAsync(int gameId, string attackerUserId);
        Task<List<BattleshipAttack>> GetAttacksByGameAsync(int gameId);
        Task<bool> CellAlreadyAttackedAsync(int gameId, string attackerUserId, int row, int col);
    }
}
