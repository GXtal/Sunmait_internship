using Domain.Entities;

namespace Domain.Interfaces.Services;
public interface IReviewService
{
    public Task<Review> GetReview(int id);

    public Task<IEnumerable<Review>> GetReviewsForProduct(int productId);

    public Task AddReview(string newReviewComment, int rating, int productId, int userId);

    public Task RemoveReview(int id);
}
