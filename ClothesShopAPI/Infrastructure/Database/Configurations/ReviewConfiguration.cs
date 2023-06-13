using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Rating).IsRequired();

        builder.Property(e => e.Comment)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.UserId);
        builder.HasOne(e => e.User).WithMany(p => p.Reviews)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.ProductId).IsRequired();
        builder.HasOne(e => e.Product).WithMany(p => p.Reviews)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
