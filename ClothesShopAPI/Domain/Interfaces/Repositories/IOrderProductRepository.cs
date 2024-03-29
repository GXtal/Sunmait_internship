﻿using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOrderProductRepository
{
    public Task<OrderProduct> AddOrderProduct(OrderProduct orderProduct);

    public Task<OrderProduct> GetOrderProduct(Order order, Product product);

    public Task<IEnumerable<OrderProduct>> GetOrderProductsByOrder(Order order);

    public Task RemoveOrderProduct(OrderProduct orderProduct);

    public Task UpdateOrderProduct(OrderProduct orderProduct);

    public Task Save();
}
