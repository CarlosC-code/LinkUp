
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.ViewModels.Social;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _comments;

        public CommentsController(ICommentService comments)
        {
            _comments = comments;
        }

        private string CurrentUser => User.Identity!.Name!;

        
        // CREATE
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int postId, string text, int? parentCommentId)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                TempData["Error"] = "El comentario no puede estar vacío.";
                return RedirectToAction("Index", "Home");
            }

            await _comments.AddAsync(CurrentUser, postId, text, parentCommentId);
            return RedirectToAction("Index", "Home");
        }

        
        // EDIT
       
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _comments.GetById(id);
            if (dto == null) return NotFound();
            if (!string.Equals(dto.UserId, CurrentUser, StringComparison.Ordinal)) return Forbid();

            var vm = new CommentEditViewModel
            {
                Id = dto.Id,
                PostId = dto.PostId,
                Text = dto.Text
            };

            return View(vm); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CommentEditViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var ok = await _comments.EditAsync(vm.Id, CurrentUser, vm.Text);
            if (!ok)
            {
                ModelState.AddModelError("", "No se pudo editar el comentario.");
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        
        // DELETE
       
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _comments.GetById(id);
            if (dto == null) return NotFound();
            if (!string.Equals(dto.UserId, CurrentUser, StringComparison.Ordinal)) return Forbid();

            var vm = new CommentEditViewModel
            {
                Id = dto.Id,
                PostId = dto.PostId,
                Text = dto.Text
            };

            return View(vm); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _comments.DeleteAsync(id, CurrentUser);
            return RedirectToAction("Index", "Home");
        }
    }
}
