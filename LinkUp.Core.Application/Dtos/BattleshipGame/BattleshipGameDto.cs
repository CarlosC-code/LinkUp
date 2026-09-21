using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.Dtos.BattleshipGame
{
    public class BattleshipGameDto : BasicDto<int>
    {
        public required string CreatorUserId { get; set; }
        public required string OpponentUserId { get; set; }

        public required DateTime StartedAtUtc { get; set; }
        public DateTime? FinishedAtUtc { get; set; }

        public required GameStatus Status { get; set; }

        public string? CurrentTurnUserId { get; set; }
        public DateTime? LastMoveAtUtc { get; set; }

        public string? WinnerUserId { get; set; }
    }
}
