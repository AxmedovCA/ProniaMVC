using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Contexts;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.ViewModels.ProductViewModels;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(AppDbContext _context, IWebHostEnvironment _environment) : Controller
    {
        public IActionResult Index()
        {
            List<ProductGetVM> vms = _context.Products.Include(x => x.Category).Select(x => new ProductGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CategoryName = x.Category.Name,
                HoverImageUrl = x.HoverImageUrl,
                Price = x.Price,
                SKU = x.SKU,
                MainImageUrl = x.MainImageUrl,
                Rating = x.Rating,
                
            }).ToList();
            return View(vms);
        }
        [HttpGet]
        public IActionResult Create()
        {
            SendCategoriesWithViewBag();
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductCreateVM vm)
        {
            if (vm.Price < 0)
            {
                ModelState.AddModelError(nameof(vm.Price), "Price menfi ola bilmez");
            }

            if (!ModelState.IsValid)
            {
                SendCategoriesWithViewBag();
                return View(vm);
            }


            var isExistCategory = _context.Categories.Any(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                SendCategoriesWithViewBag();
                ModelState.AddModelError("", "Bu kateqoriya movcud deyil");
                return View(vm);
            }
            if (!vm.MainImage.CheckType("image"))
            {
                ModelState.AddModelError("MainImage", "Yalniz sekil formatinda data daxil ede bilersiz");
                return View(vm);
            }
            if (!vm.MainImage.CheckSize(2))
            {
                ModelState.AddModelError("MainImage", "Max 2 mb sekil yukleye bilersiz");
                return View(vm);
            }
            if (!vm.HoverImage.CheckType("image"))
            {
                ModelState.AddModelError("HoverImage", "Yalniz sekil formatinda data daxil ede bilersiz");
                return View(vm);
            }
            if (!vm.HoverImage.CheckSize(2))
            {
                ModelState.AddModelError("HoverImage", "Max 2 mb sekil yukleye bilersiz");
                return View(vm);
            }

            string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            string mainImaneUniqueName = vm.MainImage.SaveFile(folderPath);
            string hoverImaneUniqueName = vm.HoverImage.SaveFile(folderPath);
            Product product = new Product()
            {
                Name = vm.Name,
                Description = vm.Description,
                SKU = vm.SKU,
                CategoryId = vm.CategoryId,
                Price = vm.Price,
                MainImageUrl = mainImaneUniqueName,
                HoverImageUrl = hoverImaneUniqueName,
                Rating = vm.Rating,

            };
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
            ProductUpdateVM vm = new ProductUpdateVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Price = product.Price,
                SKU = product.SKU,
                Rating = product.Rating,

            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Update(ProductUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                SendCategoriesWithViewBag();
                return View(vm);
            }
            var isExistProduct = _context.Products.Find(vm.Id);

            if (isExistProduct == null)
            {
                return NotFound();
            }
            var isExistCategory = _context.Categories.Any(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                SendCategoriesWithViewBag();
                ModelState.AddModelError("CategoryId", "Bu kateqori mevcud deyil");
                return View(vm);
            }
            if (!vm.MainImage?.CheckType("image") ?? false)
            {
                ModelState.AddModelError("MainImage", "Yalniz sekil formatinda data daxil ede bilersiz");
                return View(vm);
            }
            if (!vm.MainImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("MainImage", "Max 2 mb sekil yukleye bilersiz");
                return View(vm);
            }
            if (!vm.HoverImage?.CheckType("image") ?? false)
            {
                ModelState.AddModelError("HoverImage", "Yalniz sekil formatinda data daxil ede bilersiz");
                return View(vm);
            }
            if (!vm.HoverImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("HoverImage", "Max 2 mb sekil yukleye bilersiz");
                return View(vm);
            }

            isExistProduct.Name = vm.Name;
            isExistProduct.Description = vm.Description;
            isExistProduct.SKU = vm.SKU;
            isExistProduct.CategoryId = vm.CategoryId;
            isExistProduct.Price = vm.Price;
            isExistProduct.Rating = vm.Rating;
            string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            if (vm.MainImage is { })
            {
                string newMainImageName = vm.MainImage.SaveFile(folderPath);
                if (System.IO.File.Exists(Path.Combine(folderPath, isExistProduct.MainImageUrl)))
                {
                    System.IO.File.Delete(Path.Combine(folderPath, isExistProduct.MainImageUrl));
                }
                isExistProduct.MainImageUrl = newMainImageName;
            }
            if (vm.HoverImage is { })
            {
                string newHoverImageName = vm.HoverImage.SaveFile(folderPath);
                if (System.IO.File.Exists(Path.Combine(folderPath, isExistProduct.HoverImageUrl)))
                {
                    System.IO.File.Delete(Path.Combine(folderPath, isExistProduct.HoverImageUrl));
                }
                isExistProduct.HoverImageUrl = newHoverImageName;


            }
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

            string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            if (System.IO.File.Exists(Path.Combine(folderPath, product.MainImageUrl)))
            {
                System.IO.File.Delete(Path.Combine(folderPath, product.MainImageUrl));
            }
            if (System.IO.File.Exists(Path.Combine(folderPath, product.HoverImageUrl)))
            {
                System.IO.File.Delete(Path.Combine(folderPath, product.HoverImageUrl));
            }

            return RedirectToAction(nameof(Index));
        }
        private void SendCategoriesWithViewBag()
        {
            var categories = _context.Categories.ToList();

            ViewBag.Categories = categories;
        }
    }
}
