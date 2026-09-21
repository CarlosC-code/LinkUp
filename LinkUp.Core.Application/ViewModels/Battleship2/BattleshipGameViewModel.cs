using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class BattleshipGameViewModel
    {
        public int Id { get; set; }

        public required string CreatorUserId { get; set; }

        public required string OpponentUserId { get; set; }

        public GameStatus Status { get; set; }

        public string? CurrentTurnUserId { get; set; }

        public string? WinnerUserId { get; set; }

        public DateTime StartedAtUtc { get; set; }

        public DateTime? FinishedAtUtc { get; set; }
    }
}
