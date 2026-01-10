using Microsoft.EntityFrameworkCore;
using Pronia.Abstraction;
using Pronia.Contexts;
using System.Security.Claims;

namespace Pronia.Services
{
    public class BasketService(IHttpContextAccessor _accessor, AppDbContext _context) : IBasketService
    {
       public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
         var userId =   _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isExistuser = await _context.Users.AnyAsync(x => x.Id == userId);
            if (!isExistuser) 
            {
                return [];
            }
            var basketItems = await _context.BasketItems.Include(x=>x.Product).Where(x=>x.AppUserId == userId).ToListAsync();
            return basketItems; 
        }
    }
}
