using LinkUp.Core.Application.Dtos.Comment;
using LinkUp.Core.Application.Dtos.Reaction;
using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.Dtos.Post
{
    public class PostDto : BasicDto<int>
    {
        public required string UserId { get; set; }
        public required string Content { get; set; }
        public required MediaType MediaType { get; set; }

        public string? ImagePath { get; set; }
        public string? YouTubeUrl { get; set; }

        public required DateTime PublishedAtUtc { get; set; }

        public List<CommentDto>? Comments { get; set; }
        public List<ReactionDto>? Reactions { get; set; }
    }
}
