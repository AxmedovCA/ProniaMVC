using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.ProductViewModels
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? SKU { get; set; }
        [Required]
        public int CategoryId { get; set; }
   
        [Required]
        public IFormFile MainImage { get; set; }

        [Required]
        public IFormFile HoverImage { get; set; }

        public int Rating {  get; set; }
    }
}
