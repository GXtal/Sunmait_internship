﻿using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IOrderService
{
    public Task<Order> GetOrder(int id, int userId);

    public Task<IEnumerable<Order>> GetOrders(int userId);

    public Task<IEnumerable<OrderHistory>> GetOrderHistory(int id, int userId);

    public Task<IEnumerable<Product>> AddOrder(int userId);

    public Task AddOrderStatus(int id, int statusId);
}
