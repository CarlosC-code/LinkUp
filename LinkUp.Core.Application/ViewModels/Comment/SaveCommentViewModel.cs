using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Comment
{
    public class SaveCommentViewModel
    {
        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }

        [Required(ErrorMessage = "El comentario no puede estar vacío")]
        [MaxLength(300)]
        public string? Text { get; set; }
    }
}
