using Domain.Entities;

namespace Domain.Interfaces.Repositories;
public interface IReservedProductRepository
{
    public Task<ReservedProduct> AddReservedProduct(ReservedProduct reservedProduct);

    public Task RemoveReservedProduct(ReservedProduct reservedProduct);

    public Task UpdateReservedProduct(ReservedProduct reservedProduct);

    public Task<IEnumerable<ReservedProduct>> GetReservedProductsByUser(int userId);

    public Task<ReservedProduct> GetReservedProduct(int userId, int productId);

    public Task<IEnumerable<ReservedProduct>> GetExpiredReservedProducts(DateTime currentTime);

    public Task Save();
}
