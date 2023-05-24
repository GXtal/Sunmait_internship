namespace Domain.Entities;

public class CategorySection
{
    public int CaregoryId { get; set; }

    public int SectionId { get; set; }

    public virtual Category Category { get; set; }

    public virtual Section Section { get; set; }
}
