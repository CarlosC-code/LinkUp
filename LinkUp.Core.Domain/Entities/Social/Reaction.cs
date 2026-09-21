using LinkUp.Core.Domain.Common;
using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Domain.Entities.Social
{
    public class Reaction : BasicEntity<int>
    {
        public required int PostId { get; set; }
        public required string UserId { get; set; } = default!;
        public required ReactionType Type { get; set; }

        // Navegación
        public Post? Post { get; set; }

    }
}
