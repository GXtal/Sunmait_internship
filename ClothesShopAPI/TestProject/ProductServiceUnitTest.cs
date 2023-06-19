using Application.Services;
using Bogus;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestProject;
public class ProductServiceUnitTest : BaseUnitTest
{
    [Fact]
    public async void GetProductsByBrand()
    {
        // Arrange
        var dbName = "GetProductsByBrand";
        using (var dbContext = GetInMemoryContext(dbName))
        {
            var products = AddProducts(dbContext, faker.Random.Int(1, 10));
            dbContext.SaveChanges();

            var productService = new ProductService(new BrandRepository(dbContext), new ProductRepository(dbContext),
            new CategoryRepository(dbContext), new OrderRepository(dbContext), new OrderProductRepository(dbContext));

            int brandId = 1;

            // Act        
            var brandProducts = await productService.GetProductsByBrand(brandId);

            // Assert 
            brandProducts.All(p => p.BrandId == brandId).Should().BeTrue();
            brandProducts.Count().Should().Be(products.Where(p => p.BrandId == brandId).Count());
        }
    }

    [Fact]
    public async void GetProductsByCategory()
    {
        // Arrange
        var dbName = "GetProductsByCategory";
        using (var dbContext = GetInMemoryContext(dbName))
        {
            var products = AddProducts(dbContext, faker.Random.Int(1, 10));
            dbContext.SaveChanges();

            var productService = new ProductService(new BrandRepository(dbContext), new ProductRepository(dbContext),
            new CategoryRepository(dbContext), new OrderRepository(dbContext), new OrderProductRepository(dbContext));

            int categoryId = 1;

            // Act        
            var categoryProducts = await productService.GetProductsByCategory(categoryId);

            // Assert 
            categoryProducts.All(p => p.CategoryId == categoryId).Should().BeTrue();
            categoryProducts.Count().Should().Be(products.Where(p => p.CategoryId == categoryId).Count());
        }
    }
}
