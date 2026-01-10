using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pronia.Configurations
{
    public class BasketConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {

            builder.HasOne(x => x.Product).WithMany(p => p.BasketItems).HasForeignKey(x => x.ProductId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.AppUser).WithMany(x => x.BasketItems).HasForeignKey(x => x.AppUserId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);
            builder.ToTable(opt =>
            {
                opt.HasCheckConstraint("CK_BasketItems_Count", "[Count]>0");
            });
        }
    }
}
