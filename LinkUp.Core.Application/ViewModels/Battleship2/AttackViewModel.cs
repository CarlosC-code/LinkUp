using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class AttackViewModel
    {
        [Required]
        public int GameId { get; set; }

        [Range(0, 11)]
        public int TargetRow { get; set; }

        [Range(0, 11)]
        public int TargetCol { get; set; }
    }
}
