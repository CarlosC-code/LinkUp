using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Social
{
    public class ProfileEditViewModel
    {

        [Required(ErrorMessage = "El nombre es requerido.")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = default!;

        [Required(ErrorMessage = "El apellido es requerido.")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = default!;

        [Required(ErrorMessage = "El teléfono es requerido.")]
        [Display(Name = "Teléfono")]
        
        [RegularExpression(@"^(?:\+?1)?[ -]?\(?(809|829|849)\)?[ -]?\d{3}[ -]?\d{4}$",
            ErrorMessage = "El teléfono debe ser un número válido de RD (809/829/849).")]
        public string Phone { get; set; } = default!;

        [Display(Name = "Foto de perfil")]
        public IFormFile? ProfileImage { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmPassword { get; set; }

        
        public string? ExistingImagePath { get; set; }
        public string UserName { get; set; } = default!;

    }
}
