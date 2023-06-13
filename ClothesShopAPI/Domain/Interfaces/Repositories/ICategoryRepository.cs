using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface ICategoryRepository
{
    public Task<Category> AddCategory(Category category);

    public Task<IEnumerable<Category>> GetCategoriesByParent(int parentId);

    public Task<IEnumerable<Category>> GetTopLevelCategories();

    public Task<IEnumerable<Category>> GetCategoriesBySection(int sectionId);

    public Task<Category> GetCategoryById(int id);

    public Task<Category> GetCategoryByName(string categoryName);

    public Task<IEnumerable<Category>> GetCategories();

    public Task RemoveCategory(Category category);

    public Task UpdateCategory(Category category);

    public Task Save();
}
