using Application.Exceptions;
using Application.Exceptions.Messages;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;

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
            throw new NotFoundException(String.Format(BrandExceptionsMessages.BrandNotFound, id));
        }
        return brand;
    }

    public async Task<IEnumerable<Brand>> GetBrands()
    {
        var brands = await _brandRepository.GetBrands();
        return brands;
    }

    public async Task AddBrand(string newBrandName)
    {
        var existingBrand = await _brandRepository.GetBrandByName(newBrandName);
        if (existingBrand != null)
        {
            throw new BadRequestException(String.Format(BrandExceptionsMessages.BrandNameExists, newBrandName));
        }

        var brand = new Brand { Name = newBrandName };
        await _brandRepository.AddBrand(brand);
    }

    public async Task RenameBrand(int id, string newBrandName)
    {
        var brand = await _brandRepository.GetBrandById(id);
        if (brand == null)
        {
            throw new NotFoundException(String.Format(BrandExceptionsMessages.BrandNotFound, id));
        }

        var existingBrand = await _brandRepository.GetBrandByName(newBrandName);
        if (existingBrand != null)
        {
            throw new BadRequestException(String.Format(BrandExceptionsMessages.BrandNameExists, newBrandName));
        }

        brand.Name = newBrandName;
        await _brandRepository.UpdateBrand(brand);
    }

    public async Task RemoveBrand(int id)
    {
        var brand = await _brandRepository.GetBrandById(id);
        if (brand == null)
        {
            throw new NotFoundException(String.Format(BrandExceptionsMessages.BrandNotFound, id));
        }

        var products = await _productRepository.GetProductsByBrand(brand);
        if (products.Count() > 0)
        {
            throw new BadRequestException(String.Format(BrandExceptionsMessages.BrandIsUsed, id));
        }

        await _brandRepository.RemoveBrand(brand);
    }
}
