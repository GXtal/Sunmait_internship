﻿using Application.Exceptions;
using Application.Exceptions.Messages;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IProductRepository _productRepository;

    public ImageService(IImageRepository imageRepository, IProductRepository productRepository)
    {
        _imageRepository = imageRepository;
        _productRepository = productRepository;
    }

    public async Task AddImage(byte[] content, int productId)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, productId));
        }

        var image = new Image() { ProductId = productId, Content = content };
        await _imageRepository.AddImage(image);
    }

    public async Task<Image> GetImageById(int id)
    {
        var image = await _imageRepository.GetImageById(id);
        if (image == null)
        {
            throw new NotFoundException(String.Format(ImageExceptionsMessages.ImageNotFound, id));
        }

        return image;
    }

    public async Task<IEnumerable<int>> GetImageIdsByProduct(int productId)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, productId));
        }

        var images = await _imageRepository.GetImageIdsByProduct(product);
        return images;
    }

    public async Task RemoveImage(int id)
    {
        var image = await _imageRepository.GetImageById(id);
        if (image == null)
        {
            throw new NotFoundException(String.Format(ImageExceptionsMessages.ImageNotFound, id));
        }

        await _imageRepository.RemoveImage(image);
    }
}
