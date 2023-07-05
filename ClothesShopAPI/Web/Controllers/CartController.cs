using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Extension;
using Web.Models.InputModels;

namespace Web.Controllers;
[Route("api/Cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly TimeSpan reservationTime;
    private string configurationPath = "CartSettings:ReservationTime";

    public CartController(ICartService cartService, WebApplicationBuilder applicationBuilder)
    {
        _cartService = cartService;
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

        await _cartService.AddProductToCart(userId, orderProduct.ProductId, orderProduct.Count, reservationTime);

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

        await _cartService.RemoveProductFromCart(userId, productId);

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

        await _cartService.UpdateProductInCart(userId, productId, newCount, reservationTime);

        return new OkResult();
    }
}
