
using AutoMapper;
using LinkUp.Core.Application.Dtos.Battleship;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Domain.Common.Enums;
using LinkUp.Core.Domain.Entities.Battleship;
using LinkUp.Core.Domain.Interface;

namespace LinkUp.Core.Application.Services
{
    public class BattleshipService : GenericService<BattleshipGame, BattleshipGameDto>, IBattleshipService
    {
        private readonly IBattleshipGameRepository _games;
        private readonly IBattleshipShipRepository _ships;
        private readonly IBattleshipAttackRepository _attacks;
        private readonly IFriendshipRepository _friendships;
        private readonly IMapper _mapper;

        private const int BOARD = 12;
        private static readonly int[] DefaultShips = new[] { 2, 3, 3, 4, 5 };
        private static readonly TimeSpan MaxTurnWait = TimeSpan.FromHours(48);

        public BattleshipService(
            IBattleshipGameRepository games,
            IBattleshipShipRepository ships,
            IBattleshipAttackRepository attacks,
            IFriendshipRepository friendships,
            IMapper mapper)
            : base(games, mapper)
        {
            _games = games;
            _ships = ships;
            _attacks = attacks;
            _friendships = friendships;
            _mapper = mapper;
        }

        public async Task<List<BattleshipGameDto>> GetActiveGamesAsync(string userId)
        {
            var list = await _games.GetActiveGamesByUserIdAsync(userId);
            return list.Select(g => ToDto(g, userId)).ToList();
        }

        public async Task<List<BattleshipGameDto>> GetFinishedGamesAsync(string userId)
        {
            var list = await _games.GetFinishedGamesByUserIdAsync(userId);
            return list.Select(g => ToDto(g, userId)).ToList();
        }

        public async Task<BattleshipGameDto?> GetByIdAsync(int gameId, string currentUserId)
        {
            var g = await _games.GetById(gameId);
            return g is null ? null : ToDto(g, currentUserId);
        }

        public async Task<int?> StartNewGameAsync(string creatorUserId, string opponentUserId)
        {
            var active = await _games.GetActiveGameBetweenUsersAsync(creatorUserId, opponentUserId);
            if (active != null) return null;

            if (!await _friendships.AreFriendsAsync(creatorUserId, opponentUserId)) return null;

            var game = new BattleshipGame
            {
                CreatorUserId = creatorUserId,
                OpponentUserId = opponentUserId,
                Status = GameStatus.Placing,
                CreatorReady = false,
                OpponentReady = false
            };
            await _games.AddAsync(game);
            return game.Id;
        }

        public async Task<List<int>> GetRemainingShipsAsync(int gameId, string userId)
        {
            var placed = await _ships.GetShipsByGameAndOwnerAsync(gameId, userId);
            var remaining = DefaultShips.ToList();
            foreach (var s in placed) remaining.Remove(s.Length);
            return remaining;
        }

        public async Task<bool> PlaceShipAsync(int gameId, string userId, int length, int startRow, int startCol, int direction)
        {
            if (!IsWithinBoard(startRow, startCol)) return false;

            var cells = GetCells(startRow, startCol, length, (Direction)direction);
            if (cells.Any(c => !IsWithinBoard(c.Row, c.Col))) return false;

            var myShips = await _ships.GetShipsByGameAndOwnerAsync(gameId, userId);
            var myCells = myShips.SelectMany(s => GetCells(s.StartRow, s.StartCol, s.Length, s.Direction)).ToHashSet();
            if (cells.Any(c => myCells.Contains(c))) return false;

            var entity = new BattleshipShip
            {
                GameId = gameId,
                OwnerUserId = userId,
                Length = length,
                StartRow = startRow,
                StartCol = startCol,
                Direction = (Direction)direction,
                PlacedAtUtc = DateTime.UtcNow
            };
            await _ships.AddAsync(entity);

            var game = await _games.GetById(gameId);
            if (game == null) return false;

            // marcar listo si ya coloco todos
            var remaining = await GetRemainingShipsAsync(gameId, userId);
            if (!remaining.Any())
            {
                if (userId == game.CreatorUserId) game.CreatorReady = true;
                if (userId == game.OpponentUserId) game.OpponentReady = true;

                if (game.CreatorReady && game.OpponentReady)
                {
                    game.Status = GameStatus.InProgress;
                    game.CurrentTurnUserId = game.CreatorUserId;
                    game.LastMoveAtUtc = DateTime.UtcNow;
                }
                await _games.UpdateAsync(game);
            }
            return true;
        }

        public async Task<bool> AreBothReadyAsync(int gameId)
        {
            var g = await _games.GetById(gameId);
            return g != null && g.CreatorReady && g.OpponentReady;
        }

        public async Task<bool> SurrenderAsync(int gameId, string userId)
        {
            var g = await _games.GetById(gameId);
            if (g == null || g.Status == GameStatus.Finished) return false;

            g.Status = GameStatus.Finished;
            g.WinnerUserId = (g.CreatorUserId == userId) ? g.OpponentUserId : g.CreatorUserId;
            g.FinishedAtUtc = DateTime.UtcNow;
            await _games.UpdateAsync(g);
            return true;
        }

        public async Task<bool> FinishIfTimeoutAsync(int gameId)
        {
            var g = await _games.GetById(gameId);
            if (g == null || g.Status != GameStatus.InProgress) return false;

            if (g.LastMoveAtUtc.HasValue && (DateTime.UtcNow - g.LastMoveAtUtc.Value) > MaxTurnWait)
            {
                g.Status = GameStatus.Finished;
                g.WinnerUserId = (g.CurrentTurnUserId == g.CreatorUserId) ? g.OpponentUserId : g.CreatorUserId;
                g.FinishedAtUtc = DateTime.UtcNow;
                await _games.UpdateAsync(g);
                return true;
            }
            return false;
        }

        public async Task<bool> IsMyTurnAsync(int gameId, string userId)
        {
            var g = await _games.GetById(gameId);
            return g != null && g.Status == GameStatus.InProgress && g.CurrentTurnUserId == userId;
        }

        public async Task<List<(int Row, int Col)>> GetMyOccupiedCellsAsync(int gameId, string userId)
        {
            var myShips = await _ships.GetShipsByGameAndOwnerAsync(gameId, userId);
            return myShips.SelectMany(s => GetCells(s.StartRow, s.StartCol, s.Length, s.Direction)).ToList();
        }

        public async Task<List<(int Row, int Col, bool IsHit)>> GetMyAttackMarksAsync(int gameId, string userId)
        {
            var attacks = await _attacks.GetAttacksByGameAndAttackerAsync(gameId, userId);
            return attacks.Select(a => (a.TargetRow, a.TargetCol, a.IsHit)).ToList();
        }

        public async Task<(bool Success, bool? IsHit, string? Error)> AttackAsync(int gameId, string attackerUserId, int row, int col)
        {
            var g = await _games.GetById(gameId);
            if (g == null || g.Status != GameStatus.InProgress) return (false, null, "La partida no está en progreso.");
            if (g.CurrentTurnUserId != attackerUserId) return (false, null, "No es tu turno.");
            if (!IsWithinBoard(row, col)) return (false, null, "Coordenada inválida.");

            var already = await _attacks.CellAlreadyAttackedAsync(gameId, attackerUserId, row, col);
            if (already) return (false, null, "Celda ya atacada.");

            var defenderId = attackerUserId == g.CreatorUserId ? g.OpponentUserId : g.CreatorUserId;
            var defenderShips = await _ships.GetShipsByGameAndOwnerAsync(gameId, defenderId);
            var defenderCells = defenderShips.SelectMany(s => GetCells(s.StartRow, s.StartCol, s.Length, s.Direction)).ToHashSet();

            var isHit = defenderCells.Contains((row, col));
            var attack = new BattleshipAttack
            {
                GameId = gameId,
                AttackerUserId = attackerUserId,
                TargetRow = row,
                TargetCol = col,
                IsHit = isHit,
                PerformedAtUtc = DateTime.UtcNow
            };
            await _attacks.AddAsync(attack);

            await UpdateSunkFlagsAsync(defenderShips, gameId, attackerUserId);

            var allCells = defenderShips.SelectMany(s => GetCells(s.StartRow, s.StartCol, s.Length, s.Direction)).ToList();
            var attackerAttacks = await _attacks.GetAttacksByGameAndAttackerAsync(gameId, attackerUserId);
            var hits = attackerAttacks.Where(a => a.IsHit).Select(a => (a.TargetRow, a.TargetCol)).ToHashSet();
            var defenderAllSunk = allCells.All(c => hits.Contains(c));

            if (defenderAllSunk)
            {
                g.Status = GameStatus.Finished;
                g.WinnerUserId = attackerUserId;
                g.FinishedAtUtc = DateTime.UtcNow;
            }
            else
            {
                g.CurrentTurnUserId = defenderId;
                g.LastMoveAtUtc = DateTime.UtcNow;
            }
            await _games.UpdateAsync(g);

            return (true, isHit, null);
        }

       

        private bool IsWithinBoard(int row, int col)
            => row >= 0 && row < BOARD && col >= 0 && col < BOARD;

        private List<(int Row, int Col)> GetCells(int startRow, int startCol, int length, Direction dir)
        {
            var cells = new List<(int, int)>();
            for (int i = 0; i < length; i++)
            {
                var (r, c) = dir switch
                {
                    Direction.Up => (startRow - i, startCol),
                    Direction.Down => (startRow + i, startCol),
                    Direction.Left => (startRow, startCol - i),
                    Direction.Right => (startRow, startCol + i),
                    _ => (startRow, startCol)
                };
                cells.Add((r, c));
            }
            return cells;
        }

        private async Task UpdateSunkFlagsAsync(List<BattleshipShip> defenderShips, int gameId, string attackerUserId)
        {
            var attackerAttacks = await _attacks.GetAttacksByGameAndAttackerAsync(gameId, attackerUserId);
            var hits = attackerAttacks.Where(a => a.IsHit).Select(a => (a.TargetRow, a.TargetCol)).ToHashSet();

            foreach (var s in defenderShips)
            {
                var allCells = GetCells(s.StartRow, s.StartCol, s.Length, s.Direction);
                var sunk = allCells.All(c => hits.Contains(c));
                if (s.IsSunk != sunk)
                {
                    s.IsSunk = sunk;
                    await _ships.UpdateAsync(s);
                }
            }
        }

        private BattleshipGameDto ToDto(BattleshipGame g, string currentUserId)
        {
            var dto = _mapper.Map<BattleshipGameDto>(g);
            dto.IsMyTurn = g.CurrentTurnUserId == currentUserId;
            dto.IAmCreator = g.CreatorUserId == currentUserId;
            return dto;
        }
    }
}
