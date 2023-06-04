using Domain.Entities;

namespace Domain.Interfaces.Services;
public interface IProductService
{
    public Task<Product> GetProduct(int id);

    public Task<IEnumerable<Product>> GetProducts();

    public Task AddProduct(string newProductName, string newProductDescription, double newProductPrice, int newProductQuantity, int brandId, int categoryId);

    public Task UpdateProduct(int id, string newProductName, string newProductDescription, double newProductPrice, int newProductQuantity, int brandId, int categoryId);

    public Task RemoveProduct(int id);
}
