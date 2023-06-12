using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Application.Exceptions.Messages;
using Application.Exceptions;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly ICategorySectionRepository _categorySectionRepository;
    private readonly IProductRepository _productRepository;

    public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository,
        ISectionRepository sectionRepository, ICategorySectionRepository categorySectionRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _sectionRepository = sectionRepository;
        _categorySectionRepository = categorySectionRepository;
    }

    public async Task AddCategory(string newCategoryName, int? parentId, int sectionId)
    {
        var existingCategory = await _categoryRepository.GetCategoryByName(newCategoryName);
        if (existingCategory != null)
        {
            throw new BadRequestException(String.Format(CategoryExceptionsMessages.CategoryNameExists, newCategoryName));
        }

        if (parentId != null)
        {
            var parentCategory = await _categoryRepository.GetCategoryById((int)parentId);
            if (parentCategory == null)
            {
                throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, parentId));
            }
        }

        var section = await _sectionRepository.GetSectionById(sectionId);
        if (section == null)
        {
            throw new NotFoundException(String.Format(SectionExceptionsMessages.SectionNotFound, sectionId));
        }

        var category = new Category { Name = newCategoryName, ParentCategoryId = parentId };
        category = await _categoryRepository.AddCategory(category);

        var categorySection = new CategorySection() { CategoryId = category.Id, SectionId = sectionId };
        await _categorySectionRepository.AddCategorySection(categorySection);
    }

    public async Task<IEnumerable<Category>> GetTopLevelCategories()
    {
        var categories = await _categoryRepository.GetTopLevelCategories();
        return categories;
    }

    public async Task LinkCategoryToParent(int id, int? parentId)
    {
        var category = await _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, id));
        }

        if (parentId != null)
        {
            var parentCategory = await _categoryRepository.GetCategoryById((int)parentId);
            if (parentCategory == null)
            {
                throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, parentId));
            }
            category.ParentCategoryId = parentId;
        }
        else
        {
            category.ParentCategoryId = null;
        }

        await _categoryRepository.UpdateCategory(category);
    }

    public async Task LinkCategoryToSection(int id, int sectionId)
    {
        var category = await _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, id));
        }

        var section = await _sectionRepository.GetSectionById(sectionId);
        if (section == null)
        {
            throw new NotFoundException(String.Format(SectionExceptionsMessages.SectionNotFound, sectionId));
        }

        var existingCategorySection = await _categorySectionRepository.GetCategorySection(category, section);
        if (existingCategorySection != null)
        {
            throw new BadRequestException(String.Format(CategorySectionExceptionsMessages.CategorySectionExists, id, sectionId));
        }

        var categorySection = new CategorySection() { CategoryId = id, SectionId = sectionId };
        await _categorySectionRepository.AddCategorySection(categorySection);
    }

    public async Task UnlinkCategoryFromSection(int id, int sectionId)
    {
        var category = await _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, id));
        }

        var section = await _sectionRepository.GetSectionById(sectionId);
        if (section == null)
        {
            throw new NotFoundException(String.Format(SectionExceptionsMessages.SectionNotFound, sectionId));
        }

        var categorySection = await _categorySectionRepository.GetCategorySection(category, section);
        if (categorySection == null)
        {
            throw new NotFoundException(String.Format(CategorySectionExceptionsMessages.CategorySectionExists, id, sectionId));
        }

        await _categorySectionRepository.RemoveCategorySection(categorySection);
    }

    public async Task RemoveCategory(int id)
    {
        var category = await _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, id));
        }

        var children = await _categoryRepository.GetCategoriesByParent(id);
        if (children.Count() > 0)
        {
            throw new BadRequestException(String.Format(CategoryExceptionsMessages.CategoryHasChildren, id));
        }

        var categorySections = await _categorySectionRepository.GetCategorySectionsByCategory(category);
        if (categorySections.Count() > 0)
        {
            throw new BadRequestException(String.Format(CategoryExceptionsMessages.CategoryHasLinksToSections, id));
        }

        var products = await _productRepository.GetProductsByCategory(category);
        if (products.Count() > 0)
        {
            throw new BadRequestException(String.Format(CategoryExceptionsMessages.CategoryIsUsed, id));
        }

        await _categoryRepository.RemoveCategory(category);
    }

    public async Task RenameCategory(int id, string newCategoryName)
    {
        var category = await _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, id));
        }

        var existingCategory = await _categoryRepository.GetCategoryByName(newCategoryName);
        if (existingCategory != null)
        {
            throw new BadRequestException(String.Format(CategoryExceptionsMessages.CategoryNameExists, newCategoryName));
        }

        category.Name = newCategoryName;
        await _categoryRepository.UpdateCategory(category);
    }

    public async Task<IEnumerable<Category>> GetCategoriesBySection(int sectionId)
    {
        var section = await _sectionRepository.GetSectionById(sectionId);
        if (section == null)
        {
            throw new NotFoundException(String.Format(SectionExceptionsMessages.SectionNotFound, sectionId));
        }

        var categories = await _categoryRepository.GetCategoriesBySection(sectionId);
        return categories;
    }

    public async Task<IEnumerable<Category>> GetCategoriesTreeByParent(int parentId)
    {
        var parent = await _categoryRepository.GetCategoryById(parentId);
        if (parent == null)
        {
            throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, parentId));
        }

        var categories = await _categoryRepository.GetCategoriesByParent(parentId);
        return categories;
    }
}
