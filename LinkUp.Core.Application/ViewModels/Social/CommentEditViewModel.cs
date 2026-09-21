
using System.ComponentModel.DataAnnotations;


namespace LinkUp.Core.Application.ViewModels.Social
{
    public class CommentEditViewModel
    {

        public int Id { get; set; }

        public int PostId { get; set; }

        [Required(ErrorMessage = "El comentario es requerido.")]
        [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        public string Text { get; set; } = default!;

    }
}
