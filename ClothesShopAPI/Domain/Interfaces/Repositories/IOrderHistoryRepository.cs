using Domain.Entities;

namespace Domain.Interfaces.Repositories;
public interface IOrderHistoryRepository
{
    public Task<OrderHistory> AddHistory(OrderHistory orderHistory);
    public Task<IEnumerable<OrderHistory>> GetHistoryByOrder(Order order);
}
