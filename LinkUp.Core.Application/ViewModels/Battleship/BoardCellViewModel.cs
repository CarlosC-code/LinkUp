
namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class BoardCellViewModel
    {

        public int Row { get; set; }
        public int Col { get; set; }
        public bool IsOccupied { get; set; }      
        public bool? IsHit { get; set; }          
        public bool IsSelectable { get; set; } = true;

    }
}
