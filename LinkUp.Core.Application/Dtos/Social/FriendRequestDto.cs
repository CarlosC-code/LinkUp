

namespace LinkUp.Core.Application.Dtos.Social
{
    public class FriendRequestDto
    {

        public int Id { get; set; }
        public required string SenderUserId { get; set; }
        public required string ReceiverUserId { get; set; }
        public int Status { get; set; } 
        public DateTime SentAtUtc { get; set; }
        public DateTime? RespondedAtUtc { get; set; }

    
        public string? SenderUserName { get; set; }
        public string? ReceiverUserName { get; set; }
        public string? SenderProfileImage { get; set; }
        public string? ReceiverProfileImage { get; set; }
        public int MutualFriends { get; set; }

    }
}
