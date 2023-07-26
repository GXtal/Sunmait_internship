using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface ICartService
{
    public Task<Product> AddProductToCart(int userId, int productId, int count, TimeSpan reservationTime);

    public Task<Product> RemoveProductFromCart(int userId, int productId);

    public Task<Product> UpdateProductInCart(int userId, int productId, int newCount, TimeSpan reservationTime);
}
