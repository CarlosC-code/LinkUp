using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class NewGameViewModel
    {

        public string? Search { get; set; }

        public List<FriendCandidateViewModel> Friends { get; set; } = new();

        [Required]
        public string? SelectedFriendUserId { get; set; }

    }
}
