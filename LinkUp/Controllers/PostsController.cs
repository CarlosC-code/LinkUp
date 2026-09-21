
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.ViewModels.Social;
using LinkUp.Core.Domain.Common.Enums;
using LinkUp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace LinkUp.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService _posts;
        private readonly ICommentService _comments;
        private readonly IReactionService _reactions;

        public PostsController(
            IPostService posts,
            ICommentService comments,
            IReactionService reactions)
        {
            _posts = posts;
            _comments = comments;
            _reactions = reactions;
        }

        private string CurrentUserId => User.Identity!.Name!;

        // ==========================================
        // CREATE
        // ==========================================

        [HttpGet]
        public IActionResult Create()
            => PartialView("_CreatePost", new PostCreateViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateViewModel vm, CancellationToken ct)
        {
            try
            {
                // Si falla el ModelState, mandamos todo a Home con TempData
                if (!ModelState.IsValid)
                {
                    TempData["PostError"] = JoinModelStateErrors(ModelState);
                    return RedirectToAction("Index", "Home");
                }

                // Reglas por tipo de medio
                if (vm.MediaType == MediaType.Image && vm.ImageFile == null)
                {
                    TempData["PostError"] = "Debe subir una imagen.";
                    return RedirectToAction("Index", "Home");
                }

                if (vm.MediaType == MediaType.YouTube)
                {
                    if (string.IsNullOrWhiteSpace(vm.YouTubeUrl))
                    {
                        TempData["PostError"] = "Debe pegar un enlace de YouTube.";
                        return RedirectToAction("Index", "Home");
                    }

                    // (Opcional) Si quieres validar el ID del video, aquí puedes hacerlo.
                    // if (!YouTubeHelper.TryGetVideoId(vm.YouTubeUrl, out var _)) {
                    //     TempData["PostError"] = "El enlace de YouTube no es válido.";
                    //     return RedirectToAction("Index", "Home");
                    // }
                }

                // Carga de imagen si aplica
                string? imagePath = null;
                if (vm.MediaType == MediaType.Image && vm.ImageFile != null)
                {
                    imagePath = await FileManager.UploadAsync(
                        file: vm.ImageFile,
                        id: CurrentUserId,
                        folderName: "posts",
                        isEditMode: false,
                        imagePath: null,
                        ct: ct
                    );

                    if (string.IsNullOrEmpty(imagePath))
                    {
                        TempData["PostError"] = "No se pudo guardar la imagen.";
                        return RedirectToAction("Index", "Home");
                    }
                }

                // Crear post
                var created = await _posts.CreateAsync(
                    userId: CurrentUserId,
                    content: vm.Content!,
                    mediaType: (int)vm.MediaType,
                    imagePath: imagePath,
                    youTubeUrl: vm.MediaType == MediaType.YouTube ? vm.YouTubeUrl : null
                );

                if (created == null)
                {
                    TempData["PostError"] = "No se pudo crear la publicación.";
                    return RedirectToAction("Index", "Home");
                }

                TempData["PostOk"] = "Publicación creada correctamente.";
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                TempData["PostError"] = "Ocurrió un error al crear la publicación.";
                return RedirectToAction("Index", "Home");
            }
        }

        // ==========================================
        // EDIT
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _posts.GetById(id);
            if (dto == null) return NotFound();

            // Seguridad: solo el autor
            if (!string.Equals(dto.UserId, CurrentUserId, StringComparison.Ordinal))
                return Forbid();

            var vm = new PostEditViewModel
            {
                Id = dto.Id,
                Content = dto.Content,
                MediaType = (MediaType)dto.MediaType,
                YouTubeUrl = dto.YouTubeUrl,
                ExistingImagePath = dto.ImagePath
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostEditViewModel vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.MediaType == MediaType.YouTube && string.IsNullOrWhiteSpace(vm.YouTubeUrl))
            {
                ModelState.AddModelError(nameof(vm.YouTubeUrl), "Debe pegar un enlace de YouTube.");
                return View(vm);
            }

            // Imagen nueva si aplica; si no, conservar
            string? newImagePath = vm.ExistingImagePath;
            if (vm.MediaType == MediaType.Image && vm.ImageFile != null)
            {
                newImagePath = await FileManager.UploadAsync(
                    file: vm.ImageFile,
                    id: CurrentUserId,
                    folderName: "posts",
                    isEditMode: !string.IsNullOrEmpty(vm.ExistingImagePath),
                    imagePath: vm.ExistingImagePath,
                    ct: ct
                );

                if (string.IsNullOrEmpty(newImagePath))
                {
                    ModelState.AddModelError("", "No se pudo guardar la imagen.");
                    return View(vm);
                }
            }

            var updated = await _posts.EditAsync(
                postId: vm.Id,
                userId: CurrentUserId,
                content: vm.Content!,
                mediaType: (int)vm.MediaType,
                imagePath: vm.MediaType == MediaType.Image ? newImagePath : null,
                youTubeUrl: vm.MediaType == MediaType.YouTube ? vm.YouTubeUrl : null
            );

            if (updated == null)
            {
                ModelState.AddModelError("", "No se pudo editar la publicación.");
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // DELETE
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _posts.GetById(id);
            if (dto == null) return NotFound();

            if (!string.Equals(dto.UserId, CurrentUserId, StringComparison.Ordinal))
                return Forbid();

            var vm = new PostItemViewModel
            {
                Id = dto.Id,
                Content = dto.Content,
                MediaType = (MediaType)dto.MediaType,
                ImagePath = dto.ImagePath,
                YouTubeUrl = dto.YouTubeUrl,
                PublishedAtUtc = dto.PublishedAtUtc,
                AuthorUserId = dto.UserId,
                AuthorUserName = dto.UserName ?? dto.UserId
            };
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _posts.DeleteAsync(id, CurrentUserId);
            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // Interacciones (si las usas aquí)
        // ==========================================
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int postId, string text, int? parentCommentId)
        {
            if (!string.IsNullOrWhiteSpace(text))
                await _comments.AddAsync(CurrentUserId, postId, text, parentCommentId);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> React(int postId, int type)
        {
            await _reactions.ReactAsync(CurrentUserId, postId, type);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveReaction(int postId)
        {
            await _reactions.RemoveReactionAsync(CurrentUserId, postId);
            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // Helpers
        // ==========================================
        private static string JoinModelStateErrors(ModelStateDictionary modelState)
        {
            var sb = new StringBuilder();
            foreach (var kv in modelState.Values)
            {
                foreach (var err in kv.Errors)
                {
                    var msg = string.IsNullOrWhiteSpace(err.ErrorMessage) ? "Error en el formulario." : err.ErrorMessage;
                    if (sb.Length > 0) sb.Append(" | ");
                    sb.Append(msg);
                }
            }
            return sb.ToString();
        }
    }
}
