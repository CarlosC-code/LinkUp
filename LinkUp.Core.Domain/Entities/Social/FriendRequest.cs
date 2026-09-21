using LinkUp.Core.Domain.Common;
using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Domain.Entities.Social
{
    public class FriendRequest : BasicEntity<int>
    {
        // CORREGIDO: renombrado para consistencia con los repositorios
        public required string SenderUserId { get; set; }
        public required string ReceiverUserId { get; set; }

        public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;
        public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAtUtc { get; set; }

    }
}
