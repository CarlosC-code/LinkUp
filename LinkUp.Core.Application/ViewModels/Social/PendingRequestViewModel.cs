

namespace LinkUp.Core.Application.ViewModels.Social
{
    public class PendingRequestViewModel
    {
        public int RequestId { get; set; }
        public string SenderUserId { get; set; } = default!;
        public string SenderUserName { get; set; } = default!;
        public string? SenderProfileImage { get; set; }
        public int MutualFriends { get; set; }
        public DateTime SentAtUtc { get; set; }

    }
}
