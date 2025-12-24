using Pronia.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Shipping:BaseEntity
    {
    
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength (256)]
        public string Description { get; set; }
        [MaxLength(512),MinLength(3)]
        public string ImageUrl { get; set; }
    }
}
