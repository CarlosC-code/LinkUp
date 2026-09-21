

using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.ViewModels.Social
{
    public class PostItemViewModel
    {

        public int Id { get; set; }
        public string AuthorUserId { get; set; } = default!;
        public string AuthorUserName { get; set; } = default!;
        public string? AuthorProfileImage { get; set; }
        public string Content { get; set; } = default!;
        public MediaType MediaType { get; set; }
        public string? ImagePath { get; set; }
        public string? YouTubeUrl { get; set; }
        public DateTime PublishedAtUtc { get; set; }

        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int? MyReaction { get; set; } // 1/2
        public List<CommentThreadViewModel> Comments { get; set; } = new();
        public bool IsOwner { get; set; }

    }
}
