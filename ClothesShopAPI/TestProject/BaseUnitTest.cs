using Bogus;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestProject;
public class BaseUnitTest
{
    protected Faker faker = new Faker();
    public ShopDbContext GetInMemoryContext(string dbName)
    {
        var builder = new DbContextOptionsBuilder<ShopDbContext>();
        builder.UseInMemoryDatabase(databaseName: dbName);

        var dbContextOptions = builder.Options;
        var shopDbContext = new ShopDbContext(dbContextOptions);

        return shopDbContext;
    }

    public Order AddOrder(ShopDbContext dbContext, int statusId)
    {
        var order = new Order
        {
            Id = faker.Random.Int(1),
            StatusId = statusId,
            UserId = faker.Random.Int(1),
            TotalCost = faker.Random.Decimal(1, 1000)
        };
        dbContext.Add(order);
        return order;
    }

    public void AddStatuses(ShopDbContext dbContext)
    {
        dbContext.Add(new Status { Id = (int)OrderStatus.AwaitingConfirmation, Name = "Awaiting confirmation" });
        dbContext.Add(new Status { Id = (int)OrderStatus.Delivering, Name = "Delivering" });
        dbContext.Add(new Status { Id = (int)OrderStatus.Completed, Name = "Completed" });
    }

    public List<Product> AddProducts(ShopDbContext dbContext, int quantity)
    {
        List<Brand> brands = new List<Brand>()
        {
            new Brand() { Id = 1, Name = faker.Company.CompanyName() },
            new Brand() { Id = 2, Name = faker.Company.CompanyName() },
            new Brand() { Id = 3, Name = faker.Company.CompanyName() },
        };

        List<Category> categories = new List<Category>()
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
            .RuleFor(p => p.AvailableQuantity, f => quantity)
            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 100))
            .RuleFor(p => p.Description, f => f.Lorem.Sentence());

        List<Product> products = productFaker.Generate(faker.Random.Int(5, 20));

        dbContext.AddRange(products);
        dbContext.AddRange(brands);
        dbContext.AddRange(categories);

        return products;
    }

    public User AddUser(ShopDbContext dbContext)
    {
        int roleId = faker.Random.Int(1);
        var user = new User()
        {
            Id = faker.Random.Int(1),
            Email = faker.Internet.Email(),
            Name = faker.Name.FirstName(),
            Surname = faker.Name.LastName(),
            PasswordHash = faker.Internet.Password(),
            RoleId = roleId
        };
        dbContext.Add(user);
        dbContext.Add(new Role() { Id = roleId, Name = faker.Name.JobTitle() });

        return user;
    }
}
