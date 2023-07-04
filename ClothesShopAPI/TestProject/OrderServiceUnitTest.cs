using Application.Exceptions;
using Application.Exceptions.Messages;
using Application.Services;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Configuration.Internal;

namespace TestProject;

public class OrderServiceUnitTest : BaseUnitTest
{
    public OrderService GetOrderService(ShopDbContext dbContext)
    {
        return new OrderService(new UserRepository(dbContext),
                new OrderRepository(dbContext), new OrderHistoryRepository(dbContext),
                new ProductRepository(dbContext), new OrderProductRepository(dbContext),
                new StatusRepository(dbContext));
    }


    [Fact]
    public async void GetOrderById_OrderExists()
    {
        // Arrange
        int statusId = (int)OrderStatus.AwaitingConfirmation;

        var dbName = Guid.NewGuid().ToString();

        using (var dbContext = GetInMemoryContext(dbName))
        {
            var added = AddOrder(dbContext, statusId);
            dbContext.SaveChanges();

            var orderService = GetOrderService(dbContext);

            // Act
            var order = await orderService.GetOrder(added.Id);

            // Assert
            order.Should().NotBeNull();
            order.Id.Should().Be(added.Id);
            order.StatusId.Should().Be(added.StatusId);
            order.UserId.Should().Be(added.UserId);
            order.TotalCost.Should().Be(added.TotalCost);
        }
    }

    [Fact]
    public async void GetOrderById_OrderDoesNotExist()
    {
        // Arrange
        var dbName = Guid.NewGuid().ToString();

        using (var dbContext = GetInMemoryContext(dbName))
        {
            var orderService = GetOrderService(dbContext);

            // Act
            // Assert
            var wrongId = faker.Random.Int(1);
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => orderService.GetOrder(wrongId));
            ex.Message.Should().Match(String.Format(OrderExceptionsMessages.OrderNotFound, wrongId));
        }
    }

    [Fact]
    public async void ChangeOrderStatus_UnchangeableFromAwaitingConfirmationToCompleted()
    {
        // Arrange
        int oldStatusId = (int)OrderStatus.AwaitingConfirmation;
        int statusId = (int)OrderStatus.Completed;

        var dbName = Guid.NewGuid().ToString();

        using (var dbContext = GetInMemoryContext(dbName))
        {
            var added = AddOrder(dbContext, oldStatusId);
            AddStatuses(dbContext);
            dbContext.SaveChanges();

            var orderService = GetOrderService(dbContext);

            // Act
            // Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(async () => await orderService.AddOrderStatus(added.Id, statusId));
            ex.Message.Should().Match(String.Format(OrderExceptionsMessages.OrderUnchangeable, added.Id, statusId));
        }
    }

    [Fact]
    public async void ChangeOrderStatus_ChangeableFromAwaitingConfirmationToDelivering()
    {
        // Arrange
        int oldStatusId = (int)OrderStatus.AwaitingConfirmation;
        int statusId = (int)OrderStatus.Delivering;
        Order added;

        var dbName = Guid.NewGuid().ToString();

        using (var dbContext = GetInMemoryContext(dbName))
        {
            added = AddOrder(dbContext, oldStatusId);
            AddStatuses(dbContext);
            dbContext.SaveChanges();

            var orderService = GetOrderService(dbContext);

            // Act
            await orderService.AddOrderStatus(added.Id, statusId);
        }

        // Assert
        using (var dbContext = GetInMemoryContext(dbName))
        {
            var result = dbContext.OrderHistories.FirstOrDefault(oh => oh.StatusId == statusId && oh.OrderId == added.Id);
            result.Should().NotBeNull();
            result.StatusId.Should().Be(statusId);
        }
    }

    [Fact]
    public async void AddNewOrder_PossibleProducts()
    {
        // Arrange
        int count = faker.Random.Int(1, 10);
        int quantity = faker.Random.Int(count, 2 * count);
        List<OrderProduct> inputProducts;
        User user;

        var dbName = Guid.NewGuid().ToString();

        using (var dbContext = GetInMemoryContext(dbName))
        {
            user = AddUser(dbContext);
            AddStatuses(dbContext);
            var ids = AddProducts(dbContext, quantity);
            dbContext.SaveChanges();

            var inputFaker = new Faker<OrderProduct>()
                .RuleFor(op => op.ProductId, f => f.Random.ArrayElement(ids.ToArray()).Id)
                .RuleFor(op => op.Count, count);

            int inputProductsCount = faker.Random.Int(1, ids.Count());

            inputProducts = inputFaker.Generate(inputProductsCount);

            var orderService = GetOrderService(dbContext);

            // Act
            await orderService.AddOrder(user.Id, inputProducts);
        }

        // Assert
        using (var dbContext = GetInMemoryContext(dbName))
        {
            var result = dbContext.Orders.FirstOrDefault();
            result.Should().NotBeNull();
            result.StatusId.Should().Be((int)OrderStatus.AwaitingConfirmation);
            result.UserId.Should().Be(user.Id);

            var usedProductsIds = inputProducts.Select(ip => ip.ProductId).ToList();
            var usedProducts = dbContext.Products.Where(p => usedProductsIds.Contains(p.Id));

            usedProducts.All(up => up.AvailableQuantity == quantity - count).Should().BeTrue();

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
        int quantity = faker.Random.Int(1, 10);
        int count = faker.Random.Int(quantity + 1, 2 * quantity);
        List<OrderProduct> inputProducts;

        var dbName = Guid.NewGuid().ToString();

        using (var dbContext = GetInMemoryContext(dbName))
        {
            var user = AddUser(dbContext);
            AddStatuses(dbContext);
            var ids = AddProducts(dbContext, quantity);

            dbContext.SaveChanges();

            var inputFaker = new Faker<OrderProduct>()
               .RuleFor(op => op.ProductId, f => f.Random.ArrayElement(ids.ToArray()).Id)
               .RuleFor(op => op.Count, count);

            int inputProductsCount = faker.Random.Int(1, ids.Count());

            inputProducts = inputFaker.Generate(inputProductsCount);

            var orderService = GetOrderService(dbContext);

            // Act
            // Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(async () => await orderService.AddOrder(user.Id, inputProducts));
            ex.Message.Should().Match(String.Format(ProductExceptionsMessages.ProductNotEnoughQuantity, inputProducts.First().ProductId));
        }
    }
}