namespace LinkUp.Core.Application.Dtos.Friendship
{
    public class FriendshipDto : BasicDto<int>
    {
        public required string UserId1 { get; set; }
        public required string UserId2 { get; set; }

        public required DateTime SinceUtc { get; set; }
    }
}
