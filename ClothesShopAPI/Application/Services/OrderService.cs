using Domain.Entities;
using Domain.Interfaces.Services;

namespace Application.Services;
public class OrderService : IOrderService
{
    public Task AddOrderStatus(int id, int statusId)
    {
        throw new NotImplementedException();
    }

    public Task AddProductToOrder(int id, int productId, int count)
    {
        throw new NotImplementedException();
    }

    public Task<Order> CreateOrder(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetOrder(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OrderHistory>> GetOrderHistory(int id)
    {
        throw new NotImplementedException();
    }

    public Task RemoveProductFromOrder(int id, int productId)
    {
        throw new NotImplementedException();
    }
}
