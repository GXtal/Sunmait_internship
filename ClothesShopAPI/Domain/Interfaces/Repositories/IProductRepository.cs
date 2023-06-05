using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IProductRepository
{
    public Task<Product> AddProduct(Product product);
    public Task<Product> GetProductById(int id);
    public Task<IEnumerable<Product>> GetProducts();
    public Task<IEnumerable<Product>> GetProductsByBrand(Brand brand);
    public Task<IEnumerable<Product>> GetProductsByCategory(Category category);
    public Task UpdateProduct(Product product);
}
