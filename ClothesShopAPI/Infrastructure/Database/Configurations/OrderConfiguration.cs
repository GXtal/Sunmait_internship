using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId).IsRequired();
        builder.HasOne(e => e.User).WithMany(p => p.Orders)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.TotalCost)
            .HasColumnType("money")
            .IsRequired();
    }
}
