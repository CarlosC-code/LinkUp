using LinkUp.Core.Domain.Common;
using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Domain.Entities.Social
{
    public class Post : BasicEntity<int>
    {

        public required string UserId { get; set; } = default!; // Autor (Identity)
        public required string Content { get; set; } = default!; // Texto obligatorio

        public required MediaType MediaType { get; set; }
        public string? ImagePath { get; set; }          // Si MediaType=Image
        public string? YouTubeUrl { get; set; }         // Si MediaType=YouTube

        public required DateTime PublishedAtUtc { get; set; } = DateTime.UtcNow;

        // Navegación
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    }
}
