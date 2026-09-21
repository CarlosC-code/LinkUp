using LinkUp.Core.Domain.Entities.Battleship;

namespace LinkUp.Core.Domain.Interface
{
    public interface IBattleshipShipRepository : IGenericRepository<BattleshipShip>
    {
        Task<List<BattleshipShip>> GetShipsByGameAndOwnerAsync(int gameId, string ownerUserId);
        Task<List<BattleshipShip>> GetShipsByGameAsync(int gameId);
    }
}
