using Microsoft.AspNetCore.Mvc;
using Pronia.Contexts;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var sliders = _context.Sliders.ToList();
            var shippings = _context.Shippings.ToList();
            HomeViewModel model = new()
            {
                Sliders = sliders,
                Shippings = shippings

            };
      
            return View(model);
        }
        public IActionResult CreateSlider()
        {
            return View();
        }

        public IActionResult CreateNewSlider(Slider slider)
        {
            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
