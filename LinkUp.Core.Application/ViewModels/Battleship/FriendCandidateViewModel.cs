
namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class FriendCandidateViewModel
    {

        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string? ProfileImage { get; set; }
        public bool IsSelected { get; set; }

    }
}
