
namespace LinkUp.Core.Application.Dtos.AccionBattleship
{
    public class CreateAttackDto
    {
        public required int GameId { get; set; }
        public required string AttackerUserId { get; set; }

        public required int TargetRow { get; set; }
        public required int TargetCol { get; set; }
    }
}
