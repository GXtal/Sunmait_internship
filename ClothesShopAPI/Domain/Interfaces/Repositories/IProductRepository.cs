using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IProductRepository
{
    public Task<IEnumerable<Product>> GetProductsByBrand(Brand brand);
}
