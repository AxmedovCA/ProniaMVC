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
		[AutoValidateAntiforgeryToken]
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
		[HttpGet]
		public IActionResult Update(int id)
		{
			var shipping = _context.Shippings.Find(id);
			if(shipping is null)
			{
				return NotFound();
			}
			return View(shipping);
		}
		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public IActionResult Update(Shipping shipping)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var isExist = _context.Shippings.Find(shipping.Id);
			isExist.Title = shipping.Title;
			isExist.Description = shipping.Description;
			isExist.ImageUrl = shipping.ImageUrl;

			_context.Shippings.Update(isExist);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
