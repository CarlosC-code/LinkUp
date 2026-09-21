
namespace LinkUp.Core.Application.ViewModels.Friendship
{
    public class FriendshipViewModel
    {
        public int Id { get; set; }

        public required string UserId1 { get; set; }

        public required string UserId2 { get; set; }

        public DateTime SinceUtc { get; set; }
    }
}
