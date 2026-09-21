using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.Dtos.Reaction
{
    public class ReactionDto : BasicDto<int>
    {
        public required int PostId { get; set; }
        public required string UserId { get; set; }
        public required ReactionType Type { get; set; }
    }
}
