using LinkUp.Core.Application.Dtos.Social;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.ViewModels.Social;
using LinkUp.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostService _posts;
        private readonly ICommentService _comments;
        private readonly IReactionService _reactions;
        private readonly IFriendRequestService _requests;
        private readonly IAccountServiceForWebApp _accounts; 

        public HomeController(
            IPostService posts,
            ICommentService comments,
            IReactionService reactions,
            IFriendRequestService requests,
            IAccountServiceForWebApp accounts)         
        {
            _posts = posts;
            _comments = comments;
            _reactions = reactions;
            _requests = requests;
            _accounts = accounts;                        
        }

        private string CurrentUserId => User.Identity!.Name!;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var myPosts = await _posts.GetMyPostsAsync(CurrentUserId);
            var pendingCount = await _requests.GetPendingCountAsync(CurrentUserId);

            var vm = new HomeFeedViewModel
            {
                PendingRequestsCount = pendingCount,
                MyPosts = myPosts.Select(p => new PostItemViewModel
                {
                    Id = p.Id,
                    AuthorUserId = p.UserId,
                    AuthorUserName = p.UserName ?? p.UserId,
                    AuthorProfileImage = p.UserProfileImage,
                    Content = p.Content,
                    MediaType = (MediaType)p.MediaType,
                    ImagePath = p.ImagePath,
                    YouTubeUrl = p.YouTubeUrl,
                    PublishedAtUtc = p.PublishedAtUtc,
                    Likes = p.Likes,
                    Dislikes = p.Dislikes,
                    MyReaction = p.Reactions.FirstOrDefault(r => r.UserId == CurrentUserId)?.Type,
                    Comments = (p.Comments ?? new()).Select(ToCommentThreadVm(CurrentUserId)).ToList(),
                    IsOwner = p.UserId == CurrentUserId
                }).ToList()
            };

            
            var distinctAuthors = vm.MyPosts.Select(x => x.AuthorUserId).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            var cache = new Dictionary<string, (string Name, string? Photo)>(StringComparer.OrdinalIgnoreCase);
            foreach (var authorId in distinctAuthors)
            {
                var u = await _accounts.GetUserByUserName(authorId);
                cache[authorId] = (u?.UserName ?? authorId, u?.ProfileImage);
            }
            foreach (var post in vm.MyPosts)
            {
                var data = cache[post.AuthorUserId];
                post.AuthorUserName = data.Name;
                post.AuthorProfileImage = data.Photo;
            }

            return View(vm);
        }

        //  Acciones rapidas para reacciones/comentarios en el feed
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> React(int postId, int type)
        {
            await _reactions.ReactAsync(CurrentUserId, postId, type);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveReaction(int postId)
        {
            await _reactions.RemoveReactionAsync(CurrentUserId, postId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int postId, string text, int? parentCommentId)
        {
            if (!string.IsNullOrWhiteSpace(text))
                await _comments.AddAsync(CurrentUserId, postId, text, parentCommentId);
            return RedirectToAction(nameof(Index));
        }

        //  helper
        private static Func<CommentDto, CommentThreadViewModel> ToCommentThreadVm(string currentUserId)
            => dto => new CommentThreadViewModel
            {
                Id = dto.Id,
                UserId = dto.UserId,
                UserName = dto.UserName ?? dto.UserId,
                UserProfileImage = dto.UserProfileImage,
                Text = dto.Text,
                CreatedAtUtc = dto.CreatedAtUtc,
                IsOwner = dto.UserId == currentUserId,
                Replies = dto.Replies?.Select(ToCommentThreadVm(currentUserId)).ToList() ?? new()
            };
    }
}


