
namespace LinkUp.Core.Application.Dtos.Battleship
{
    public class BattleshipAttackDto
    {

        public int Id { get; set; }
        public required int GameId { get; set; }
        public required string AttackerUserId { get; set; }
        public required int TargetRow { get; set; }
        public required int TargetCol { get; set; }
        public required bool IsHit { get; set; }
        public DateTime PerformedAtUtc { get; set; }

    }
}
