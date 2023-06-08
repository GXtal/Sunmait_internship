using Application.Services;
using Domain.Entities;
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

    public ProductController(IProductService productService)
    {
        _productService = productService;
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
                Quantity = product.Quantity,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
            });
        }

        return new OkObjectResult(result);
    }

    // GET: api/Products/Brand/5
    [HttpGet("Brand/{brandId}")]
    public async Task<IActionResult> GetProductsByBrand([FromRoute] int brandId)
    {
        var allProducts = await _productService.GetProductsByBrand(brandId);

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
                Quantity = product.Quantity,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
            });
        }

        return new OkObjectResult(result);
    }

    // GET: api/Products/Category/5
    [HttpGet("Category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategory([FromRoute] int categoryId)
    {
        var allProducts = await _productService.GetProductsByCategory(categoryId);

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
                Quantity = product.Quantity,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
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
            Quantity = product.Quantity,
            CategoryName = product.Category.Name,
            BrandName = product.Brand.Name,
        };
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

    // GET api/Products/Orders/5
    [HttpGet("Orders/{orderId}")]
    public async Task<IActionResult> GetOrderProducts([FromRoute] int orderId)
    {
        var orderProducts = await _productService.GetOrderProducts(orderId);
        var result = new List<OrderProductViewModel>();
        foreach (var orderProduct in orderProducts)
        {
            result.Add(new OrderProductViewModel
            {
                OrderId = orderProduct.OrderId,
                ProductId = orderProduct.ProductId,
                Count = orderProduct.Count,
                ProductName = orderProduct.Product.Name,
                ProductPrice = orderProduct.Product.Price,
            });
        }
        return new OkObjectResult(result);
    }
}
