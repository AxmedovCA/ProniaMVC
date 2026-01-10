using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Abstraction;
using Pronia.Contexts;
using System.Security.Claims;
using System.Security.Claims;

namespace Pronia.Controllers
{
        [Authorize]
    public class BasketController(AppDbContext _context,IBasketService _baskerService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var basketItems = await _baskerService.GetBasketItemsAsync();
            return View(basketItems);
        }
        public async Task<IActionResult> AddToBasket(int productId)
        {
            var isExistProduct = await _context.Products.AnyAsync(x=>x.Id == productId);
            if (!isExistProduct)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isExistUser = await _context.Users.AnyAsync(x=>x.Id == userId);
            if (!isExistUser)
            {
                return BadRequest();
            }
           var isExistBasketItem = await _context.BasketItems.FirstOrDefaultAsync(x => x.AppUserId == userId && x.ProductId == productId);
            if(isExistBasketItem is { })
            {
                isExistBasketItem.Count++;
                _context.Update(isExistBasketItem);
            }
            else {
                BasketItem item = new()
                {
                    ProductId = productId,
                    Count = 1,
                    AppUserId = userId!
                };
                await _context.BasketItems.AddAsync(item);
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Product successfully added";
            //return RedirectToAction("Index","Shop");
            var basketItems = await _baskerService.GetBasketItemsAsync();
            return PartialView("_BasketPartialView", basketItems);
        }
        public async Task<IActionResult> RemoveFromBasket(int productId)
        {
            var isExistProduct = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!isExistProduct)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isExistUser = await _context.Users.AnyAsync(x => x.Id == userId);
            if (!isExistUser)
            {
                return BadRequest();
            }
            var existBasketItem = await _context.BasketItems.FirstOrDefaultAsync(x => x.AppUserId == userId && x.ProductId ==productId);
            if(existBasketItem is null)
            {
            return NotFound();
            }
            _context.BasketItems.Remove(existBasketItem);
            await _context.SaveChangesAsync();
            var returnUrl = Request.Headers["Referer"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Shop");
        }
        public async Task<IActionResult> DecreaseBasketItemCount(int productId)
        {
            var isExistProduct = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!isExistProduct)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isExistUser = await _context.Users.AnyAsync(x => x.Id == userId);
            if (!isExistUser)
            {
                return BadRequest();
            }
            var existBasketItem = await _context.BasketItems.FirstOrDefaultAsync(x => x.AppUserId == userId && x.ProductId == productId);
            if (existBasketItem is null)
            {
                return NotFound();
            }
            if (existBasketItem.Count > 1)
            {
                existBasketItem.Count--;
            }
            _context.BasketItems.Update(existBasketItem);
         await   _context.SaveChangesAsync();

            var basketItems = await _baskerService.GetBasketItemsAsync();
            return PartialView("_BasketPartialView",basketItems);
        }
    }
}
