using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IImageRepository
{
    public Task<Image> AddImage(Image image);

    public Task<Image> GetImageById(int id);

    public Task<IEnumerable<Image>> GetImagesByProduct(Product product);

    public Task RemoveImage(Image image);

    public Task Save();
}
