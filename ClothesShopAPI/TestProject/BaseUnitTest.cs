using Bogus;
using Domain.Entities;
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
}
