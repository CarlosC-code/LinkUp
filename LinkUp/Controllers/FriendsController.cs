
using LinkUp.Core.Application.Dtos.Social;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.ViewModels.Social;
using LinkUp.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly IFriendshipService _friendships;
        private readonly IPostService _posts;
        private readonly ICommentService _comments;
        private readonly IReactionService _reactions;
        private readonly IAccountServiceForWebApp _accounts; 

        public FriendsController(
            IFriendshipService friendships,
            IPostService posts,
            ICommentService comments,
            IReactionService reactions,
            IAccountServiceForWebApp accounts)              
        {
            _friendships = friendships;
            _posts = posts;
            _comments = comments;
            _reactions = reactions;
            _accounts = accounts;                            
        }

        private string CurrentUserId => User.Identity!.Name!;

        
        // INDEX: publicaciones de mis amigos
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var friendIds = await _friendships.GetFriendIdsAsync(CurrentUserId);
            var posts = await _posts.GetFriendPostsAsync(friendIds, CurrentUserId);

            var vm = new HomeFeedViewModel
            {
                MyPosts = posts.Select(p => new PostItemViewModel
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

        
        // Publicaciones de un amigo específico
       
        [HttpGet]
        public async Task<IActionResult> FriendPosts(string id)
        {
            var posts = await _posts.GetSpecificFriendPostsAsync(id, CurrentUserId);

            var list = posts.Select(p => new PostItemViewModel
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
            }).ToList();

            
            var cache = new Dictionary<string, (string Name, string? Photo)>(StringComparer.OrdinalIgnoreCase);
            foreach (var p in list.Select(x => x.AuthorUserId).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var u = await _accounts.GetUserByUserName(p);
                cache[p] = (u?.UserName ?? p, u?.ProfileImage);
            }
            foreach (var post in list)
            {
                var data = cache[post.AuthorUserId];
                post.AuthorUserName = data.Name;
                post.AuthorProfileImage = data.Photo;
            }

            return View(list);
        }

        
        // Confirmación para eliminar amigo
        
        [HttpGet]
        public IActionResult ConfirmRemove(string friendUserId, string friendUserName)
        {
            ViewBag.FriendUserId = friendUserId;
            ViewBag.FriendUserName = friendUserName;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(string friendUserId)
        {
            await _friendships.DeleteFriendshipAsync(CurrentUserId, friendUserId);
            return RedirectToAction(nameof(Index));
        }

        
        // Acciones de interacción en posts 
       
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            await _comments.AddAsync(CurrentUserId, vm.PostId, vm.Text, vm.ParentCommentId);
            return RedirectToAction(nameof(Index));
        }

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

        
        // Helpers
    
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

