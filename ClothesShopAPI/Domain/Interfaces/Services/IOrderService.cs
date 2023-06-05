using Domain.Entities;

namespace Domain.Interfaces.Services;
public interface IOrderService
{
    public Task<Order> GetOrder(int id);

    public Task<IEnumerable<Order>> GetOrders(int userId);

    public Task<IEnumerable<OrderHistory>> GetOrderHistory(int id);

    public Task AddOrder(int userId);

    public Task AddProductToOrder(int id, int productId, int count);

    public Task RemoveProductFromOrder(int id, int productId);

    public Task AddOrderStatus(int id, int statusId);
}
