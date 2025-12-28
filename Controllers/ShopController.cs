using Microsoft.AspNetCore.Mvc;
using Pronia.Contexts;
using Pronia.ViewModels.ProductViewModels;

namespace Pronia.Controllers
{
    public class ShopController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            var products = _context.Products.Select(product=>new ProductGetVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryName = product.Category.Name,
                HoverImageUrl = product.HoverImageUrl,
                Price = product.Price,  
                SKU = product.SKU,
                MainImageUrl = product.MainImageUrl,
                Rating = product.Rating,
            }).ToList();
            return View(products);
        }
    }
}
