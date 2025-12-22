using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        [MaxLength (512),MinLength(3)]
		[Required]
		public string ImageUrl { get; set; }
        public decimal DiscountPercentage { get; set; }
    }

}
