using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOrderProductRepository
{
    public Task<OrderProduct> AddOrderProduct(OrderProduct orderProduct);
    public Task<OrderProduct> GetOrderProduct(Order order, Product product);
    public Task RemoveOrderProduct(OrderProduct orderProduct);
    public Task UpdateOrderProduct(OrderProduct orderProduct);
}
