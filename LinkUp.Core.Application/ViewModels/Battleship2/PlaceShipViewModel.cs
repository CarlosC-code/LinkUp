using LinkUp.Core.Domain.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class PlaceShipViewModel
    {
        [Required]
        public int GameId { get; set; }

        [Required]
        public int Length { get; set; }

        [Range(0, 11)]
        public int StartRow { get; set; }

        [Range(0, 11)]
        public int StartCol { get; set; }

        [Required]
        public Direction Direction { get; set; }
    }
}
