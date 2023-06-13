using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface ICategorySectionRepository
{
    public Task<CategorySection> AddCategorySection(CategorySection categorySection);

    public Task<CategorySection> GetCategorySection(Category category, Section section);

    public Task<IEnumerable<CategorySection>> GetCategorySectionsByCategory(Category category);

    public Task<IEnumerable<CategorySection>> GetCategorySectionsBySection(Section section);

    public Task RemoveCategorySection(CategorySection categorySection);

    public Task Save();
}
