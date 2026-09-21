
using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class PlaceOnBoardViewModel
    {

        public int GameId { get; set; }

        [Required]
        public int SelectedShipLength { get; set; }

        public List<BoardCellViewModel> Board { get; set; } = new(); // 12x12

    }
}
