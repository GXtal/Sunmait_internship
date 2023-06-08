using Application.Exceptions;
using Application.Exceptions.Messages;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderProductRepository _orderProductRepository;

    public ProductService(IBrandRepository brandRepository, IProductRepository productRepository,
        ICategoryRepository categoryRepository, IOrderRepository orderRepository, IOrderProductRepository orderProductRepository)
    {
        _brandRepository = brandRepository;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _orderRepository = orderRepository;
        _orderProductRepository = orderProductRepository;
    }

    public async Task AddProduct(string newProductName, string newProductDescription, decimal newProductPrice,
        int newProductQuantity, int brandId, int categoryId)
    {
        var brand = await _brandRepository.GetBrandById(brandId);
        if (brand == null)
        {
            throw new NotFoundException(String.Format(BrandExceptionsMessages.BrandNotFound, brandId));
        }

        var category = await _categoryRepository.GetCategoryById(categoryId);
        if (category == null)
        {
            throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, categoryId));
        }

        var product = new Product()
        {
            BrandId = brandId,
            CategoryId = categoryId,
            Name = newProductName,
            Description = newProductDescription,
            Price = newProductPrice,
            Quantity = newProductQuantity
        };
        await _productRepository.AddProduct(product);
    }

    public async Task<Product> GetProduct(int id)
    {
        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, id));
        }
        return product;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        var products = await _productRepository.GetProducts();
        return products;
    }

    public async Task<IEnumerable<Product>> GetProductsByBrand(int brandId)
    {
        var brand = await _brandRepository.GetBrandById(brandId);
        if (brand == null)
        {
            throw new NotFoundException(String.Format(BrandExceptionsMessages.BrandNotFound, brandId));
        }

        var products = await _productRepository.GetProductsByBrand(brand);
        return products;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId)
    {
        var category = await _categoryRepository.GetCategoryById(categoryId);
        if (category == null)
        {
            throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, categoryId));
        }

        var products = await _productRepository.GetProductsByCategory(category);
        return products;
    }

    public async Task UpdateProduct(int id, string newProductName, string newProductDescription, decimal newProductPrice,
        int newProductQuantity, int brandId, int categoryId)
    {
        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, id));
        }

        var brand = await _brandRepository.GetBrandById(brandId);
        if (brand == null)
        {
            throw new NotFoundException(String.Format(BrandExceptionsMessages.BrandNotFound, brandId));
        }

        var category = await _categoryRepository.GetCategoryById(categoryId);
        if (category == null)
        {
            throw new NotFoundException(String.Format(CategoryExceptionsMessages.CategoryNotFound, brandId));
        }

        product.Name = newProductName;
        product.Description = newProductDescription;
        product.Price = newProductPrice;
        product.Quantity = newProductQuantity;
        product.BrandId = brandId;
        product.CategoryId = categoryId;

        await _productRepository.UpdateProduct(product);
    }

    public async Task<IEnumerable<OrderProduct>> GetOrderProducts(int id)
    {
        var order = await _orderRepository.GetOrderById(id);
        if (order == null)
        {
            throw new NotFoundException(String.Format(OrderExceptionsMessages.OrderNotFound, id));
        }

        var orderProducts = await _orderProductRepository.GetOrderProductsByOrder(order);
        return orderProducts;
    }
}
