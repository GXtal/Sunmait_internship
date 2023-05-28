using Application.Exceptions;
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
            if (brand == null)
            {
                throw new BrandNotFoundException($"Brand with id={id} is not found");
            }
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
                throw new ExistingBrandNameException($"Can't add brand with name={newBrandName}.Brand with this name already exists");
            }

            var newBrand = new Brand { Name = newBrandName };
            await _brandRepository.AddBrand(newBrand);

            return true;
        }

        public async Task<bool> UpdateBrand(int id, string newBrandName)
        {
            var updatedBrand = await _brandRepository.GetBrandById(id);
            if (updatedBrand == null)
            {
                throw new BrandNotFoundException($"Brand with id={id} is not found");
            }

            var existingBrand = await _brandRepository.GetBrandByName(newBrandName);
            if (existingBrand != null)
            {
                throw new ExistingBrandNameException($"Can't rename brand to name={newBrandName}.Brand with this name already exists");
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
                throw new BrandNotFoundException($"Brand with id={id} is not found");
            }

            var products = await _productRepository.GetProductsByBrand(removedBrand);
            if(products.Count()>0)
            {
                throw new BrandInUseException($"Can't remove brand with id={id}. This brand is used by products");
            }

            await _brandRepository.RemoveBrand(removedBrand);

            return true;
        }
    }
}
