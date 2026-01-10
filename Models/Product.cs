using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Pronia.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pronia.Models
{
    public class Product : BaseEntity
    {
  
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? SKU { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public string MainImageUrl { get; set; }
        
        public string HoverImageUrl { get; set; }

        public int Rating { get; set; }

        public ICollection<ProductTag> ProductTags { get; set; } = [];

        public ICollection<ProductImage> ProductImages { get; set; } = [];

        public ICollection<BasketItem> BasketItems { get; set; } = [];

    }
}
