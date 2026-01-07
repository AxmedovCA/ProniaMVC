using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pronia.Configurations
{
    public class ShippingConfiguration : IEntityTypeConfiguration<Shipping>
    {
        public void Configure(EntityTypeBuilder<Shipping> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(256);
            builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(512);
        }
    }
}
