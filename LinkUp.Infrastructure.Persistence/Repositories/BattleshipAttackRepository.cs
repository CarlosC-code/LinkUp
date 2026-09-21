using LinkUp.Core.Domain.Entities.Battleship;
using LinkUp.Core.Domain.Interface;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class BattleshipAttackRepository : GenericRepository<BattleshipAttack>, IBattleshipAttackRepository
    {
        private readonly LinkUpAppContext _context;

        public BattleshipAttackRepository(LinkUpAppContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BattleshipAttack>> GetAttacksByGameAndAttackerAsync(int gameId, string attackerUserId)
        {
            return await _context.BattleshipAttacks
                .Where(a => a.GameId == gameId && a.AttackerUserId == attackerUserId)
                .OrderBy(a => a.PerformedAtUtc)
                .ToListAsync();
        }

        public async Task<List<BattleshipAttack>> GetAttacksByGameAsync(int gameId)
        {
            return await _context.BattleshipAttacks
                .Where(a => a.GameId == gameId)
                .OrderBy(a => a.PerformedAtUtc)
                .ToListAsync();
        }

        public async Task<bool> CellAlreadyAttackedAsync(int gameId, string attackerUserId, int row, int col)
        {
            return await _context.BattleshipAttacks
                .AnyAsync(a => a.GameId == gameId
                            && a.AttackerUserId == attackerUserId
                            && a.TargetRow == row
                            && a.TargetCol == col);
        }
    }
}
