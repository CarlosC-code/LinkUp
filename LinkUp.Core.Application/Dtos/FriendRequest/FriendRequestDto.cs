using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.Dtos.FriendRequest
{
    public class FriendRequestDto : BasicDto<int>
    {
        public required string FromUserId { get; set; }
        public required string ToUserId { get; set; }
        public required FriendRequestStatus Status { get; set; }

        public required DateTime RequestedAtUtc { get; set; }
        public DateTime? RespondedAtUtc { get; set; }
    }
}
