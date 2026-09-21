namespace LinkUp.Core.Application.Dtos.AccionBattleship
{
    public class CreateBattleshipGameDto
    {
        public required string CreatorUserId { get; set; }
        public required string OpponentUserId { get; set; }
    }
}
