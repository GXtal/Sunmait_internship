using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly ShopDbContext _dbContext;

    public BrandRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Brand> GetBrandById(int id)
    {
        var brand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);
        return brand;
    }

    public async Task<Brand> GetBrandByName(string brandName)
    {
        var brand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Name == brandName);
        return brand;
    }

    public async Task<IEnumerable<Brand>> GetBrands()
    {
        var allBrands = await _dbContext.Brands.ToListAsync();
        return allBrands;
    }

    public async Task<Brand> AddBrand(Brand brand)
    {
        _dbContext.Add(brand);
        await Save();
        return brand;
    }

    public async Task UpdateBrand(Brand brand)
    {
        _dbContext.Entry(brand).State = EntityState.Modified;
        await Save();
    }

    public async Task RemoveBrand(Brand brand)
    {
        _dbContext.Brands.Remove(brand);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
