using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Contexts;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            var products = _context.Products.Include(x=>x.Category).ToList();
            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            SendCategoriesWithViewBag();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (product.Price < 0)
            {
                ModelState.AddModelError(nameof(product.Price), "Price menfi ola bilmez");
            }

            if (!ModelState.IsValid)
            {
                SendCategoriesWithViewBag();
                return View(product);
            }

            var isExistCategory = _context.Categories.Any(x => x.Id == product.CategoryId);
            if(!isExistCategory) {
                SendCategoriesWithViewBag();
                ModelState.AddModelError("", "Bu kateqoriya movcud deyil");
            return View(product);
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            SendCategoriesWithViewBag();
            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (!ModelState.IsValid)
            {
                SendCategoriesWithViewBag();
                return View(product);
            }
            var isExistProduct = _context.Products.Find(product.Id);

            if(isExistProduct == null)
            {
                return NotFound();
            }
            var isExistCategory = _context.Categories.Any(x => x.Id == product.CategoryId);
            if(!isExistCategory)
            {
                SendCategoriesWithViewBag();
                ModelState.AddModelError("CategoryId", "Bu kateqori mevcud deyil");
                return View(product);
            }

            isExistProduct.Name = product.Name;
            isExistProduct.Description = product.Description;
            isExistProduct.SKU = product.SKU;
            isExistProduct.CategoryId = product.CategoryId;
            isExistProduct.Price = product.Price;
            isExistProduct.MainImageUrl = product.MainImageUrl;
            isExistProduct.HoverImageUrl = product.HoverImageUrl;
            _context.Products.Update(isExistProduct);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        private void SendCategoriesWithViewBag()
        {
            var categories = _context.Categories.ToList();

            ViewBag.Categories = categories;
        }
    }
}
