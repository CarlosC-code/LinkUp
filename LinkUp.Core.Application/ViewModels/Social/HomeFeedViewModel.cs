

namespace LinkUp.Core.Application.ViewModels.Social
{
    public class HomeFeedViewModel
    {

        public List<PostItemViewModel> MyPosts { get; set; } = new();
        public PostCreateViewModel NewPost { get; set; } = new();
        public int PendingRequestsCount { get; set; }

    }
}
