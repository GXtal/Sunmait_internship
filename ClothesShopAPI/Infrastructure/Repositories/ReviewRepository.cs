﻿using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ShopDbContext _dbContext;

    public ReviewRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Review> AddReview(Review review)
    {
        _dbContext.Add(review);
        await Save();
        return review;
    }

    public async Task<Review> GetReviewById(int id)
    {
        var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        return review;
    }

    public async Task<IEnumerable<Review>> GetReviewsByProduct(Product product)
    {
        var reviews = await _dbContext.Reviews.Where(r => r.ProductId== product.Id).ToListAsync();
        return reviews;
    }

    public async Task RemoveReview(Review review)
    {
        _dbContext.Reviews.Remove(review);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}