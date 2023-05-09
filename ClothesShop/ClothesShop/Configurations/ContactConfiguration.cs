using ClothesShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesShop.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId).IsRequired();
        builder.HasOne(e => e.User).WithMany(p => p.Contacts)
            .HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();
    }
}
