using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Social
{
    public class CommentCreateViewModel
    {

        [Required]
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }

        [Required, StringLength(500)]
        public string Text { get; set; } = default!;
        public int Id { get; set; }
    }
}
