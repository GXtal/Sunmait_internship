using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Application.Exceptions;
using Application.Exceptions.Messages;

namespace Application.Services;
public class OrderService : IOrderService
{
    public const int AwaitingConfirmation = 1;
    public const int Delivering = 2;
    public const int Completed = 3;
    public const int Canceled = 4;

    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderHistoryRepository _orderHistoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IStatusRepository _statusRepository;

    public OrderService(IUserRepository userRepository, IOrderRepository orderRepository,
        IOrderHistoryRepository orderHistoryRepository, IProductRepository productRepository,
        IOrderProductRepository orderProductRepository, IStatusRepository statusRepository)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _orderHistoryRepository = orderHistoryRepository;
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
        _statusRepository = statusRepository;
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

        var orderHistory = new OrderHistory() { OrderId = id, StatusId = statusId, SetTime = DateTime.Now };
        await _orderHistoryRepository.AddHistory(orderHistory);
    }

    public async Task AddProductToOrder(int id, int productId, int count)
    {
        var order = await _orderRepository.GetOrderById(id);
        if (order == null)
        {
            throw new NotFoundException(String.Format(OrderExceptionsMessages.OrderNotFound, id));
        }

        var history = await _orderHistoryRepository.GetHistoryByOrder(order);
        history = history.OrderByDescending(h => h.SetTime);

        var currentStatus = history.First();
        
        if (currentStatus.StatusId != AwaitingConfirmation)
        {
            throw new BadRequestException(String.Format(OrderExceptionsMessages.OrderUnchangeable, id));
        }

        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, id));
        }

        if(product.Quantity < count)
        {
            throw new BadRequestException(String.Format(ProductExceptionsMessages.ProductNotEnoughQuantity, id));
        }

        var existingOrderProduct =await  _orderProductRepository.GetOrderProduct(order, product);
        if (existingOrderProduct == null)
        {
            var orderProduct = new OrderProduct() { OrderId = id, ProductId = productId, Count = count };
            await _orderProductRepository.AddOrderProduct(orderProduct);
        }
        else
        {
            existingOrderProduct.Count += count;
            await _orderProductRepository.UpdateOrderProduct(existingOrderProduct);
        }
        
        product.Quantity -= count;        
        await _productRepository.UpdateProduct(product);

        order.TotalCost += count * product.Price;
        await _orderRepository.UpdateOrder(order);
    }

    public async Task AddOrder(int userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }

        var order = new Order() { UserId = userId, TotalCost = 0 };
        order = await _orderRepository.AddOrder(order);

        var orderHistory = new OrderHistory() { OrderId = order.Id, StatusId = AwaitingConfirmation, SetTime = DateTime.Now };
        await _orderHistoryRepository.AddHistory(orderHistory);
    }

    public async Task<Order> GetOrder(int id)
    {
        var order = await _orderRepository.GetOrderById(id);
        if (order == null)
        {
            throw new NotFoundException(String.Format(OrderExceptionsMessages.OrderNotFound, id));
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

        var orders = await _orderRepository.GetOrdersByUser(user);
        return orders;
    }

    public async Task<IEnumerable<OrderHistory>> GetOrderHistory(int id)
    {
        var order = await _orderRepository.GetOrderById(id);
        if (order == null)
        {
            throw new NotFoundException(String.Format(OrderExceptionsMessages.OrderNotFound, id));
        }

        var history = await _orderHistoryRepository.GetHistoryByOrder(order);
        return history;
    }

    public async Task RemoveProductFromOrder(int id, int productId)
    {
        var order = await _orderRepository.GetOrderById(id);
        if (order == null)
        {
            throw new NotFoundException(String.Format(OrderExceptionsMessages.OrderNotFound, id));
        }

        var history = await _orderHistoryRepository.GetHistoryByOrder(order);
        history = history.OrderByDescending(h => h.SetTime);

        var currentStatus = history.First();

        if (currentStatus.StatusId != AwaitingConfirmation)
        {
            throw new BadRequestException(String.Format(OrderExceptionsMessages.OrderUnchangeable, id));
        }

        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, id));
        }

        var orderProduct = await _orderProductRepository.GetOrderProduct(order, product);
        if (orderProduct == null)
        {
            throw new NotFoundException(String.Format(OrderProductExceptionsMessages.OrderProductNotFound, id, productId));
        }

        order.TotalCost -= product.Price * orderProduct.Count;
        await _orderRepository.UpdateOrder(order);

        product.Quantity += orderProduct.Count;
        await _productRepository.UpdateProduct(product);

        await _orderProductRepository.RemoveOrderProduct(orderProduct);
    }    
}
