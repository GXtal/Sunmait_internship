namespace ClothesShop.Entities;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ParentCategoryId { get; set; }

    public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();

    public virtual Category ParentCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<CategorySection> CategoriesSections { get; set; } = new List<CategorySection>();
}
