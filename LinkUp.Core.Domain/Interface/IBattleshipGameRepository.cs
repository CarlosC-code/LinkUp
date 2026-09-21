using LinkUp.Core.Domain.Entities.Battleship;

namespace LinkUp.Core.Domain.Interface
{
    public interface IBattleshipGameRepository : IGenericRepository<BattleshipGame>
    {
        Task<List<BattleshipGame>> GetActiveGamesByUserIdAsync(string userId);
        Task<List<BattleshipGame>> GetFinishedGamesByUserIdAsync(string userId);
        Task<BattleshipGame?> GetActiveGameBetweenUsersAsync(string userA, string userB);
        Task<List<BattleshipGame>> GetAllActiveGamesAsync();
    }
}
