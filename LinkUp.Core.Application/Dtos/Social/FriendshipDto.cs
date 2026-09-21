

namespace LinkUp.Core.Application.Dtos.Social
{
    public class FriendshipDto
    {
        public int Id { get; set; }
        public required string UserAId { get; set; }
        public required string UserBId { get; set; }
        public DateTime SinceUtc { get; set; }

      
        public string? FriendUserId { get; set; }
        public string? FriendUserName { get; set; }
        public string? FriendProfileImage { get; set; }

    }
}
