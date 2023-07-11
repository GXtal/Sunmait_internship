using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Web.Authorization;
using Web.AuthorizationData;
using Web.Extension;
using Web.Hubs.ClientInterfaces;
using Web.Hubs;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers;

[Route("api/Products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IHubContext<ProductCountHub, IProductCountClient> _hubContext;

    public ProductController(IProductService productService, IHubContext<ProductCountHub, IProductCountClient> hubContext)
    {
        _productService = productService;
        _hubContext = hubContext;
    }

    // GET: api/Products
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductViewModel>))]
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
                AvailableQuantity = product.AvailableQuantity,
                ReservedQuantity = product.ReservedQuantity,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
            });
        }

        return new OkObjectResult(result);
    }

    // GET: api/Products/Brand/5
    [HttpGet("Brand/{brandId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductViewModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                AvailableQuantity = product.AvailableQuantity,
                ReservedQuantity = product.ReservedQuantity,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
            });
        }

        return new OkObjectResult(result);
    }

    // GET: api/Products/Category/5
    [HttpGet("Category/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductViewModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                AvailableQuantity = product.AvailableQuantity,
                ReservedQuantity = product.ReservedQuantity,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
            });
        }

        return new OkObjectResult(result);
    }

    // GET api/Products/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            AvailableQuantity = product.AvailableQuantity,
            ReservedQuantity = product.ReservedQuantity,
            CategoryName = product.Category.Name,
            BrandName = product.Brand.Name,
        };
        return new OkObjectResult(result);
    }

    // POST api/Products
    [Authorize]
    [RequiresClaim(CustomClaimNames.RoleId, (int)UserRole.Admin)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddProduct([FromBody] ProductInputModel newProduct)
    {
        await _productService.AddProduct(newProduct.Name, newProduct.Description,
            newProduct.Price, newProduct.Quantity, newProduct.BrandId, newProduct.CategoryId);
        return new OkResult();
    }

    // PUT api/Products/5
    [Authorize]
    [RequiresClaim(CustomClaimNames.RoleId, (int)UserRole.Admin)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductInputModel newProduct)
    {
        var product = await _productService.UpdateProduct(id, newProduct.Name, newProduct.Description,
            newProduct.Price, newProduct.Quantity, newProduct.BrandId, newProduct.CategoryId);
        _hubContext.Clients.Group(product.Id.ToString()).GetProductCount(new ProductCountViewModel
        {
            ProductId = product.Id,
            AvailableQuantity = product.AvailableQuantity,
            ReservedQuantity = product.ReservedQuantity
        });

        return new OkResult();
    }

    // GET api/Products/Orders/5
    [Authorize]
    [HttpGet("Orders/{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderProductViewModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOrderProducts([FromRoute] int orderId)
    {
        var userId = User.GetUserId();
        var orderProducts = await _productService.GetOrderProducts(orderId, userId);
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

    // GET api/Products/Orders/5
    [Authorize]
    [HttpGet("Cart/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderProductViewModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetReservedProducts([FromRoute] int userId)
    {
        User.CheckIsOwnerOrAdmin(userId);
        var orderProducts = await _productService.GetReservedProducts(userId);
        var result = new List<OrderProductViewModel>();
        foreach (var orderProduct in orderProducts)
        {
            result.Add(new OrderProductViewModel
            {
                ProductId = orderProduct.ProductId,
                Count = orderProduct.Count,
                ProductName = orderProduct.Product.Name,
                ProductPrice = orderProduct.Product.Price,
            });
        }
        return new OkObjectResult(result);
    }

}
