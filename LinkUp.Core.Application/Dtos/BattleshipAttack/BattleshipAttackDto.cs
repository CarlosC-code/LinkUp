namespace LinkUp.Core.Application.Dtos.BattleshipAttack
{
    public class BattleshipAttackDto : BasicDto<int>
    {
        public required int GameId { get; set; }
        public required string AttackerUserId { get; set; }

        public required int TargetRow { get; set; }
        public required int TargetCol { get; set; }

        public required bool IsHit { get; set; }

        public required DateTime PerformedAtUtc { get; set; }
    }
}
