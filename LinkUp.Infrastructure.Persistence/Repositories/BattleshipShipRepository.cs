using LinkUp.Core.Domain.Entities.Battleship;
using LinkUp.Core.Domain.Interface;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class BattleshipShipRepository : GenericRepository<BattleshipShip>, IBattleshipShipRepository
    {
        private readonly LinkUpAppContext _context;

        public BattleshipShipRepository(LinkUpAppContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BattleshipShip>> GetShipsByGameAndOwnerAsync(int gameId, string ownerUserId)
        {
            return await _context.BattleshipShips
                .Where(s => s.GameId == gameId && s.OwnerUserId == ownerUserId)
                .ToListAsync();
        }

        public async Task<List<BattleshipShip>> GetShipsByGameAsync(int gameId)
        {
            return await _context.BattleshipShips
                .Where(s => s.GameId == gameId)
                .ToListAsync();
        }
    }
}