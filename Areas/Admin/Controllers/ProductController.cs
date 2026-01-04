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
            List<ProductGetVM> vms = _context.Products.Include(x => x.Category).Include(x=>x.Brand).Select(x => new ProductGetVM()
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
                BrandName = x.Brand.Name
            }).ToList();
            return View(vms);
        }
        [HttpGet]
        public IActionResult Create()
        {
            SendItemsWithViewBag();
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
                SendItemsWithViewBag();
                return View(vm);
            }


            var isExistCategory = _context.Categories.Any(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                SendItemsWithViewBag();
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

            foreach(var image in vm.Images)
            {
                if (!image.CheckType("image"))
                {
                    ModelState.AddModelError("Images", "Yalniz sekil formatinda data daxil ede bilersiz");
                    return View(vm);
                }
                if (!image.CheckSize(2))
                {
                    ModelState.AddModelError("Images", "Max 2 mb sekil yukleye bilersiz");
                    return View(vm);
                }
            }

            foreach(var tagId in vm.TagIds)
            {
                var isExist = _context.Tags.Any(x => x.Id == tagId);
                if (isExist is false)
                {
                    SendItemsWithViewBag();
                    ModelState.AddModelError("", "Bu tag movcud deyil");
                    return View(vm);    
                }

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
                ProductTags = [],
                ProductImages = [],
                BrandId = vm.BrandId,
            };

            foreach(var image in vm.Images)
            {
                string ImaneUniqueName = image.SaveFile(folderPath);
                ProductImage productImage = new()
                {
                    ImageUrl = ImaneUniqueName,
                    Product = product
                };
                product.ProductImages.Add(productImage);
            }

            foreach(var tagId in vm.TagIds)
            {
                ProductTag productTag = new()
                {
                    TagId = tagId,
                    Product = product
                };
                product.ProductTags.Add(productTag);
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _context.Products.Include(x=>x.ProductTags).Include(x=>x.ProductImages).FirstOrDefault(x=>x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            SendItemsWithViewBag();
            ProductUpdateVM vm = new ProductUpdateVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Price = product.Price,
                SKU = product.SKU,
                Rating = product.Rating,
                TagIds = product.ProductTags.Select(x=>x.TagId).ToList(),
                HoverImageUrl = product.HoverImageUrl,
                MainImageUrl = product.MainImageUrl,
                ImagesUrls = product.ProductImages.Select(x=>x.ImageUrl).ToList(),
                BrandId = product.BrandId,

            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Update(ProductUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                SendItemsWithViewBag();
                return View(vm);
            }
            var isExistProduct = _context.Products.Include(x=>x.ProductTags).FirstOrDefault(x=>x.Id ==vm.Id);

            if (isExistProduct == null)
            {
                return NotFound();
            }
            var isExistCategory = _context.Categories.Any(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                SendItemsWithViewBag();
                ModelState.AddModelError("CategoryId", "Bu kateqori mevcud deyil");
                return View(vm);
            }
            var isExistBrand = _context.Brands.Any(x=>x.Id == vm.BrandId);
            if (!isExistBrand)
            {
                SendItemsWithViewBag();
                ModelState.AddModelError("BrandId", "Bu Brand mevcud deyil");
                return View(vm);
            }
            foreach (var tagId in vm.TagIds)
            {
                var isExist = _context.Tags.Any(x => x.Id == tagId);
                if (isExist is false)
                {
                    SendItemsWithViewBag();
                    ModelState.AddModelError("", "Bu tag movcud deyil");
                    return View(vm);
                }

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
            isExistProduct.BrandId = vm.BrandId;
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
           
            isExistProduct.ProductTags = [];
            foreach (var tagId in vm.TagIds)
            {
                ProductTag productTag = new()
                {
                    TagId = tagId,
                    ProductId = isExistProduct.Id
                };
                isExistProduct.ProductTags.Add(productTag); 
            }
            _context.Products.Update(isExistProduct);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Include(x=>x.ProductImages).FirstOrDefault(x=>x.Id == id);
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
            foreach(var productImage in product.ProductImages)
            {
                if (System.IO.File.Exists(Path.Combine(folderPath, productImage.ImageUrl)))
                {
                    System.IO.File.Delete(Path.Combine(folderPath, productImage.ImageUrl));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            var products = _context.Products.Select(x=>new ProductGetVM()
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
                TagsName = x.ProductTags.Select(x=>x.Tag.Name).ToList(),
                ImageUrls = x.ProductImages.Select(x=>x.ImageUrl).ToList(),
                BrandName = x.Brand.Name,
                
			}).FirstOrDefault(x=>x.Id ==id); 
            if(products == null)
            {
                return NotFound();
            }
            return View(products);
        }
        private void SendItemsWithViewBag()
        {
            var categories = _context.Categories.ToList();

            ViewBag.Categories = categories;

            var tags = _context.Tags.ToList();

            ViewBag.Tags = tags;  
            
            var brands  = _context.Brands.ToList();
            ViewBag.Brands = brands;
            
        }

        
    }
}
