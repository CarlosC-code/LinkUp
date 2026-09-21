using LinkUp.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El contenido es obligatorio")]
        [MaxLength(500)]
        public string? Content { get; set; }

        [Required]
        public MediaType MediaType { get; set; }

        public IFormFile? ImageFile { get; set; }

        [Url]
        public string? YouTubeUrl { get; set; }
    }
}
