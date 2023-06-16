using Application.Exceptions;
using Application.Exceptions.Messages;
using Application.Services;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Configuration.Internal;

namespace TestProject;

public class OrderServiceUnitTest : BaseUnitTest
{
    [Fact]
    public async void GetOrderById_OrderExists()
    {
        // Arrange
        int workingId = faker.Random.Int(1);
        int statusId = faker.Random.Int(1);
        int userId = faker.Random.Int(1);
        decimal cost = faker.Random.Decimal(1, 1000);
        var dbName = "OrderExists";

        using (var dbContext = GetInMemoryContext(dbName))
        {
            dbContext.Add(new Order { Id = workingId, StatusId = statusId, UserId = userId, TotalCost = cost });
            dbContext.SaveChanges();

            var orderService = new OrderService(new UserRepository(dbContext),
                new OrderRepository(dbContext), new OrderHistoryRepository(dbContext),
                new ProductRepository(dbContext), new OrderProductRepository(dbContext),
                new StatusRepository(dbContext));

            // Act
            var order = await orderService.GetOrder(workingId);

            // Assert
            order.Should().NotBeNull();
            order.Id.Should().Be(workingId);
            order.StatusId.Should().Be(statusId);
            order.UserId.Should().Be(userId);
            order.TotalCost.Should().Be(cost);
        }
    }

    [Fact]
    public async void GetOrderById_OrderDoesNotExist()
    {
        // Arrange
        int workingId = faker.Random.Int(1);
        int wrongId = faker.Random.Int(1);
        var dbName = "OrderDoesNotExist";

        using (var dbContext = GetInMemoryContext(dbName))
        {
            dbContext.Add(new Order
            {
                Id = workingId,
                StatusId = faker.Random.Int(1),
                UserId = faker.Random.Int(1),
                TotalCost = faker.Random.Decimal(1, 1000)
            });
            dbContext.SaveChanges();

            var orderService = new OrderService(new UserRepository(dbContext),
                new OrderRepository(dbContext), new OrderHistoryRepository(dbContext),
                new ProductRepository(dbContext), new OrderProductRepository(dbContext),
                new StatusRepository(dbContext));

            // Act
            // Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => orderService.GetOrder(wrongId));
            ex.Message.Should().Match(String.Format(OrderExceptionsMessages.OrderNotFound, wrongId));
        }
    }

    [Fact]
    public async void ChangeOrderStatus_UnchangeableFromAwaitingConfirmationToCompleted()
    {
        // Arrange
        int orderId = faker.Random.Int(1);
        int oldStatusId = (int)OrderStatus.AwaitingConfirmation;
        int statusId = (int)OrderStatus.Completed;
        var dbName = "Unchangeable";

        using (var dbContext = GetInMemoryContext(dbName))
        {
            dbContext.Add(new Order
            {
                Id = orderId,
                StatusId = oldStatusId,
                UserId = faker.Random.Int(1),
                TotalCost = faker.Random.Decimal(1, 1000)
            });
            dbContext.Add(new Status { Id = oldStatusId, Name = "Awaiting confirmation" });
            dbContext.Add(new Status { Id = statusId, Name = "Completed" });
            dbContext.SaveChanges();

            var orderService = new OrderService(new UserRepository(dbContext),
                new OrderRepository(dbContext), new OrderHistoryRepository(dbContext),
                new ProductRepository(dbContext), new OrderProductRepository(dbContext),
                new StatusRepository(dbContext));

            // Act
            // Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(async () => await orderService.AddOrderStatus(orderId, statusId));
            ex.Message.Should().Match(String.Format(OrderExceptionsMessages.OrderUnchangeable, orderId, statusId));
        }
    }

    [Fact]
    public async void ChangeOrderStatus_ChangeableFromAwaitingConfirmationToDelivering()
    {
        // Arrange
        int orderId = faker.Random.Int(1);
        int oldStatusId = (int)OrderStatus.AwaitingConfirmation;
        int statusId = (int)OrderStatus.Delivering;
        var dbName = "Changeable";

        using (var dbContext = GetInMemoryContext(dbName))
        {
            dbContext.Add(new Order
            {
                Id = orderId,
                StatusId = oldStatusId,
                UserId = faker.Random.Int(1),
                TotalCost = faker.Random.Decimal(1, 1000)
            });
            dbContext.Add(new Status { Id = oldStatusId, Name = "Awaiting confirmation" });
            dbContext.Add(new Status { Id = statusId, Name = "Completed" });
            dbContext.SaveChanges();

            var orderService = new OrderService(new UserRepository(dbContext),
                new OrderRepository(dbContext), new OrderHistoryRepository(dbContext),
                new ProductRepository(dbContext), new OrderProductRepository(dbContext),
                new StatusRepository(dbContext));

            // Act
            await orderService.AddOrderStatus(orderId, statusId);
        }

        // Assert
        using (var dbContext = GetInMemoryContext(dbName))
        {
            var result = dbContext.OrderHistories.FirstOrDefault(oh => oh.StatusId == statusId && oh.OrderId == orderId);
            result.Should().NotBeNull();
            result.StatusId.Should().Be(statusId);
        }
    }

    [Fact]
    public async void AddNewOrder_PossibleProducts()
    {
        // Arrange
        int userId = faker.Random.Int(1);
        int roleId = faker.Random.Int(1);
        int count = faker.Random.Int(1, 10);
        int quantity = faker.Random.Int(count, 2 * count);
        var dbName = "PossibleProducts";

        int brandId = faker.Random.Int(1);
        int categoryId = faker.Random.Int(1);

        int allProductsCount = faker.Random.Int(1, 10);
        int inputProductsCount = faker.Random.Int(1, allProductsCount);

        List<OrderProduct> inputProducts;

        using (var dbContext = GetInMemoryContext(dbName))
        {
            dbContext.Add(new User()
            {
                Id = userId,
                Email = faker.Internet.Email(),
                Name = faker.Name.FirstName(),
                Surname = faker.Name.LastName(),
                PasswordHash = faker.Internet.Password(),
                RoleId = roleId
            });
            dbContext.Add(new Role() { Id = roleId, Name = faker.Name.JobTitle() });
            dbContext.Add(new Status() { Id = (int)OrderStatus.AwaitingConfirmation, Name = "Awaiting confirmation" });

            var productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.BrandId, brandId)
            .RuleFor(p => p.CategoryId, categoryId)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Quantity, quantity)
            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 100))
            .RuleFor(p => p.Description, f => f.Lorem.Sentence());
            var products = productFaker.Generate(allProductsCount);
            dbContext.AddRange(products);

            dbContext.Add(new Category() { Id = categoryId, Name = faker.Commerce.Department() });
            dbContext.Add(new Brand() { Id = brandId, Name = faker.Company.CompanyName() });
            dbContext.SaveChanges();

            var inputFaker = new Faker<OrderProduct>()
                .RuleFor(op => op.ProductId, f => f.Random.ArrayElement(products.ToArray()).Id)
                .RuleFor(op => op.Count, count);

            inputProducts = inputFaker.Generate(inputProductsCount);

            var orderService = new OrderService(new UserRepository(dbContext),
                    new OrderRepository(dbContext), new OrderHistoryRepository(dbContext),
                    new ProductRepository(dbContext), new OrderProductRepository(dbContext),
                    new StatusRepository(dbContext));

            // Act
            await orderService.AddOrder(userId, inputProducts);
        }

        // Assert
        using (var dbContext = GetInMemoryContext(dbName))
        {
            var result = dbContext.Orders.FirstOrDefault();
            result.Should().NotBeNull();
            result.StatusId.Should().Be((int)OrderStatus.AwaitingConfirmation);
            result.UserId.Should().Be(userId);

            var usedProductsIds = inputProducts.Select(ip => ip.ProductId).ToList();
            var usedProducts = dbContext.Products.Where(p => usedProductsIds.Contains(p.Id));

            usedProducts.All(up => up.Quantity == quantity - count).Should().BeTrue();

            decimal cost = 0;
            foreach (var product in usedProducts)
            {
                cost += product.Price * count;
            }

            result.TotalCost.Should().Be(cost);
        }
    }

    [Fact]
    public async void AddNewOrder_ImpossibleProductCount()
    {
        // Arrange
        int userId = faker.Random.Int(1);
        int roleId = faker.Random.Int(1);
        int quantity = faker.Random.Int(1, 10);
        int count = faker.Random.Int(quantity, 2 * quantity);
        var dbName = "ImpossibleProducts";

        int brandId = faker.Random.Int(1);
        int categoryId = faker.Random.Int(1);

        int allProductsCount = faker.Random.Int(1, 10);
        int inputProductsCount = faker.Random.Int(1, allProductsCount);

        List<OrderProduct> inputProducts;

        using (var dbContext = GetInMemoryContext(dbName))
        {
            dbContext.Add(new User()
            {
                Id = userId,
                Email = faker.Internet.Email(),
                Name = faker.Name.FirstName(),
                Surname = faker.Name.LastName(),
                PasswordHash = faker.Internet.Password(),
                RoleId = roleId
            });
            dbContext.Add(new Role() { Id = roleId, Name = faker.Name.JobTitle() });
            dbContext.Add(new Status() { Id = (int)OrderStatus.AwaitingConfirmation, Name = "Awaiting confirmation" });

            var productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.BrandId, brandId)
            .RuleFor(p => p.CategoryId, categoryId)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Quantity, quantity)
            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 100))
            .RuleFor(p => p.Description, f => f.Lorem.Sentence());
            var products = productFaker.Generate(allProductsCount);
            dbContext.AddRange(products);

            dbContext.Add(new Category() { Id = categoryId, Name = faker.Commerce.Department() });
            dbContext.Add(new Brand() { Id = brandId, Name = faker.Company.CompanyName() });
            dbContext.SaveChanges();

            var inputFaker = new Faker<OrderProduct>()
                .RuleFor(op => op.ProductId, f => f.Random.ArrayElement(products.ToArray()).Id)
                .RuleFor(op => op.Count, count);
            inputProducts = inputFaker.Generate(inputProductsCount);

            var orderService = new OrderService(new UserRepository(dbContext),
                    new OrderRepository(dbContext), new OrderHistoryRepository(dbContext),
                    new ProductRepository(dbContext), new OrderProductRepository(dbContext),
                    new StatusRepository(dbContext));

            // Act
            // Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(async () => await orderService.AddOrder(userId, inputProducts));
            ex.Message.Should().Match(String.Format(ProductExceptionsMessages.ProductNotEnoughQuantity, inputProducts.First().ProductId));
        }
    }
}