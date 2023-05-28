using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IProductRepository
{
    public Task<Product> GetProductById(int id);

    public Task<IEnumerable<Product>> GetProducts();

    public Task<IEnumerable<Product>> GetProductsByBrand(Brand brand);

    public Task AddProduct(Product product);

    public Task UpdateProduct(Product product);

    public Task RemoveProduct(Product product);

    public Task Save();
}
