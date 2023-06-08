using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly ShopDbContext _dbContext;

    public ImageRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Image> AddImage(Image image)
    {
        _dbContext.Add(image);
        await Save();
        return image;
    }

    public async Task<Image> GetImageById(int id)
    {
        var image = await _dbContext.Images.FirstOrDefaultAsync(b => b.Id == id);
        return image;
    }

    public async Task<IEnumerable<int>> GetImageIdsByProduct(Product product)
    {
        var allImages = await _dbContext.Images.Where(p => p.ProductId == product.Id).Select(i => i.Id).ToListAsync();
        return allImages;
    }

    public async Task RemoveImage(Image image)
    {
        _dbContext.Images.Remove(image);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
