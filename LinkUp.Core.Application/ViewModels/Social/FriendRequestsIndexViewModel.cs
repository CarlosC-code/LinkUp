

namespace LinkUp.Core.Application.ViewModels.Social
{
    public class FriendRequestsIndexViewModel
    {

        public List<PendingRequestViewModel> PendingReceived { get; set; } = new();
        public List<SentRequestViewModel> Sent { get; set; } = new();
        public int PendingCount => PendingReceived.Count;

    }
}
