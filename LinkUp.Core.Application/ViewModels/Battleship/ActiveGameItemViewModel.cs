
namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class ActiveGameItemViewModel
    {

        public int GameId { get; set; }
        public string OpponentUserId { get; set; } = default!;
        public string OpponentUserName { get; set; } = default!;
        public string? OpponentProfileImage { get; set; }
        public DateTime StartedAtUtc { get; set; }
        public double Hours => (DateTime.UtcNow - StartedAtUtc).TotalHours;
        public bool IsMyTurn { get; set; }

    }
}
