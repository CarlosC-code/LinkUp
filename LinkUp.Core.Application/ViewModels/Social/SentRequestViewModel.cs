

namespace LinkUp.Core.Application.ViewModels.Social
{
    public class SentRequestViewModel
    {
        public int RequestId { get; set; }
        public string ReceiverUserId { get; set; } = default!;
        public string ReceiverUserName { get; set; } = default!;
        public string? ReceiverProfileImage { get; set; }
        public int MutualFriends { get; set; }
        public DateTime SentAtUtc { get; set; }
        public string StatusText { get; set; } = default!;

    }
}
