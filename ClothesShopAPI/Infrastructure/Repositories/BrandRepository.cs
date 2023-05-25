using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
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

        public async Task<IEnumerable<Brand>> GetBrands()
        {
            var allBrands = await _dbContext.Brands.ToListAsync();
            return allBrands;
        }

        public async Task<bool> AddBrand(string newBrandName)
        {
            bool isPresent = await _dbContext.Brands.AnyAsync(b => b.Name == newBrandName);
            if (isPresent)
            {
                return false;
            }

            var brand = new Brand() { Name = newBrandName };
            _dbContext.Add(brand);
            await Save();

            return true;
        }

        public async Task<bool> UpdateBrand(int id, string newBrandName)
        {
            var isPresent = await _dbContext.Brands.AnyAsync(b => b.Name == newBrandName);
            if (isPresent)
            {
                return false;
            }

            var brand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (brand == null)
            {
                return false;
            }

            brand.Name = newBrandName;
            await Save();

            return true;
        }

        public async Task<bool> RemoveBrand(int id)
        {
            var brand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
            {
                return false;
            }

            var isUsed = await _dbContext.Products.AnyAsync(p => p.BrandId == brand.Id);
            if (isUsed)
            {
                return false;
            }

            _dbContext.Brands.Remove(brand);
            await Save();

            return true;
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
