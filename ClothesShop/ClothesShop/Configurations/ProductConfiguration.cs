using ClothesShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesShop.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
           .HasMaxLength(45)
           .IsRequired();

        builder.Property(e => e.Price)
            .HasColumnType("money")
            .IsRequired();

        builder.Property(e => e.Description)
           .HasMaxLength(200)
           .IsRequired();

        builder.Property(e => e.CategoryId).IsRequired();
        builder.HasOne(e => e.Category).WithMany(p => p.Products)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.BrandId).IsRequired();
        builder.HasOne(e => e.Brand).WithMany(p => p.Products)
            .HasForeignKey(e => e.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
