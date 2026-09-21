
using LinkUp.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp.Controllers
{
    [Authorize]
    public class ReactionsController : Controller
    {
        private readonly IReactionService _reactions;

        public ReactionsController(IReactionService reactions)
        {
            _reactions = reactions;
        }

        private string CurrentUserId => User.Identity!.Name!;

        // 1 = Like, 2 = Dislike
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int postId, int type)
        {
            await _reactions.ReactAsync(CurrentUserId, postId, type);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int postId)
        {
            await _reactions.RemoveReactionAsync(CurrentUserId, postId);
            return RedirectToAction("Index", "Home");
        }
    }
}
