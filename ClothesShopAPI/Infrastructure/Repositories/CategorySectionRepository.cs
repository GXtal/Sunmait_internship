using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategorySectionRepository : ICategorySectionRepository
{
    private readonly ShopDbContext _dbContext;

    public CategorySectionRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<CategorySection> AddCategorySection(CategorySection categorySection)
    {
        _dbContext.Add(categorySection);
        await Save();
        return categorySection;
    }

    public async Task<CategorySection> GetCategorySection(Category category, Section section)
    {
        var categorySection = await _dbContext.CategoriesSections.
            FirstOrDefaultAsync(cs => cs.SectionId == section.Id && cs.CategoryId == category.Id);
        return categorySection;
    }

    public async Task<IEnumerable<CategorySection>> GetCategorySectionsByCategory(Category category)
    {
        var categoriesSections = await _dbContext.CategoriesSections.Where(cs => cs.CategoryId == category.Id).ToListAsync();
        return categoriesSections;
    }

    public async Task<IEnumerable<CategorySection>> GetCategorySectionsBySection(Section section)
    {
        var categoriesSections = await _dbContext.CategoriesSections.Where(cs => cs.SectionId == section.Id).ToListAsync();
        return categoriesSections;
    }

    public async Task RemoveCategorySection(CategorySection categorySection)
    {
        _dbContext.CategoriesSections.Remove(categorySection);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
