using LinkUp.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Social
{
    public class PostCreateViewModel
    {

        [Required, StringLength(1000)]
        public string Content { get; set; } = default!;

        [Required]
        public MediaType MediaType { get; set; }

        
        public IFormFile? ImageFile { get; set; }

        
        [Url]
        public string? YouTubeUrl { get; set; }

    }
}
