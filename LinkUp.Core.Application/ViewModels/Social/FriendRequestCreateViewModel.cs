

using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Social
{
    public class FriendRequestCreateViewModel
    {

        [Required]
        public string? TargetUserId { get; set; }

        public string? Search { get; set; }

        public List<SelectUserViewModel> Candidates { get; set; } = new();

    }
}
