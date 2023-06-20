using Application.Exceptions.Messages;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;

public class ReviewService : IReviewService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IProductRepository productRepository, IUserRepository userRepository, IReviewRepository reviewRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task AddReview(string newReviewComment, int rating, int productId, int userId)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, productId));
        }

        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }

        var review = new Review() { Comment = newReviewComment, ProductId = productId, UserId = userId, Rating = rating };
        await _reviewRepository.AddReview(review);
    }

    public async Task<IEnumerable<Review>> GetReviews(int productId)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product == null)
        {
            throw new NotFoundException(String.Format(ProductExceptionsMessages.ProductNotFound, productId));
        }

        var reviews = await _reviewRepository.GetReviewsByProduct(product.Id);
        return reviews;
    }

    public async Task RemoveReview(int id, int userId)
    {
        var review = await _reviewRepository.GetReviewById(id);
        if (review == null)
        {
            throw new NotFoundException(String.Format(ReviewExceptionsMessages.ReviewNotFound, id));
        }
        if (review.UserId != userId)
        {
            throw new ForbiddenException(UserExceptionsMessages.ForbiddenModify);
        }

        await _reviewRepository.RemoveReview(review);
    }
}
