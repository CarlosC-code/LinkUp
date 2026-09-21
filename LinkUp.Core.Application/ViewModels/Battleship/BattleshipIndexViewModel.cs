
namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class BattleshipIndexViewModel
    {

        public List<ActiveGameItemViewModel> ActiveGames { get; set; } = new();
        public List<HistoryGameItemViewModel> History { get; set; } = new();

       
        public int TotalPlayed => History.Count;
        public int Won => History.Count(h => h.IsWinner);
        public int Lost => History.Count(h => !h.IsWinner);

    }
}
