using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IImageService
{
    public Task<IEnumerable<int>> GetImageIdsByProduct(int productId);

    public Task<Image> GetImageById(int id);

    public Task AddImage(byte[] content, int productId);

    public Task RemoveImage(int id);
}
