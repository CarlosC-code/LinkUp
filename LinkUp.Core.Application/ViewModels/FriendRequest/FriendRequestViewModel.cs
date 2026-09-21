
namespace LinkUp.Core.Application.ViewModels.FriendRequest
{
    public class FriendRequestViewModel
    {

        public int Id { get; set; }

        public required string FromUserId { get; set; }

        public required string ToUserId { get; set; }

        public required string Status { get; set; }

        public DateTime RequestedAtUtc { get; set; }
    }
}
