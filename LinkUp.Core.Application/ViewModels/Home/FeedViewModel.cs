using LinkUp.Core.Application.ViewModels.Post;

namespace LinkUp.Core.Application.ViewModels.Home
{
    public class FeedViewModel
    {
        public List<PostViewModel> Posts { get; set; } = new();
    }
}
