using Application.Exceptions;
using Application.Exceptions.Messages;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace TestProject;

public class OrderServiceUnitTest
{

    Mock<IOrderRepository> _orderRepository;
    Mock<IUserRepository> _userRepository;
    Mock<IProductRepository> _productRepository;
    Mock<IOrderHistoryRepository> _orderHistoryRepository;
    Mock<IOrderProductRepository> _orderProductRepository;
    Mock<IStatusRepository> _statusRepository;

    public OrderServiceUnitTest()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _orderHistoryRepository = new Mock<IOrderHistoryRepository>();
        _orderProductRepository = new Mock<IOrderProductRepository>();
        _productRepository = new Mock<IProductRepository>();
        _statusRepository = new Mock<IStatusRepository>();
        _userRepository = new Mock<IUserRepository>();
    }

    [Fact]
    public async void GetOrderById_OrderExistsTest()
    {
        // Arrange
        int workingId = 2;
        _orderRepository.Setup(x => x.GetOrderById(workingId)).ReturnsAsync(new Order() { Id = workingId });

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
    public async void GetOrderById_OrderDoesNotExistTest()
    {
        // Arrange
        int workingId = 2;
        _orderRepository.Setup(x => x.GetOrderById(1)).ReturnsAsync(new Order());

        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        try
        {
            var order = await orderService.GetOrder(workingId);
        }
        catch (Exception ex)
        {
            // Assert
            ex.Should().BeOfType<NotFoundException>();
            ex.Message.Should().Match(String.Format(OrderExceptionsMessages.OrderNotFound, workingId));
        }
    }

    [Fact]
    public async void ChangeOrderStatus_UnchangeableFrom1to3Test()
    {
        // Arrange
        int orderId = 1;
        int oldStatusId = 1;
        int statusId = 3;

        _orderRepository.Setup(x => x.GetOrderById(orderId)).ReturnsAsync(new Order() { Id = orderId, StatusId = oldStatusId });
        _statusRepository.Setup(x => x.GetStatusById(statusId)).ReturnsAsync(new Status() { Id = statusId });
        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        try
        {
            await orderService.AddOrderStatus(orderId, statusId);
        }
        catch (Exception ex)
        {
            // Assert
            ex.Should().BeOfType<BadRequestException>();
            ex.Message.Should().Match(String.Format(OrderExceptionsMessages.OrderUnchangeable, orderId, statusId));
        }
    }

    [Fact]
    public async void ChangeOrderStatus_ChangeableFrom1to2Test()
    {
        // Arrange
        int orderId = 1;
        int oldStatusId = 1;
        int statusId = 2;

        _orderRepository.Setup(x => x.GetOrderById(orderId)).ReturnsAsync(new Order() { Id = orderId, StatusId = oldStatusId });
        _statusRepository.Setup(x => x.GetStatusById(statusId)).ReturnsAsync(new Status() { Id = statusId });
        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        await orderService.AddOrderStatus(orderId, statusId);

        // Assert == didn't throw an exception
    }

    [Fact]
    public async void AddNewOrder_PossibleProductsTest()
    {
        // Arrange
        int userId = 1;
        int count = 10;
        int quantity = 20;
        _userRepository.Setup(x => x.GetUserById(userId)).ReturnsAsync(new User() { Id = userId });

        var inputProducts = new List<OrderProduct>() { new OrderProduct() { ProductId = 1, Count = count }, new OrderProduct() { ProductId = 2, Count = count } };
        _productRepository.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync((int id) => new Product() { Id = id, Quantity = quantity });

        _orderRepository.Setup(x => x.AddOrder(It.IsAny<Order>())).ReturnsAsync((Order order) => { order.Id = 1; return order; });

        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        await orderService.AddOrder(userId, inputProducts);

        // Assert == didn't throw an exception
    }

    [Fact]
    public async void AddNewOrder_ImpossibleProductCountTest()
    {
        // Arrange
        int userId = 1;
        int count = 10;
        int quantity = 20;
        int productId = 1;
        _userRepository.Setup(x => x.GetUserById(userId)).ReturnsAsync(new User() { Id = userId });

        var inputProducts = new List<OrderProduct>() { new OrderProduct() { ProductId = productId, Count = count } };
        _productRepository.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync((int id) => new Product() { Id = id, Quantity = quantity });

        _orderRepository.Setup(x => x.AddOrder(It.IsAny<Order>())).ReturnsAsync((Order order) => { order.Id = 1; return order; });

        var orderService = new OrderService(_userRepository.Object,
            _orderRepository.Object, _orderHistoryRepository.Object,
            _productRepository.Object, _orderProductRepository.Object,
            _statusRepository.Object);

        // Act
        try
        {
            await orderService.AddOrder(userId, inputProducts);
        }
        catch (Exception ex)
        {
            // Assert
            ex.Should().BeOfType<BadRequestException>();
            ex.Message.Should().Match(String.Format(ProductExceptionsMessages.ProductNotEnoughQuantity, productId));
        }

    }
}