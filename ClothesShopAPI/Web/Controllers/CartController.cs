using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Web.Extension;
using Web.Hubs;
using Web.Hubs.ClientInterfaces;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers;
[Route("api/Cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly TimeSpan reservationTime;
    private readonly IHubContext<ProductCountHub, IProductCountClient> _hubContext;
    private string configurationPath = "CartSettings:ReservationTime";


    public CartController(ICartService cartService, WebApplicationBuilder applicationBuilder,
        IHubContext<ProductCountHub, IProductCountClient> hubContext)
    {
        _cartService = cartService;
        _hubContext = hubContext;
        reservationTime = TimeSpan.FromMinutes(Int32.Parse(applicationBuilder.Configuration[configurationPath]));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddProductToCart([FromBody] OrderProductInputModel orderProduct)
    {
        var userId = User.GetUserId();

        var product = await _cartService.AddProductToCart(userId, orderProduct.ProductId, orderProduct.Count, reservationTime);
        _hubContext.Clients.Group(product.Id.ToString()).GetProductCount(new ProductCountViewModel
        {
            ProductId = product.Id,
            AvailableQuantity = product.AvailableQuantity,
            ReservedQuantity = product.ReservedQuantity
        });

        return new OkResult();
    }

    [Authorize]
    [HttpDelete("{productId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveProductFromCart([FromRoute] int productId)
    {
        var userId = User.GetUserId();

        var product = await _cartService.RemoveProductFromCart(userId, productId);
        _hubContext.Clients.Group(product.Id.ToString()).GetProductCount(new ProductCountViewModel
        {
            ProductId = product.Id,
            AvailableQuantity = product.AvailableQuantity,
            ReservedQuantity = product.ReservedQuantity
        });

        return new OkResult();
    }

    [Authorize]
    [HttpPut("{productId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateProductInCart([FromRoute] int productId, [FromBody] int newCount)
    {
        var userId = User.GetUserId();

        var product = await _cartService.UpdateProductInCart(userId, productId, newCount, reservationTime);
        _hubContext.Clients.Group(product.Id.ToString()).GetProductCount(new ProductCountViewModel
        {
            ProductId = product.Id,
            AvailableQuantity = product.AvailableQuantity,
            ReservedQuantity = product.ReservedQuantity
        });

        return new OkResult();
    }
}
