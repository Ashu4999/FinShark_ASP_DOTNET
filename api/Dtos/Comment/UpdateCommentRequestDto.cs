using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must have 5 characters")]
        [MaxLength(50, ErrorMessage = "Title can't be over 50 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Title must have 5 characters")]
        [MaxLength(280, ErrorMessage = "Title can't be over 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}