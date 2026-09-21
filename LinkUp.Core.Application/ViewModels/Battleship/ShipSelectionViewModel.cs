

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class ShipSelectionViewModel
    {
        public int GameId { get; set; }
        public List<int> RemainingShips { get; set; } = new(); 

    }
}
