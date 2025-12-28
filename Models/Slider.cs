using Pronia.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Models
{
    public class Slider : BaseEntity
    {

        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        [MaxLength(512), MinLength(3)]

        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public decimal DiscountPercentage { get; set; }
    }

}
