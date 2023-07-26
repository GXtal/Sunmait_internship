using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class ReservedProductConfiguration : IEntityTypeConfiguration<ReservedProduct>
{
    public void Configure(EntityTypeBuilder<ReservedProduct> builder)
    {
        builder.HasKey(e => new { e.UserId, e.ProductId });

        builder.Property(e => e.Count).IsRequired();

        builder.HasOne(e => e.User).WithMany(p => p.ReservedProducts)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Product).WithMany(p => p.ReservedProducts)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.ExpirationTime)
            .HasColumnType("timestamp without time zone")
            .IsRequired();
    }
}
