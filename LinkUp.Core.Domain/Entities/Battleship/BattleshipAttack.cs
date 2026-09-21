using LinkUp.Core.Domain.Common;

namespace LinkUp.Core.Domain.Entities.Battleship
{
    public class BattleshipAttack : BasicEntity<int>
    {
        public required int GameId { get; set; }
        public required string AttackerUserId { get; set; } 
        public required int TargetRow { get; set; } // 0..11
        public required int TargetCol { get; set; } // 0..11
        public required bool IsHit { get; set; } // true=rojo, false=verde
        public required DateTime PerformedAtUtc { get; set; } = DateTime.UtcNow;

    }
}
