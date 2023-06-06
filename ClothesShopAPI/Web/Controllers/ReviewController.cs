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

    // GET api/Reviews/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewById([FromRoute] int id)
    {
        var review = await _reviewService.GetReview(id);
        var result = new ReviewViewModel()
        {
            Id = review.Id,
            Comment = review.Comment,
            Rating = review.Rating,
            ProductId = review.ProductId,
            UserId = review.ProductId
        };
        return new OkObjectResult(result);
    }

    // POST api/Reviews
    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] ReviewInputModel newReview)
    {
        await _reviewService.AddReview(newReview.Comment, newReview.Rating, newReview.ProductId, newReview.UserId);
        return new OkResult();
    }

    // DELETE api/Reviews/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveReview([FromRoute] int id)
    {
        await _reviewService.RemoveReview(id);
        return new OkResult();
    }
}
