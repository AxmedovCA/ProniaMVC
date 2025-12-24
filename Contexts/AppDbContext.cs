using Microsoft.EntityFrameworkCore;


namespace Pronia.Contexts
{
    public class AppDbContext:DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Slider> Sliders { get; set; }

        public DbSet<Shipping> Shippings { get; set; }    
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
