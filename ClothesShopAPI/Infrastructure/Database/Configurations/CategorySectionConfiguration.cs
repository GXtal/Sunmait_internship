using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class CategorySectionConfiguration : IEntityTypeConfiguration<CategorySection>
{
    public void Configure(EntityTypeBuilder<CategorySection> builder)
    {
        builder.HasKey(e => new { e.CaregoryId, e.SectionId });

        builder.HasOne(e => e.Section).WithMany(s => s.CategoriesSections)
            .HasForeignKey(e => e.SectionId).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Category).WithMany(s => s.CategoriesSections)
            .HasForeignKey(e => e.CaregoryId).OnDelete(DeleteBehavior.Cascade);
    }
}
