

namespace LinkUp.Core.Application.Dtos.Social
{
    public class CommentDto
    {
        public int Id { get; set; }
        public required int PostId { get; set; }
        public required string UserId { get; set; }
        public required string Text { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public List<CommentDto> Replies { get; set; } = new();

   
        public string? UserName { get; set; }
        public string? UserProfileImage { get; set; }
        public bool IsOwner { get; set; }

    }
}
