using Microsoft.AspNetCore.Mvc;
using Pronia.Contexts;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingController(AppDbContext _context) : Controller
	{

		
		public IActionResult Index()
		{
			var shippings = _context.Shippings.ToList();

			return View(shippings);
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult CreateShipping(Shipping shipping) {
			if (ModelState.IsValid == false)
			{
				return View();
			}
			_context.Shippings.Add(shipping);	
			_context.SaveChanges();
			return RedirectToAction("Index");

		}
		public IActionResult Delete(int id)
		{
			var shipping = _context.Shippings.Find(id);
			if (shipping == null)
			{
				return NotFound();
			}
			_context.Shippings.Remove(shipping);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

	}
}
