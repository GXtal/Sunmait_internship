using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ShopDbContext _dbContext;

    public OrderRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> AddOrder(Order order)
    {
        _dbContext.Add(order);
        await Save();
        return order;
    }

    public async Task<Order> GetOrderById(int id)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
        return order;
    }

    public async Task<IEnumerable<Order>> GetOrdersByUser(User user)
    {
        var orders = await _dbContext.Orders.Where(o => o.UserId == user.Id).ToListAsync();
        return orders;
    }

    public async Task RemoveOrder(Order order)
    {
        _dbContext.Orders.Remove(order);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateOrder(Order order)
    {
        _dbContext.Entry(order).State = EntityState.Modified;
        await Save();
    }
}
