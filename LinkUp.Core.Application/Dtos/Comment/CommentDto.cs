namespace LinkUp.Core.Application.Dtos.Comment
{
    public class CommentDto : BasicDto<int>
    {
        public required int PostId { get; set; }
        public required string UserId { get; set; }
        public required string Text { get; set; }

        public int? ParentCommentId { get; set; }

        public List<CommentDto>? Replies { get; set; }
    }
}
