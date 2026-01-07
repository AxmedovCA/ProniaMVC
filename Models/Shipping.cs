using Pronia.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Shipping:BaseEntity
    {
 
        public string Title { get; set; }
       
        public string? Description { get; set; }
      
        public string ImageUrl { get; set; }
    }
}
