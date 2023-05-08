using ClothesShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesShop.Configurations;

public class OrderHistoryConfiguration : IEntityTypeConfiguration<OrderHistory>
{
    public void Configure(EntityTypeBuilder<OrderHistory> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.OrderId).IsRequired();
        builder.HasOne(e => e.Order).WithMany(p => p.OrderHistories)
            .HasForeignKey(e => e.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.StatusId).IsRequired();
        builder.HasOne(e => e.Status).WithMany(p => p.OrderHistories)
           .HasForeignKey(e => e.StatusId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.SetTime)
            .HasColumnType("timestamp without time zone")
            .IsRequired();
    }
}
