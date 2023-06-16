using Application.Exceptions;
using Application.Exceptions.Messages;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace TestProject;

public class OrderServiceUnitTest : BaseUnitTest
{
    [Fact]
    public async void GetOrderById_OrderExists()
    {
        // Arrange
        int workingId = 1;

        _shopDbContext.Add(new Order { Id = workingId, StatusId = 1, UserId = 1, TotalCost = 0 });
        _shopDbContext.SaveChanges();

        SetupOrderRepository();

        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        var order = await orderService.GetOrder(workingId);

        // Assert
        order.Should().NotBeNull();
        order.Id.Should().Be(workingId);
    }

    [Fact]
    public async void GetOrderById_OrderDoesNotExist()
    {
        // Arrange
        int workingId = 1;
        int wrongId = 2;

        _shopDbContext.Add(new Order { Id = workingId, StatusId = 1, UserId = 1, TotalCost = 0 });
        _shopDbContext.SaveChanges();

        SetupOrderRepository();

        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        // Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => orderService.GetOrder(wrongId));
        ex.Message.Should().Match(String.Format(OrderExceptionsMessages.OrderNotFound, wrongId));
    }

    [Fact]
    public async void ChangeOrderStatus_UnchangeableFromAwaitingConfirmationToCompleted()
    {
        // Arrange
        int orderId = 1;
        int oldStatusId = (int)OrderStatus.AwaitingConfirmation;
        int statusId = (int)OrderStatus.Completed;

        _shopDbContext.Add(new Order { Id = orderId, StatusId = oldStatusId, UserId = 1, TotalCost = 0 });
        _shopDbContext.Add(new Status { Id = oldStatusId, Name = "Awaiting confirmation" });
        _shopDbContext.Add(new Status { Id = statusId, Name = "Completed" });
        _shopDbContext.SaveChanges();

        SetupOrderRepository();
        SetupStatusRepository();
        SetupOrderHistoryRepository();

        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        // Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(async () => await orderService.AddOrderStatus(orderId, statusId));
        ex.Message.Should().Match(String.Format(OrderExceptionsMessages.OrderUnchangeable, orderId, statusId));
    }

    [Fact]
    public async void ChangeOrderStatus_ChangeableFromAwaitingConfirmationToDelivering()
    {
        // Arrange
        int orderId = 1;
        int oldStatusId = (int)OrderStatus.AwaitingConfirmation;
        int statusId = (int)OrderStatus.Delivering;

        _shopDbContext.Add(new Order { Id = orderId, StatusId = oldStatusId, UserId = 1, TotalCost = 0 });
        _shopDbContext.Add(new Status { Id = oldStatusId, Name = "Awaiting confirmation" });
        _shopDbContext.Add(new Status { Id = statusId, Name = "Completed" });
        _shopDbContext.SaveChanges();

        SetupOrderRepository();
        SetupStatusRepository();
        SetupOrderHistoryRepository();

        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        await orderService.AddOrderStatus(orderId, statusId);

        // Assert
        var result = _shopDbContext.OrderHistories.FirstOrDefault(oh => oh.StatusId == statusId && oh.OrderId == orderId);
        result.Should().NotBeNull();
        result.StatusId.Should().Be(statusId);
    }

    [Fact]
    public async void AddNewOrder_PossibleProducts()
    {
        // Arrange
        int userId = 1;
        int count = 10;
        int quantity = 20;

        _shopDbContext.Add(new User() { Id = userId, Email = "", Name = "", Surname = "", PasswordHash = "", RoleId = 1 });
        _shopDbContext.Add(new Status() { Id = (int)OrderStatus.AwaitingConfirmation, Name = "Awaiting confirmation" });
        _shopDbContext.Add(new Product() { Id = 1, BrandId = 1, CategoryId = 1, Name = "a", Quantity = quantity, Price = 10, Description = "" });
        _shopDbContext.Add(new Product() { Id = 2, BrandId = 1, CategoryId = 1, Name = "b", Quantity = quantity, Price = 5, Description = "" });
        _shopDbContext.SaveChanges();

        SetupOrderRepository();
        SetupProductRepository();
        SetupUserRepository();
        SetupOrderHistoryRepository();
        SetupOrderProductRepository();

        var inputProducts = new List<OrderProduct>() { new OrderProduct() { ProductId = 1, Count = count }, new OrderProduct() { ProductId = 2, Count = count } };

        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        await orderService.AddOrder(userId, inputProducts);

        // Assert
        var result = _shopDbContext.Orders.FirstOrDefault();
        result.Should().NotBeNull();
        result.StatusId.Should().Be((int)OrderStatus.AwaitingConfirmation);
        result.UserId.Should().Be(userId);
        result.TotalCost.Should().Be(150);
    }

    [Fact]
    public async void AddNewOrder_ImpossibleProductCount()
    {
        // Arrange
        int userId = 1;
        int count = 10;
        int quantity = 5;
        int productId = 1;

        _shopDbContext.Add(new User() { Id = userId, Email = "", Name = "", Surname = "", PasswordHash = "", RoleId = 1 });
        _shopDbContext.Add(new Status() { Id = (int)OrderStatus.AwaitingConfirmation, Name = "Awaiting confirmation" });
        _shopDbContext.Add(new Product() { Id = 1, BrandId = 1, CategoryId = 1, Name = "a", Quantity = quantity, Price = 10, Description = "" });
        _shopDbContext.Add(new Product() { Id = 2, BrandId = 1, CategoryId = 1, Name = "b", Quantity = quantity, Price = 5, Description = "" });
        _shopDbContext.SaveChanges();

        SetupOrderRepository();
        SetupProductRepository();
        SetupUserRepository();
        SetupOrderHistoryRepository();
        SetupOrderProductRepository();

        var inputProducts = new List<OrderProduct>() { new OrderProduct() { ProductId = productId, Count = count } };

        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        // Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(async () => await orderService.AddOrder(userId, inputProducts));
        ex.Message.Should().Match(String.Format(ProductExceptionsMessages.ProductNotEnoughQuantity, productId));
    }
}