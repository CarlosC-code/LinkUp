namespace LinkUp.Core.Application.Dtos.Social
{
    public class PostDto
    {

        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string Content { get; set; }
        public required int MediaType { get; set; } 
        public string? ImagePath { get; set; }
        public string? YouTubeUrl { get; set; }
        public DateTime PublishedAtUtc { get; set; }

        public List<CommentDto> Comments { get; set; } = new();
        public List<ReactionDto> Reactions { get; set; } = new();

        
        public int Likes => Reactions.Count(r => r.Type == 1);
        public int Dislikes => Reactions.Count(r => r.Type == 2);
        public string? UserName { get; set; }     
        public string? UserProfileImage { get; set; }

    }
}
