namespace Domain.Entities;

public class Section
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<CategorySection> CategoriesSections { get; set; }
}
