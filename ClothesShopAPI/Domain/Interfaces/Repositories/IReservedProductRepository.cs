using Domain.Entities;

namespace Domain.Interfaces.Repositories;
public interface IReservedProductRepository
{
    public Task AddReservedProduct(int userId, int productId, int count);

    public Task RemoveReservedProduct(int userId, int productId);

    public Task<IEnumerable<ReservedProduct>> GetReservedProductsByUser(int userId);
}
