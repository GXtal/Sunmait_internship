using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IBrandService
{
    public Task<IEnumerable<Brand>> GetBrands();

    public Task AddBrand(string newBrandName);

    public Task RenameBrand(int id, string newBrandName);

    public Task RemoveBrand(int id);
}
