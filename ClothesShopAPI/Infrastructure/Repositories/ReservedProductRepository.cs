using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservedProductRepository : IReservedProductRepository
{
    private readonly ShopDbContext _dbContext;

    public ReservedProductRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReservedProduct> AddReservedProduct(ReservedProduct reservedProduct)
    {
        _dbContext.Add(reservedProduct);
        await Save();
        return reservedProduct;
    }

    public async Task<IEnumerable<ReservedProduct>> GetExpiredReservedProducts(DateTime currentTime)
    {
        var reservedProducts = await _dbContext.ReservedProducts.
            Where(pr => pr.ExpirationTime <= currentTime).
            ToListAsync();
        return reservedProducts;
    }

    public async Task<ReservedProduct> GetReservedProduct(int userId, int productId)
    {
        var reservedProduct = await _dbContext.ReservedProducts.
            Where(pr => pr.ProductId == productId && pr.UserId == userId).
            FirstOrDefaultAsync();
        return reservedProduct;
    }

    public async Task<IEnumerable<ReservedProduct>> GetReservedProductsByUser(int userId)
    {
        var reservedProducts = await _dbContext.ReservedProducts.
            Include(rp => rp.Product).
            Where(rp => rp.UserId == userId).
            ToListAsync();
        return reservedProducts;
    }

    public async Task RemoveReservedProduct(ReservedProduct reservedProduct)
    {
        _dbContext.ReservedProducts.Remove(reservedProduct);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateReservedProduct(ReservedProduct reservedProduct)
    {
        _dbContext.Entry(reservedProduct).State = EntityState.Modified;
        await Save();
    }
}
