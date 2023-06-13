namespace Web.Models.ViewModels;

public class CategoryViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<CategoryViewModel> Subcategories { get; set; }
}
