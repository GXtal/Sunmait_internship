using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderProductRepository : IOrderProductRepository
{
    private readonly ShopDbContext _dbContext;

    public OrderProductRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<OrderProduct> AddOrderProduct(OrderProduct orderProduct)
    {
        _dbContext.Add(orderProduct);
        await Save();
        return orderProduct;
    }

    public async Task<OrderProduct> GetOrderProduct(Order order, Product product)
    {
        var orderProduct = await _dbContext.OrdersProducts.
            FirstOrDefaultAsync(cs => cs.OrderId == order.Id && cs.ProductId == product.Id);
        return orderProduct;
    }

    public async Task RemoveOrderProduct(OrderProduct orderProduct)
    {
        _dbContext.OrdersProducts.Remove(orderProduct);
        await Save();
    }

    public async Task UpdateOrderProduct(OrderProduct orderProduct)
    {
        _dbContext.Entry(orderProduct).State = EntityState.Modified;
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<OrderProduct>> GetOrderProductsByOrder(Order order)
    {
        var orderProducts = await _dbContext.OrdersProducts.
            Where(cs => cs.OrderId == order.Id).ToListAsync();
        return orderProducts;
    }
}
