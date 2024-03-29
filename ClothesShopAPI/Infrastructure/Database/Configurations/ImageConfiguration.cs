﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;
public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Content)
            .IsRequired();

        builder.Property(e => e.ProductId).IsRequired();
        builder.HasOne(e => e.Product).WithMany(p => p.Images)
            .HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Cascade);
    }
}
