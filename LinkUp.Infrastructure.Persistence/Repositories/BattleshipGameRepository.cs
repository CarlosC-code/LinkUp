using LinkUp.Core.Domain.Common.Enums;
using LinkUp.Core.Domain.Entities.Battleship;
using LinkUp.Core.Domain.Interface;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class BattleshipGameRepository : GenericRepository<BattleshipGame>, IBattleshipGameRepository
    {
        private readonly LinkUpAppContext _context;

        public BattleshipGameRepository(LinkUpAppContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BattleshipGame>> GetActiveGamesByUserIdAsync(string userId)
        {
            return await _context.BattleshipGames
                .Where(g => (g.CreatorUserId == userId || g.OpponentUserId == userId)
                         && g.Status != GameStatus.Finished)
                .OrderByDescending(g => g.StartedAtUtc)
                .ToListAsync();
        }

        public async Task<List<BattleshipGame>> GetFinishedGamesByUserIdAsync(string userId)
        {
            return await _context.BattleshipGames
                .Where(g => (g.CreatorUserId == userId || g.OpponentUserId == userId)
                         && g.Status == GameStatus.Finished)
                .OrderByDescending(g => g.FinishedAtUtc)
                .ToListAsync();
        }

        public async Task<BattleshipGame?> GetActiveGameBetweenUsersAsync(string userA, string userB)
        {
            return await _context.BattleshipGames
                .FirstOrDefaultAsync(g =>
                    g.Status != GameStatus.Finished
                    && ((g.CreatorUserId == userA && g.OpponentUserId == userB)
                     || (g.CreatorUserId == userB && g.OpponentUserId == userA)));
        }

        public async Task<List<BattleshipGame>> GetAllActiveGamesAsync()
        {
            return await _context.BattleshipGames
                .Where(g => g.Status == GameStatus.InProgress)
                .ToListAsync();
        }
    }
}