using LinkUp.Core.Domain.Common;
using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Domain.Entities.Battleship
{
    public class BattleshipGame : BasicEntity<int>
    {


        public required string CreatorUserId { get; set; }
        public required string OpponentUserId { get; set; }

        public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAtUtc { get; set; }

        public required GameStatus Status { get; set; }

        public string? CurrentTurnUserId { get; set; }
        public DateTime? LastMoveAtUtc { get; set; }

        public string? WinnerUserId { get; set; }

        // NUEVO: control de fase de posicionamiento
        public bool CreatorReady { get; set; } = false;
        public bool OpponentReady { get; set; } = false;

    }
}
