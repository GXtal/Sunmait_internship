﻿using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ShopDbContext _dbContext;

    public CategoryRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Category> AddCategory(Category category)
    {
        _dbContext.Add(category);
        await Save();
        return category;
    }

    public async Task<IEnumerable<Category>> GetCategoriesByParent(int parentId)
    {
        var parentIds = new List<int?>() { parentId };

        var categories = await GetCategoriesByParentRecursive(parentIds);

        return categories;
    }

    private async Task<IEnumerable<Category>> GetCategoriesByParentRecursive(List<int?> parentIds)
    {
        var categories = await _dbContext.Categories.
            Where(c => parentIds.Contains(c.ParentCategoryId)).
            ToListAsync();

        var newParentIds = new List<int?>();
        foreach (var category in categories)
        {
            category.ChildCategories = new List<Category>();
            newParentIds.Add(category.Id);
        }

        if (newParentIds.Count > 0)
        {
            var children = await GetCategoriesByParentRecursive(newParentIds);
        }

        return categories;
    }

    public async Task<IEnumerable<Category>> GetCategoriesBySection(int sectionId)
    {
        var categories = await _dbContext.CategoriesSections.
            Where(cs => cs.SectionId == sectionId).
            Select(c => c.Category).
            ToListAsync();
        return categories;
    }

    public async Task<Category> GetCategoryById(int id)
    {
        var category = await _dbContext.Categories.
            FirstOrDefaultAsync(c => c.Id == id);
        return category;
    }

    public async Task<Category> GetCategoryByName(string categoryName)
    {
        var category = await _dbContext.Categories.
            FirstOrDefaultAsync(c => c.Name == categoryName);
        return category;
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        var allCategories = await _dbContext.Categories.ToListAsync();
        return allCategories;
    }

    public async Task RemoveCategory(Category category)
    {
        _dbContext.Categories.Remove(category);
        await Save();
    }

    public async Task UpdateCategory(Category category)
    {
        _dbContext.Entry(category).State = EntityState.Modified;
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetTopLevelCategories()
    {
        var allCategories = await _dbContext.Categories.
            Where(c => c.ParentCategoryId == null).
            ToListAsync();
        return allCategories;
    }
}
