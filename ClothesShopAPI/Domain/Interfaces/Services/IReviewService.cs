using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IReviewService
{
    public Task<IEnumerable<Review>> GetReviews(int productId);

    public Task AddReview(string newReviewComment, int rating, int productId, int userId);

    public Task RemoveReview(int id, int userId);
}
