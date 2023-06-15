using Application.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace TestProject;
public class ProductServiceUnitTest
{
    Mock<IOrderRepository> _orderRepository;
    Mock<IBrandRepository> _brandRepository;
    Mock<IProductRepository> _productRepository;
    Mock<IOrderProductRepository> _orderProductRepository;
    Mock<ICategoryRepository> _categoryRepository;

    List<Product> products = new List<Product>()
    {
        new Product { Id = 1, BrandId = 1, CategoryId = 1 },
        new Product { Id = 2, BrandId = 1, CategoryId = 2 },
        new Product { Id = 3, BrandId = 1, CategoryId = 2 },
        new Product { Id = 4, BrandId = 1, CategoryId = 3 },
        new Product { Id = 5, BrandId = 2, CategoryId = 2 },
        new Product { Id = 6, BrandId = 3, CategoryId = 3 },
    };

    public ProductServiceUnitTest()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _orderProductRepository = new Mock<IOrderProductRepository>();
        _productRepository = new Mock<IProductRepository>();
        _categoryRepository = new Mock<ICategoryRepository>();
        _brandRepository = new Mock<IBrandRepository>();
    }


    [Fact]
    public async void GetProductsByBrand_Test()
    {
        // Arrange
        _productRepository.Setup(x => x.GetProductsByBrand(It.IsAny<Brand>())).ReturnsAsync((Brand brand) =>
        {
            return products.Where(p => p.BrandId == brand.Id).ToList();
        });
        _brandRepository.Setup(x => x.GetBrandById(It.IsAny<int>())).ReturnsAsync((int id) => new Brand { Id = id });
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
    public async void GetProductsByCategory_Test()
    {
        // Arrange
        _productRepository.Setup(x => x.GetProductsByCategory(It.IsAny<Category>())).ReturnsAsync((Category category) =>
        {
            return products.Where(p => p.CategoryId == category.Id).ToList();
        });
        _categoryRepository.Setup(x => x.GetCategoryById(It.IsAny<int>())).ReturnsAsync((int id) => new Category { Id = id });
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
