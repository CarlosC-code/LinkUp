
namespace LinkUp.Core.Application.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string? UserId { get; set; }

        public string? Text { get; set; }

        public int? ParentCommentId { get; set; }

        public List<CommentViewModel>? Replies { get; set; }
    }
}
