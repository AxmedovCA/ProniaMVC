using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pronia.Contexts;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;

        public SliderController(AppDbContext context,IWebHostEnvironment enviroment)
        {
            _context = context; 
            _enviroment = enviroment;
        }

        public IActionResult Index()
        {
            var sliders = _context.Sliders.ToList();
            return View(sliders);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }



            if (slider.DiscountPercentage < 0 || slider.DiscountPercentage > 100)
            {
                ModelState.AddModelError("DiscountPercentage", "Endirim 0 100 araliqinda olmalidir");
                return View();
            }
            if (!slider.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Yalniz sekil formatinda data daxil ede bilersiz");
                return View(slider);
            }
            if(slider.Image.Length > 2 * Math.Pow(2, 20))
            {
                ModelState.AddModelError("Image", "Max 2 mb sekil yukleye bilersiz");
                return View(slider);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + slider.Image.FileName;
            //string folderPath = @$"{_enviroment.WebRootPath}\assets\images\website-images\{uniqueFileName}";
            string folderPath = Path.Combine(_enviroment.WebRootPath, "assets", "images", "website-images", uniqueFileName);
           using FileStream stream = new(folderPath,FileMode.Create);

            slider.Image.CopyTo(stream);
            slider.ImageUrl = uniqueFileName;




            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var slider = _context.Sliders.Find(id);
            if (slider == null)
            {
                return NotFound();
            }
            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            string folderPath = Path.Combine(_enviroment.WebRootPath, "assets", "images", "website-images", slider.ImageUrl);
            if(System.IO.File.Exists(folderPath))
                System.IO.File.Delete(folderPath);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var slider = _context.Sliders.Find(id);
            if (slider is null)
            {
                return NotFound();
            }
            return View(slider);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existSlider = _context.Sliders.Find(slider.Id);
            if (existSlider is null)
            {
                return NotFound();
            }
            existSlider.Title= slider.Title;
            existSlider.Description= slider.Description;
            existSlider.ImageUrl= slider.ImageUrl;
            existSlider.DiscountPercentage = slider.DiscountPercentage; 
            _context.Sliders.Update(existSlider);
            _context.SaveChanges(); 
            return RedirectToAction(nameof(Index));
        }

    }
}
