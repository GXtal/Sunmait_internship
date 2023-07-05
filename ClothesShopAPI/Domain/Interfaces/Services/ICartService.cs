using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface ICartService
{
    public Task AddProductToCart(int userId, int productId, int count, TimeSpan reservationTime);

    public Task RemoveProductFromCart(int userId, int productId);

    public Task UpdateProductInCart(int userId, int productId, int newCount, TimeSpan reservationTime);
}
