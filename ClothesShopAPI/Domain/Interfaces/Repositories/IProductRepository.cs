using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IProductRepository
{
    public Task<IEnumerable<Product>> GetProductsByBrand(Brand brand);
    public Task<IEnumerable<Product>> GetProductsByCategory(Category category);
}
