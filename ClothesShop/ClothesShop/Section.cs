namespace ClothesShop;

public class Section
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CategoriesSection> CategoriesSections { get; set; } = new List<CategoriesSection>();
}
