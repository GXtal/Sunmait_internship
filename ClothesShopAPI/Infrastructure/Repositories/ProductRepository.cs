using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShopDbContext _dbContext;

    public ProductRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> AddProduct(Product product)
    {
        _dbContext.Add(product);
        await Save();
        return product;
    }

    public async Task<Product> GetProductById(int id)
    {
        var product = await _dbContext.Products.
            Include(p => p.Brand).
            Include(p => p.Category).
            FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        var allProducts = await _dbContext.Products.
            Include(p => p.Brand).
            Include(p => p.Category).
            ToListAsync();
        return allProducts;
    }

    public async Task<IEnumerable<Product>> GetProductsByBrand(Brand brand)
    {
        var products = await _dbContext.Products.
            Include(p => p.Brand).
            Include(p => p.Category).
            Where(p => p.BrandId == brand.Id).
            ToListAsync();
        return products;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(Category category)
    {
        var products = await _dbContext.Products.
            Include(p => p.Brand).
            Include(p => p.Category).
            Where(p => p.CategoryId == category.Id).
            ToListAsync();
        return products;
    }

    public async Task UpdateProduct(Product product)
    {
        _dbContext.Entry(product).State = EntityState.Modified;
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsBySection(Section section)
    {
        var products = await _dbContext.Products.
            Include(p => p.Brand).
            Include(p => p.Category).
            ThenInclude(c => c.CategoriesSections).
            Where(p => p.Category.CategoriesSections.Any(cs => cs.SectionId == section.Id)).
            ToListAsync();
        return products;
    }
}
