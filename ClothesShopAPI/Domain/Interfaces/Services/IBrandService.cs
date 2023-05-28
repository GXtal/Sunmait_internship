using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IBrandService
{
    public Task<Brand> GetBrand(int id);

    public Task<IEnumerable<Brand>> GetBrands();

    public Task<bool> AddBrand(string newBrandName);

    public Task<bool> UpdateBrand(int id, string newBrandName);

    public Task<bool> RemoveBrand(int id);
}
