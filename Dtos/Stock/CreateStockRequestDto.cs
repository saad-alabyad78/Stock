using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1 , 10000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.01 , 100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10)]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(0 , 100000000)]
        public long MarketCap { get; set; }

    }
}