using Microsoft.AspNetCore.Mvc;
using Pronia.Contexts;

namespace Pronia.Controllers
{
    public class ProductController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
    }
}
