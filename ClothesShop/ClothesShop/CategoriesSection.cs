namespace ClothesShop;

public class CategoriesSection
{
    public int CaregoryId { get; set; }

    public int SectionId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Section Section { get; set; } = null!;
}
