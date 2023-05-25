using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBrandRepository
    {
        public Task<bool> RemoveBrand(int id);

        public Task<Brand> GetBrandById(int id);

        public Task<IEnumerable<Brand>> GetBrands();

        public Task<bool> AddBrand(string newBrandName);

        public Task<bool> UpdateBrand(int id, string newBrandName);

        public Task Save();
    }
}
