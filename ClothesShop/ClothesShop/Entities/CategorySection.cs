namespace ClothesShop.Entities;

public class CategorySection
{
    public int CategoryId { get; set; }

    public int SectionId { get; set; }

    public virtual Category Category { get; set; }

    public virtual Section Section { get; set; }
}
