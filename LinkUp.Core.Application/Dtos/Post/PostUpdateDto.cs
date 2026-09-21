

namespace LinkUp.Core.Application.Dtos.Post
{
    public class PostUpdateDto
    {

        public required int Id { get; set; }
        public required string UserId { get; set; }
        public required string Content { get; set; }
        public required int MediaType { get; set; } // 1=Image, 2=YouTube
        public string? YouTubeUrl { get; set; }

    }
}
