using ClothesShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesShop.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId).IsRequired();
        builder.HasOne(e => e.User).WithMany(p => p.Addresses)
            .HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.FullAddress)
            .HasMaxLength(200)
            .IsRequired();
    }
}
