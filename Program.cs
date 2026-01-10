using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pronia.Abstraction;
using Pronia.Contexts;
using Pronia.Services;

namespace Pronia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric= true;
                opt.User.RequireUniqueEmail = true;

                opt.Lockout.MaxFailedAccessAttempts = 5;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );
            

            app.MapDefaultControllerRoute();

            

            app.Run();
        }
    }
}
