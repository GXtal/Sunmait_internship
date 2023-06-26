using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories;

public interface IOrderRepository
{
    public Task<Order> GetOrderById(int id);

    public Task<IEnumerable<Order>> GetOrdersByUser(int userId);

    public Task<Order> AddOrder(Order order);

    public Task UpdateOrder(Order order);

    public Task RemoveOrder(Order order);

    public Task<IEnumerable<Order>> GetOrdersByUserAndProduct(int userId, int productId);

    public Task Save();
}
