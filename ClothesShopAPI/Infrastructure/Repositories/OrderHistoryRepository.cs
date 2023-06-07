using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderHistoryRepository : IOrderHistoryRepository
{
    private readonly ShopDbContext _dbContext;

    public OrderHistoryRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<OrderHistory> AddHistory(OrderHistory orderHistory)
    {
        _dbContext.Add(orderHistory);
        await Save();
        return orderHistory;
    }

    public async Task<IEnumerable<OrderHistory>> GetHistoryByOrder(Order order)
    {
        var orderHistories = await _dbContext.OrderHistories.
            Include(oh => oh.Status).
            Where(oh => oh.OrderId == order.Id).
            ToListAsync();
        return orderHistories;
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
