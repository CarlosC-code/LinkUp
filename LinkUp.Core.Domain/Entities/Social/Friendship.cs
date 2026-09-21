using LinkUp.Core.Domain.Common;

namespace LinkUp.Core.Domain.Entities.Social
{
    public class Friendship : BasicEntity<int>
    {
        // CORREGIDO: renombrado para consistencia con los repositorios
        public required string UserAId { get; set; }
        public required string UserBId { get; set; }
        public DateTime SinceUtc { get; set; } = DateTime.UtcNow;

    }
}
