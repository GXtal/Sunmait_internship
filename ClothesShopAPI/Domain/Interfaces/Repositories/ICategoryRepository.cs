using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface ICategoryRepository
{
    public Task<Category> AddCategory(Category category);
    public Task<IEnumerable<Category>> GetCategoriesByParent(int parentId);
    public Task<Category> GetCategoryById(int id);
    public Task<Category> GetCategoryByName(string newCategoryName);
    public Task<IEnumerable<Category>> GetCategorys();
    public Task RemoveCategory(Category category);
    public Task UpdateCategory(Category category);
}
