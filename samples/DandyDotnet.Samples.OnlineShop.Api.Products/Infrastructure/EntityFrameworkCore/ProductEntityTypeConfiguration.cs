using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DandyDotnet.Samples.OnlineShop.Api.Products.Infrastructure.EntityFrameworkCore;

internal sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products", "master-data");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id, "id");
        builder.Property(p => p.Sku, "sku", 21);
        builder.Property(p => p.Name, "name", 128);
        builder.Property(p => p.Description, "description", 128, false);
        builder.Property(p => p.Price, "price");
        builder.CreatedAtProperty();
        builder.UpdatedAtProperty();
        builder.SoftDeletedAtProperties();
    }
}