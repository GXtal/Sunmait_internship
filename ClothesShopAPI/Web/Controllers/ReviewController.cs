using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers;

[Route("api/Reviews")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // POST api/Reviews
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddReview([FromBody] ReviewInputModel newReview)
    {
        await _reviewService.AddReview(newReview.Comment, newReview.Rating, newReview.ProductId, newReview.UserId);
        return new OkResult();
    }

    // DELETE api/Reviews/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveReview([FromRoute] int id)
    {
        await _reviewService.RemoveReview(id);
        return new OkResult();
    }

    // GET: api/Reviews/Products/5
    [HttpGet("Products/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReviewViewModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductReviews([FromRoute] int productId)
    {
        var allReviews = await _reviewService.GetReviews(productId);

        var result = new List<ReviewViewModel>();
        foreach (var review in allReviews)
        {
            result.Add(new ReviewViewModel()
            {
                Id = review.Id,
                Comment = review.Comment,
                Rating = review.Rating,
                ProductId = review.ProductId,
                UserId = review.ProductId,
                UserName = review.User.Name,
            });
        }

        return new OkObjectResult(result);
    }
}
