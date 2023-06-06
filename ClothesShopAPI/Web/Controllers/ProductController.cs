using Application.Services;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers;

[Route("api/Products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IReviewService _reviewService;

    public ProductController(IProductService productService, IReviewService reviewService)
    {
        _productService = productService;
        _reviewService = reviewService;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var allProducts = await _productService.GetProducts();

        var result = new List<ProductViewModel>();
        foreach (var product in allProducts)
        {
            result.Add(new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity
            });
        }

        return new OkObjectResult(result);
    }

    // GET api/Products/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] int id)
    {
        var product = await _productService.GetProduct(id);
        var result = new ProductViewModel()
        {
            Id = product.Id,
            Name = product.Name,
            BrandId = product.BrandId,
            CategoryId = product.CategoryId,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity
        };
        return new OkObjectResult(result);
    }

    // GET: api/Products/5/Reviews
    [HttpGet("{id}/Reviews")]
    public async Task<IActionResult> GetProductReviews([FromRoute] int id)
    {
        var allReviews = await _reviewService.GetReviews(id);

        var result = new List<ReviewViewModel>();
        foreach (var review in allReviews)
        {
            result.Add(new ReviewViewModel()
            {
                Id = review.Id,
                Comment = review.Comment,
                Rating = review.Rating,
                ProductId = review.ProductId,
                UserId = review.ProductId
            });
        }

        return new OkObjectResult(result);
    }

    // POST api/Products
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductInputModel newProduct)
    {
        await _productService.AddProduct(newProduct.Name, newProduct.Description,
            newProduct.Price, newProduct.Quantity, newProduct.BrandId, newProduct.CategoryId);
        return new OkResult();
    }

    // PUT api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductInputModel newProduct)
    {
        await _productService.UpdateProduct(id, newProduct.Name, newProduct.Description,
            newProduct.Price, newProduct.Quantity, newProduct.BrandId, newProduct.CategoryId);
        return new OkResult();
    }
}
