using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Reflection;


namespace Pronia.Contexts
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
            base.OnModelCreating(builder);
        }
        public DbSet<Slider> Sliders { get; set; }

        public DbSet<Shipping> Shippings { get; set; }    
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ProductTag> ProductTags { get; set; }

        public DbSet<Brand> Brands { get; set; }
    }
}
