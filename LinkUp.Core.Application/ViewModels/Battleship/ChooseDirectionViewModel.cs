
using System.ComponentModel.DataAnnotations;


namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class ChooseDirectionViewModel
    {

        public int GameId { get; set; }
        public int StartRow { get; set; }
        public int StartCol { get; set; }
        public int ShipLength { get; set; }

        [Required] 
        public int Direction { get; set; }
        public string? ErrorMessage { get; set; }

    }
}
