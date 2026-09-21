
namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class AttackBoardViewModel
    {

        public int GameId { get; set; }
        public List<BoardCellViewModel> Board { get; set; } = new();
        public bool IsMyTurn { get; set; }
        public string TurnMessage { get; set; } = string.Empty;

    }
}
