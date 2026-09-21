

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class HistoryGameItemViewModel
    {

        public int GameId { get; set; }
        public string OpponentUserName { get; set; } = default!;
        public DateTime StartedAtUtc { get; set; }
        public DateTime FinishedAtUtc { get; set; }
        public double DurationHours => (FinishedAtUtc - StartedAtUtc).TotalHours;
        public bool IsWinner { get; set; }
        public string WinnerText { get; set; } = default!;

    }
}
