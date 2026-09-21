using LinkUp.Core.Domain.Common;

namespace LinkUp.Core.Domain.Entities.Social
{
    public class Comment : BasicEntity<int>
    {
        public required int PostId { get; set; }
        public required string UserId { get; set; } = default!;
        public required string Text { get; set; } = default!;

        public int? ParentCommentId { get; set; } // null = comentario raíz; no null = reply

        // Navegación
        public Post? Post { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment>? Replies { get; set; } 

    }
}
