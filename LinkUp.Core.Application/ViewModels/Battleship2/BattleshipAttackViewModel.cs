using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class BattleshipAttackViewModel
    {
        public int Id { get; set; }

        [Required]
        public int GameId { get; set; }

        [Required]
        public string? AttackerId { get; set; }

        [Required]
        [Range(0, 9, ErrorMessage = "Row must be between 0 and 9")]
        public int Row { get; set; }

        [Required]
        [Range(0, 9, ErrorMessage = "Column must be between 0 and 9")]
        public int Column { get; set; }

        public bool Hit { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
