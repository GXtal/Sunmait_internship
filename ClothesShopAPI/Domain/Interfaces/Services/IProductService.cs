using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IProductService
{
    public Task<Product> GetProduct(int id);

    public Task<IEnumerable<Product>> GetProducts();

    public Task<IEnumerable<Product>> GetProductsByBrand(int brandId);

    public Task<IEnumerable<Product>> GetProductsByCategory(int categoryId);

    public Task AddProduct(string newProductName, string newProductDescription, decimal newProductPrice,
        int newProductQuantity, int brandId, int categoryId);

    public Task UpdateProduct(int id, string newProductName, string newProductDescription, decimal newProductPrice,
        int newProductQuantity, int brandId, int categoryId);
    public Task<IEnumerable<OrderProduct>> GetOrderProducts(int orderId);
}
