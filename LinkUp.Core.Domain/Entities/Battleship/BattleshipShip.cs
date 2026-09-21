using LinkUp.Core.Domain.Common;
using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Domain.Entities.Battleship
{
    public class BattleshipShip : BasicEntity<int>
    {
        public required int GameId { get; set; }
        public required string OwnerUserId { get; set; } = default!;
        public required int Length { get; set; }
        public required int StartRow { get; set; }
        public required int StartCol { get; set; }
        public required Direction Direction { get; set; }
        public required DateTime PlacedAtUtc { get; set; }

        // NUEVO: para saber si fue completamente hundido
        public bool IsSunk { get; set; } = false;

    }
}
