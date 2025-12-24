using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Pronia.Models.Common;
namespace Pronia.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Precision(10,2)]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? SKU { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        [Required]
        public string MainImageUrl { get; set; }
        [Required]
        public string HoverImageUrl { get; set; }
    }
}
