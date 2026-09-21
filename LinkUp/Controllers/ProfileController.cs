
using LinkUp.Core.Application.Dtos.User;             // SaveUserDto, EditResponseDto (tu DTO)
using LinkUp.Core.Application.Interfaces;            // IAccountServiceForWebApp
using LinkUp.Core.Application.ViewModels.Social;     // ProfileEditViewModel
using LinkUp.Helpers;                                // FileManager
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IAccountServiceForWebApp _accounts;

        public ProfileController(IAccountServiceForWebApp accounts)
        {
            _accounts = accounts;
        }

        private string RawUserName => User.Identity!.Name!;

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            // Trae SIEMPRE el usuario canónico de tu servicio
            var me = await _accounts.GetUserByUserName(RawUserName);
            if (me == null) return NotFound();

            var vm = new ProfileEditViewModel
            {
                UserName = me.UserName ?? "",
                FirstName = me.Name ?? "",
                LastName = me.LastName ?? "",
                Phone = me.Phone ?? "",
                ExistingImagePath = me.ProfileImage
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileEditViewModel vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // 0) Usuario canónico
            var me = await _accounts.GetUserByUserName(RawUserName);
            if (me == null)
            {
                ModelState.AddModelError("", "Usuario no encontrado.");
                return View(vm);
            }
            var canonicalUserName = me.UserName ?? RawUserName;

            // 1) Imagen (opcional) usando SIEMPRE el userName canónico
            string? newImagePath = vm.ExistingImagePath;
            if (vm.ProfileImage != null)
            {
                newImagePath = await FileManager.UploadAsync(
                    file: vm.ProfileImage,
                    id: canonicalUserName,
                    folderName: "profiles",
                    isEditMode: !string.IsNullOrEmpty(vm.ExistingImagePath),
                    imagePath: vm.ExistingImagePath,
                    ct: ct
                );

                if (string.IsNullOrEmpty(newImagePath))
                {
                    ModelState.AddModelError("", "No se pudo guardar la imagen de perfil.");
                    return View(vm);
                }
            }

            // 2) Crea el SaveUserDto con los NOMBRES que usa tu API
            var saveDto = new SaveUserDto
            {
                UserName = canonicalUserName,
                Name = vm.FirstName.Trim(),
                LastName = vm.LastName.Trim(),
                Phone = vm.Phone.Trim(),
                ProfileImage = newImagePath
            };

            // (Opcional) si tu SaveUserDto realmente trae más campos obligatorios, asígnalos aquí:
            // saveDto.Email = me.Email;
            // saveDto.Id    = me.Id;

            // 3) Guardar
            var origin = $"{Request.Scheme}://{Request.Host}";
            var editRes = await _accounts.EditUser(saveDto, origin, isCreated: false);

            // 3.1) TU DTO no tiene 'Succeeded', usa 'HasError' + 'Errors'
            if (editRes == null || editRes.HasError)
            {
                // Mensaje genérico
                ModelState.AddModelError("", "No se pudo actualizar el perfil.");
                // Mensajes detallados del servicio (si vienen)
                if (editRes?.Errors != null)
                {
                    foreach (var e in editRes.Errors)
                        if (!string.IsNullOrWhiteSpace(e))
                            ModelState.AddModelError("", e);
                }
                return View(vm);
            }

            // 4) Cambio de contraseña (opcional) — SOLO si ya tienes ResetPasswordRequestDto/ResetPasswordAsync funcionando

            //if (!string.IsNullOrWhiteSpace(vm.NewPassword))
            //{
            //    var me = await _accounts.GetUserByUserName(User.Identity!.Name!);
            //    if (me == null)
            //    {
            //        ModelState.AddModelError("", "Usuario no encontrado.");
            //        return View(vm);
            //    }

            //    var change = await _accounts.ResetPasswordAsync(me.Id, vm.NewPassword!);
            //    if (change == null || change.HasError)
            //    {
            //        ModelState.AddModelError("", "No se pudo cambiar la contraseña.");
            //        if (change?.Errors != null)
            //            foreach (var e in change.Errors)
            //                if (!string.IsNullOrWhiteSpace(e))
            //                    ModelState.AddModelError("", e);
            //        return View(vm);
            //    }
            //}


            TempData["msg"] = "Perfil actualizado.";
            // Si prefieres ver el cambio al instante, regresa a tu mismo perfil:
            // return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Posts");
        }
    }
}
