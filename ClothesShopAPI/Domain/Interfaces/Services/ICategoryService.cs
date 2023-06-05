using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface ICategoryService
{
    public Task<Category> GetCategory(int id);

    public Task<IEnumerable<Category>> GetCategories();

    public Task AddCategory(string newCategoryName);

    public Task RenameCategory(int id, string newCategoryName);

    public Task LinkCategoryToParent(int id, int parentId);

    public Task LinkCategoryToSection(int id, int sectionId);

    public Task UnlinkCategoryToSection(int id, int sectionId);

    public Task RemoveCategory(int id);
}
