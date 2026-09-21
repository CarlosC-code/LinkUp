
using LinkUp.Core.Application.Dtos.Battleship; 

namespace LinkUp.Core.Application.Interfaces
{
    public interface IBattleshipService : IGenericService<BattleshipGameDto>
    {
        Task<List<BattleshipGameDto>> GetActiveGamesAsync(string userId);
        Task<List<BattleshipGameDto>> GetFinishedGamesAsync(string userId);
        Task<BattleshipGameDto?> GetByIdAsync(int gameId, string currentUserId);
        Task<int?> StartNewGameAsync(string creatorUserId, string opponentUserId);

        Task<List<int>> GetRemainingShipsAsync(int gameId, string userId);
        Task<bool> PlaceShipAsync(int gameId, string userId, int length, int startRow, int startCol, int direction);
        Task<bool> AreBothReadyAsync(int gameId);

       
        Task<bool> IsMyTurnAsync(int gameId, string userId);
        Task<(bool Success, bool? IsHit, string? Error)> AttackAsync(int gameId, string attackerUserId, int row, int col);
        Task<List<(int Row, int Col)>> GetMyOccupiedCellsAsync(int gameId, string userId);
        Task<List<(int Row, int Col, bool IsHit)>> GetMyAttackMarksAsync(int gameId, string userId);

       
        Task<bool> SurrenderAsync(int gameId, string userId);
        Task<bool> FinishIfTimeoutAsync(int gameId);
    }
}
