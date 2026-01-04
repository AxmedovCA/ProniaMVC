using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Contexts;

namespace Pronia.Controllers
{
    public class DetailController(AppDbContext _context) : Controller
    {
        
        public IActionResult Index(int id)
        {
            var product = _context.Products.Include(x => x.Category).Include(x=>x.ProductTags).ThenInclude(x=>x.Tag).Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
