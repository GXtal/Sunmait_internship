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
    List<Product> products;
    List<Brand> brands;
    List<Category> categories;

    public ProductServiceUnitTest()
    {
        brands = new List<Brand>()
        {
            new Brand() { Id = 1, Name = faker.Company.CompanyName() },
            new Brand() { Id = 2, Name = faker.Company.CompanyName() },
            new Brand() { Id = 3, Name = faker.Company.CompanyName() },
        };

        categories = new List<Category>()
        {
            new Category() { Id = 1, Name = faker.Commerce.Department() },
            new Category() { Id = 2, Name = faker.Commerce.Department() },
            new Category() { Id = 3, Name = faker.Commerce.Department() },
        };

        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.BrandId, f => f.Random.ArrayElement(brands.ToArray()).Id)
            .RuleFor(p => p.CategoryId, f => f.Random.ArrayElement(categories.ToArray()).Id)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 100))
            .RuleFor(p => p.Description, f => f.Lorem.Sentence());

        products = productFaker.Generate(6);
    }

    [Fact]
    public async void GetProductsByBrand()
    {
        // Arrange
        var dbName = "GetProductsByBrand";
        using (var dbContext = GetInMemoryContext(dbName))
        {
            dbContext.AddRange(products);
            dbContext.AddRange(brands);
            dbContext.AddRange(categories);
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
            dbContext.AddRange(products);
            dbContext.AddRange(categories);
            dbContext.AddRange(brands);
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
