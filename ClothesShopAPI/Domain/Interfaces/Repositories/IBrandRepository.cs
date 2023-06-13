using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IBrandRepository
{
    public Task<Brand> GetBrandById(int id);

    public Task<Brand> GetBrandByName(string brandName);

    public Task<IEnumerable<Brand>> GetBrands();

    public Task<Brand> AddBrand(Brand brand);

    public Task UpdateBrand(Brand brand);

    public Task RemoveBrand(Brand brand);

    public Task Save();
}
