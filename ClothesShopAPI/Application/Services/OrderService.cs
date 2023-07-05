using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Application.Exceptions;
using Application.Exceptions.Messages;
using Domain.Enums;

namespace Application.Services;

public class OrderService : IOrderService
{
    private readonly Dictionary<OrderStatus, List<OrderStatus>> _possibilities = new Dictionary<OrderStatus, List<OrderStatus>>()
    {
        { OrderStatus.AwaitingConfirmation, new List<OrderStatus>() { OrderStatus.Delivering, OrderStatus.Canceled } },
        { OrderStatus.Delivering, new List<OrderStatus>() { OrderStatus.AwaitingConfirmation, OrderStatus.Canceled, OrderStatus.Completed } },
        { OrderStatus.Completed, new List<OrderStatus>() },
        { OrderStatus.Canceled, new List<OrderStatus>() }
    };

    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderHistoryRepository _orderHistoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IReservedProductRepository _cartRepository;

    public OrderService(IUserRepository userRepository, IOrderRepository orderRepository,
        IOrderHistoryRepository orderHistoryRepository, IProductRepository productRepository,
        IOrderProductRepository orderProductRepository, IStatusRepository statusRepository,
        IReservedProductRepository cartRepository)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _orderHistoryRepository = orderHistoryRepository;
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
        _statusRepository = statusRepository;
        _cartRepository = cartRepository;
    }

    public async Task AddOrderStatus(int id, int statusId)
    {
        var order = await _orderRepository.GetOrderById(id);
        if (order == null)
        {
            throw new NotFoundException(String.Format(OrderExceptionsMessages.OrderNotFound, id));
        }

        var status = await _statusRepository.GetStatusById(statusId);
        if (status == null)
        {
            throw new NotFoundException(String.Format(StatusExceptionsMessages.StatusNotFound, statusId));
        }

        if (!_possibilities[(OrderStatus)order.StatusId].Any(s => s == (OrderStatus)statusId))
        {
            throw new BadRequestException(String.Format(OrderExceptionsMessages.OrderUnchangeable, id, statusId));
        }

        var orderHistory = new OrderHistory() { OrderId = id, StatusId = statusId, SetTime = DateTime.Now };
        await _orderHistoryRepository.AddHistory(orderHistory);

        order.StatusId = statusId;
        await _orderRepository.UpdateOrder(order);
    }

    public async Task AddOrder(int userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }

        var order = new Order() { UserId = userId, TotalCost = 0, StatusId = (int)OrderStatus.AwaitingConfirmation };

        var reservedProducts = await _cartRepository.GetReservedProductsByUser(userId);

        order = await _orderRepository.AddOrder(order);

        var orderHistory = new OrderHistory() { OrderId = order.Id, StatusId = (int)OrderStatus.AwaitingConfirmation, SetTime = DateTime.Now };
        await _orderHistoryRepository.AddHistory(orderHistory);
    }

    public async Task<Order> GetOrder(int id, int userId)
    {
        var order = await _orderRepository.GetOrderById(id);
        if (order == null)
        {
            throw new NotFoundException(String.Format(OrderExceptionsMessages.OrderNotFound, id));
        }
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }
        if (!(user.RoleId == (int)UserRole.Admin || order.UserId == userId))
        {
            throw new ForbiddenException(UserExceptionsMessages.ForbiddenRead);
        }

        return order;
    }

    public async Task<IEnumerable<Order>> GetOrders(int userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }

        var orders = await _orderRepository.GetOrdersByUser(user.Id);
        return orders;
    }

    public async Task<IEnumerable<OrderHistory>> GetOrderHistory(int id, int userId)
    {
        var order = await _orderRepository.GetOrderById(id);
        if (order == null)
        {
            throw new NotFoundException(String.Format(OrderExceptionsMessages.OrderNotFound, id));
        }
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }
        if (!(user.RoleId == (int)UserRole.Admin || order.UserId == userId))
        {
            throw new ForbiddenException(UserExceptionsMessages.ForbiddenRead);
        }

        var history = await _orderHistoryRepository.GetHistoryByOrder(order);
        return history;
    }
}
