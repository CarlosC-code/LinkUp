

namespace LinkUp.Core.Application.ViewModels.Social
{
    public class SelectUserViewModel
    {
        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string? ProfileImage { get; set; }
        public int MutualFriends { get; set; }
        public bool IsSelected { get; set; }

    }
}
