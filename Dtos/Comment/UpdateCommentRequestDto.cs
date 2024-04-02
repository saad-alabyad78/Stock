using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
     
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty ;
        
      
        [MinLength(5)]
        [MaxLength(50)]
        public string Content { get; set; } = string.Empty;
    }
}