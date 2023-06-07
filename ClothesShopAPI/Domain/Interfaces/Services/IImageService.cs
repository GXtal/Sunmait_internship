using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IImageService
{
    public Task<IEnumerable<Image>> GetImagesByProduct(int productId);

    public Task AddImage(string newImagePath, int productId);

    public Task RemoveImage(int id);
}
