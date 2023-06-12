using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IReviewRepository
{
    public Task<Review> AddReview(Review review);

    public Task<Review> GetReviewById(int id);

    public Task<IEnumerable<Review>> GetReviewsByProduct(int productId);

    public Task RemoveReview(Review review);

    public Task Save();
}
