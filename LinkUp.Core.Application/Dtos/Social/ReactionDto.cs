

namespace LinkUp.Core.Application.Dtos.Social
{
    public class ReactionDto
    {

        public int Id { get; set; }
        public required int PostId { get; set; }
        public required string UserId { get; set; }
        public required int Type { get; set; } 
        public DateTime CreatedAtUtc { get; set; }

    }
}
