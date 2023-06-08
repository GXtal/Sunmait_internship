using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers;

[Route("api/Categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<IActionResult> GetTopLevelCategories()
    {
        var allCategories = await _categoryService.GetTopLevelCategories();

        var result = new List<CategoryViewModel>();
        foreach (var category in allCategories)
        {
            result.Add(new CategoryViewModel { Id = category.Id, Name = category.Name, Subcategories = new List<CategoryViewModel>() });
        }

        return new OkObjectResult(result);
    }

    // GET: api/Categories/Parent/5
    [HttpGet("Parent/{parentId}")]
    public async Task<IActionResult> GetCategoriesTreeByParent([FromRoute] int parentId)
    {
        var allCategories = await _categoryService.GetCategoriesTreeByParent(parentId);

        var result = FillRecursive(allCategories);

        return new OkObjectResult(result);
    }

    private IEnumerable<CategoryViewModel> FillRecursive(IEnumerable<Category> allCategories)
    {
        var result = new List<CategoryViewModel>();
        foreach (var category in allCategories)
        {
            var categoryView = new CategoryViewModel { Id = category.Id, Name = category.Name, Subcategories = new List<CategoryViewModel>() };

            categoryView.Subcategories = FillRecursive(category.ChildCategories);

            result.Add(categoryView);
        }
        return result;
    }

    // GET: api/Categories/Section/5
    [HttpGet("Section/{sectionId}")]
    public async Task<IActionResult> GetCategoriesBySection([FromRoute] int sectionId)
    {
        var allCategories = await _categoryService.GetCategoriesBySection(sectionId);

        var result = new List<CategoryViewModel>();
        foreach (var category in allCategories)
        {
            result.Add(new CategoryViewModel { Id = category.Id, Name = category.Name });
        }

        return new OkObjectResult(result);
    }

    // POST api/Categories
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] CategoryInputModel newCategory)
    {
        await _categoryService.AddCategory(newCategory.Name);
        return new OkResult();
    }

    // PUT api/Categories/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryInputModel newCategory)
    {
        await _categoryService.RenameCategory(id, newCategory.Name);
        return new OkResult();
    }

    // DELETE api/Categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveCategory([FromRoute] int id)
    {
        await _categoryService.RemoveCategory(id);
        return new OkResult();
    }

    // POST api/Categories/5/Parent/3
    [HttpPost("{id}/Parent/{parentId}")]
    public async Task<IActionResult> LinkCategoryToParent([FromRoute] int id, [FromRoute] int parentId)
    {
        await _categoryService.LinkCategoryToParent(id, parentId);
        return new OkResult();
    }

    // POST api/Categories/5/Section/3
    [HttpPost("{id}/Section/{sectionId}")]
    public async Task<IActionResult> LinkCategoryToSection([FromRoute] int id, [FromRoute] int sectionId)
    {
        await _categoryService.LinkCategoryToSection(id, sectionId);
        return new OkResult();
    }

    // DELETE api/Categories/5/Section/3
    [HttpDelete("{id}/Section/{sectionId}")]
    public async Task<IActionResult> UnlinkCategoryFromSection([FromRoute] int id, [FromRoute] int sectionId)
    {
        await _categoryService.UnlinkCategoryToSection(id, sectionId);
        return new OkResult();
    }
}
