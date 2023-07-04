using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface ICartService
{
    public Task AddProductToCart(int userId, int productId, int count);

    public Task RemoveProductFromCart(int userId, int productId);

    public Task UpdateProductInCart(int userId, int productId, int count);
}
