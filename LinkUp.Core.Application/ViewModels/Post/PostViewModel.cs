using LinkUp.Core.Application.ViewModels.Comment;
using LinkUp.Core.Application.ViewModels.Reaction;
using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Content { get; set; }

        public MediaType MediaType { get; set; }

        public string? ImagePath { get; set; }

        public string? YouTubeUrl { get; set; }

        public DateTime PublishedAtUtc { get; set; }

        public List<CommentViewModel>? Comments { get; set; }

        public List<ReactionViewModel>? Reactions { get; set; }
    }
}
