

namespace LinkUp.Core.Application.Dtos.Battleship
{
    public class BattleshipShipDto
    {

        public int Id { get; set; }
        public required int GameId { get; set; }
        public required string OwnerUserId { get; set; }
        public required int Length { get; set; }
        public required int StartRow { get; set; }
        public required int StartCol { get; set; }
        public required int Direction { get; set; } 
        public required DateTime PlacedAtUtc { get; set; }
        public bool IsSunk { get; set; }

       
        public List<(int Row, int Col)> Cells { get; set; } = new();

    }
}
