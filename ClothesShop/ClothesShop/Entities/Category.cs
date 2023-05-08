﻿namespace ClothesShop.Entities;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ParentCategoryId { get; set; }

    public virtual ICollection<Category> ChildCategories { get; set; }

    public virtual Category ParentCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; }

    public virtual ICollection<CategorySection> CategoriesSections { get; set; }
}
