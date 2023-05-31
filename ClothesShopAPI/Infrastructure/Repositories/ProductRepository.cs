using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopDbContext _dbContext;

        public ProductRepository(ShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetProductsByBrand(Brand brand)
        {
            var products = await _dbContext.Products.Where(p => p.BrandId == brand.Id).ToListAsync();
            return products;
        }
    }
}
