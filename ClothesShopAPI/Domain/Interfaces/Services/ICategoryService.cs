using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface ICategoryService
{
    public Task<IEnumerable<Category>> GetTopLevelCategories();

    public Task AddCategory(string newCategoryName,int? parentId, int sectionId);

    public Task RenameCategory(int id, string newCategoryName);

    public Task LinkCategoryToParent(int id, int? parentId);

    public Task LinkCategoryToSection(int id, int sectionId);

    public Task UnlinkCategoryFromSection(int id, int sectionId);

    public Task RemoveCategory(int id);

    public Task<IEnumerable<Category>> GetCategoriesBySection(int sectionId);

    public Task<IEnumerable<Category>> GetCategoriesTreeByParent(int parentId);
}
