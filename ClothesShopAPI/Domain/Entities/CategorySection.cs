namespace Domain.Entities;

public class CategorySection
{
    public int CategoryId { get; set; }

    public int SectionId { get; set; }

    public Category Category { get; set; }

    public Section Section { get; set; }
}
