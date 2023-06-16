using Application.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace TestProject;
public class ProductServiceUnitTest : BaseUnitTest
{
    List<Product> products = new List<Product>()
    {
        new Product { Id = 1, BrandId = 1, CategoryId = 1, Name = "a", Quantity = 1, Price = 10, Description ="" },
        new Product { Id = 2, BrandId = 1, CategoryId = 2, Name = "a", Quantity = 1, Price = 10, Description ="" },
        new Product { Id = 3, BrandId = 1, CategoryId = 2, Name = "a", Quantity = 1, Price = 10, Description ="" },
        new Product { Id = 4, BrandId = 1, CategoryId = 3, Name = "a", Quantity = 1, Price = 10, Description ="" },
        new Product { Id = 5, BrandId = 2, CategoryId = 2, Name = "a", Quantity = 1, Price = 10, Description ="" },
        new Product { Id = 6, BrandId = 3, CategoryId = 3, Name = "a", Quantity = 1, Price = 10, Description ="" },
    };

    List<Brand> brands = new List<Brand>()
    {
        new Brand() { Id = 1, Name = "a" },
        new Brand() { Id = 2, Name = "b" },
        new Brand() { Id = 3, Name = "c" },
    };

    List<Category> categories = new List<Category>()
    {
        new Category() { Id = 1, Name = "a" },
        new Category() { Id = 2, Name = "b" },
        new Category() { Id = 3, Name = "c" },
    };

    [Fact]
    public async void GetProductsByBrand()
    {
        // Arrange
        _shopDbContext.AddRange(products);
        _shopDbContext.AddRange(brands);
        _shopDbContext.SaveChanges();

        SetupProductRepository();
        SetupBrandRepository();

        var productService = new ProductService(_brandRepository.Object, _productRepository.Object,
            _categoryRepository.Object, _orderRepository.Object, _orderProductRepository.Object);

        int brandId = 1;

        // Act        
        var brandProducts = await productService.GetProductsByBrand(brandId);

        // Assert 
        brandProducts.All(p => p.BrandId == brandId).Should().BeTrue();
        brandProducts.Count().Should().Be(products.Where(p => p.BrandId == brandId).Count());
    }

    [Fact]
    public async void GetProductsByCategory()
    {
        // Arrange
        _shopDbContext.AddRange(products);
        _shopDbContext.AddRange(categories);
        _shopDbContext.SaveChanges();

        SetupProductRepository();
        SetupCategoryRepository();

        var productService = new ProductService(_brandRepository.Object, _productRepository.Object,
            _categoryRepository.Object, _orderRepository.Object, _orderProductRepository.Object);

        int categoryId = 1;

        // Act        
        var categoryProducts = await productService.GetProductsByCategory(categoryId);

        // Assert 

        categoryProducts.All(p => p.CategoryId == categoryId).Should().BeTrue();
        categoryProducts.Count().Should().Be(products.Where(p => p.CategoryId == categoryId).Count());
    }
}
