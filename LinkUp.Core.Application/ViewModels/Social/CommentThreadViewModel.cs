
namespace LinkUp.Core.Application.ViewModels.Social
{
    public class CommentThreadViewModel
    {

        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string? UserProfileImage { get; set; }
        public string Text { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; }
        public bool IsOwner { get; set; }
        public List<CommentThreadViewModel> Replies { get; set; } = new();

    }
}
