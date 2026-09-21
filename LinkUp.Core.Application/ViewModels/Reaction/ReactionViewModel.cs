using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.ViewModels.Reaction
{
    public class ReactionViewModel
    {
        public int PostId { get; set; }

        public required string UserId { get; set; }

        public ReactionType Type { get; set; }
    }
}
