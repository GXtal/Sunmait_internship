using Domain.Entities;
namespace Domain.Interfaces.Services;
public interface IImageService
{
    public Task<Image> GetImage(int id);

    public Task<IEnumerable<Image>> GetImagesForProduct(int productId);

    public Task AddImage(string newImagePath, int productId);

    public Task RemoveImage(int id);
}
