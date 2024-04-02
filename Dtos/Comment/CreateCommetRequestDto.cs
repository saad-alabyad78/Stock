using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class CreateCommetRequestDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty ;
        
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Content { get; set; } = string.Empty;
        public int? StockId { get; set; }
    }
}