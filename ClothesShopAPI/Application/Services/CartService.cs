using Application.Exceptions.Messages;
using Application.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Entities;

namespace Application.Services;

public class CartService : ICartService
{
    private readonly IReservedProductRepository _reservedProductRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public CartService(IReservedProductRepository reservedProductRepository, IProductRepository productRepository,
        IUserRepository userRepository)
    {
        _reservedProductRepository = reservedProductRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    public async Task<Product> AddProductToCart(int userId, int productId, int count, TimeSpan reservationTime)
    {
        var user = _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }

        var product = await _productRepository.GetProductById(productId);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, productId));
        }

        if (product.AvailableQuantity < count)
        {
            throw new BadRequestException(String.Format(ProductExceptionsMessages.ProductNotEnoughQuantity, productId));
        }

        var reservedProduct = new ReservedProduct()
        {
            UserId = userId,
            ProductId = productId,
            Count = count,
            ExpirationTime = DateTime.Now + reservationTime
        };

        product.AvailableQuantity -= count;
        product.ReservedQuantity += count;

        await _reservedProductRepository.AddReservedProduct(reservedProduct);
        await _productRepository.UpdateProduct(product);

        return product;
    }

    public async Task<Product> RemoveProductFromCart(int userId, int productId)
    {
        var user = _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }

        var product = await _productRepository.GetProductById(productId);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, productId));
        }

        var reservedProduct = await _reservedProductRepository.GetReservedProduct(userId, productId);
        if (reservedProduct == null)
        {
            throw new NotFoundException(String.Format(CartExceptionMessages.ReservedProductNotFound, userId, productId));
        }

        product.AvailableQuantity += reservedProduct.Count;
        product.ReservedQuantity -= reservedProduct.Count;

        await _reservedProductRepository.RemoveReservedProduct(reservedProduct);
        await _productRepository.UpdateProduct(product);

        return product;
    }

    public async Task<Product> UpdateProductInCart(int userId, int productId, int newCount, TimeSpan reservationTime)
    {
        var user = _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }

        var product = await _productRepository.GetProductById(productId);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, productId));
        }

        var reservedProduct = await _reservedProductRepository.GetReservedProduct(userId, productId);
        if (reservedProduct == null)
        {
            throw new NotFoundException(String.Format(CartExceptionMessages.ReservedProductNotFound, userId, productId));
        }

        product.AvailableQuantity += reservedProduct.Count;
        product.ReservedQuantity -= reservedProduct.Count;
        if (product.AvailableQuantity < newCount)
        {
            throw new BadRequestException(String.Format(ProductExceptionsMessages.ProductNotEnoughQuantity, productId));
        }

        product.AvailableQuantity -= newCount;
        product.ReservedQuantity += newCount;

        reservedProduct.Count = newCount;
        reservedProduct.ExpirationTime = DateTime.Now + reservationTime;

        await _reservedProductRepository.UpdateReservedProduct(reservedProduct);
        await _productRepository.UpdateProduct(product);

        return product;
    }
}
