using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Services;

public class ReservationService : IReservationService
{
    private readonly IReservedProductRepository _reservedProductRepository;
    private readonly IProductRepository _productRepository;

    public ReservationService(IReservedProductRepository reservedProductRepository, IProductRepository productRepository)
    {
        _reservedProductRepository = reservedProductRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<int>> DeleteExpiredReservations()
    {
        var modifiedProductsIds = new List<int>();
        var expiredReservedProducts = await _reservedProductRepository.GetExpiredReservedProducts(DateTime.Now);
        foreach (var expiredReservedProduct in expiredReservedProducts)
        {
            var existingProduct = await _productRepository.GetProductById(expiredReservedProduct.ProductId);

            existingProduct.AvailableQuantity += expiredReservedProduct.Count;
            existingProduct.ReservedQuantity -= expiredReservedProduct.Count;

            await _reservedProductRepository.RemoveReservedProduct(expiredReservedProduct);
            await _productRepository.UpdateProduct(existingProduct);

            modifiedProductsIds.Add(expiredReservedProduct.ProductId);
        }

        return modifiedProductsIds;
    }
}
