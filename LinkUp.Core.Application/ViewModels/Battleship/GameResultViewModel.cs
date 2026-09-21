

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class GameResultViewModel
    {

        public int GameId { get; set; }
        public string OpponentUserName { get; set; } = default!;
        public DateTime StartedAtUtc { get; set; }
        public DateTime FinishedAtUtc { get; set; }
        public double DurationHours => (FinishedAtUtc - StartedAtUtc).TotalHours;
        public bool IsWinner { get; set; }

        
        public List<BoardCellViewModel> MyAttackBoard { get; set; } = new();
        public List<BoardCellViewModel> OpponentAttackBoard { get; set; } = new();
        public List<BoardCellViewModel> MyPlacementBoard { get; set; } = new();

    }
}
