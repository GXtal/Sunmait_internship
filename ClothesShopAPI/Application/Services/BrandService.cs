using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;

        public BrandService(IBrandRepository brandRepository, IProductRepository productRepository)
        {
            _brandRepository = brandRepository;
            _productRepository = productRepository;
        }

        public async Task<Brand> GetBrand(int id)
        {
            var brand = await _brandRepository.GetBrandById(id);
            return brand;
        }

        public async Task<IEnumerable<Brand>> GetBrands()
        {
            var brands = await _brandRepository.GetBrands();
            return brands;
        }

        public async Task<bool> AddBrand(string newBrandName)
        {
            var existingBrand = await _brandRepository.GetBrandByName(newBrandName);
            if (existingBrand != null)
            {
                return false;
            }

            var newBrand = new Brand { Name = newBrandName };
            await _brandRepository.AddBrand(newBrand);

            return true;
        }

        public async Task<bool> UpdateBrand(int id, string newBrandName)
        {
            var existingBrand = await _brandRepository.GetBrandByName(newBrandName);
            if (existingBrand != null)
            {
                return false;
            }

            var updatedBrand = await _brandRepository.GetBrandById(id);
            if (updatedBrand == null)
            {
                return false;
            }

            updatedBrand.Name = newBrandName;
            await _brandRepository.UpdateBrand(updatedBrand);

            return true;
        }

        public async Task<bool> RemoveBrand(int id)
        {
            var removedBrand = await _brandRepository.GetBrandById(id);
            if (removedBrand == null)
            {
                return false;
            }

            var products = await _productRepository.GetProductsByBrand(removedBrand);
            if(products.Count()>0)
            {
                return false;
            }

            await _brandRepository.RemoveBrand(removedBrand);

            return true;
        }
    }
}
