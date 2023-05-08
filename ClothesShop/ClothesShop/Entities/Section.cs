namespace ClothesShop.Entities;

public class Section
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<CategorySection> CategoriesSections { get; set; } = new List<CategorySection>();
}
