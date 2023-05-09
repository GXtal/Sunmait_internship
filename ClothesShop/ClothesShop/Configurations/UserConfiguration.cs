using ClothesShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesShop.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email)
            .HasMaxLength(45)
            .IsRequired();
        builder.HasIndex(e => e.Email).IsUnique();

        builder.Property(e => e.PasswordHash)
           .HasMaxLength(64)
           .IsRequired();

        builder.Property(e => e.RoleId).IsRequired();
        builder.HasOne(e => e.Role).WithMany(p => p.Users)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.Name)
            .HasMaxLength(45)
            .IsRequired();

        builder.Property(e => e.Surname)
            .HasMaxLength(45)
            .IsRequired();
    }
}
