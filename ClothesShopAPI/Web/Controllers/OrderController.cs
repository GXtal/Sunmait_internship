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

[Route("api/Orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IHubContext<ProductCountHub, IProductCountClient> _hubContext;

    public OrderController(IOrderService orderService, IHubContext<ProductCountHub, IProductCountClient> hubContext)
    {
        _orderService = orderService;
        _hubContext = hubContext;
    }

    // GET api/Orders/5
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOrderById([FromRoute] int id)
    {
        var userId = User.GetUserId();
        var order = await _orderService.GetOrder(id, userId);
        var result = new OrderViewModel()
        {
            Id = order.Id,
            TotalCost = order.TotalCost,
            UserId = order.UserId,
            StatusId = order.StatusId
        };
        return new OkObjectResult(result);
    }

    // GET api/Orders/5/History
    [Authorize]
    [HttpGet("{id}/History")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderHistoryViewModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOrderHistory([FromRoute] int id)
    {
        var userId = User.GetUserId();
        var history = await _orderService.GetOrderHistory(id, userId);
        var result = new List<OrderHistoryViewModel>();
        foreach (var item in history)
        {
            result.Add(new OrderHistoryViewModel
            {
                OrderId = item.OrderId,
                StatusId = item.StatusId,
                StatusName = item.Status.Name,
                SetTime = item.SetTime
            });
        }
        return new OkObjectResult(result);
    }

    // POST api/Orders/5/History/3
    [Authorize]
    [RequiresClaim(CustomClaimNames.RoleId, (int)UserRole.Admin)]
    [HttpPost("{id}/History/{statusId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddStatusToOrder([FromRoute] int id, [FromRoute] int statusId)
    {
        await _orderService.AddOrderStatus(id, statusId);
        return new OkResult();
    }

    // GET api/Orders/User/5
    [Authorize]
    [HttpGet("Users/{userId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderViewModel>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOrders([FromRoute] int userId)
    {
        User.CheckIsOwnerOrAdmin(userId);

        var allOrders = await _orderService.GetOrders(userId);

        var result = new List<OrderViewModel>();
        foreach (var order in allOrders)
        {
            result.Add(new OrderViewModel()
            {
                Id = order.Id,
                TotalCost = order.TotalCost,
                UserId = order.UserId,
                StatusId = order.StatusId
            });
        }

        return new OkObjectResult(result);
    }

    // POST api/Orders
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddOrder()
    {
        var userId = User.GetUserId();

        var modifiedProducts = await _orderService.AddOrder(userId);
        foreach (var product in modifiedProducts)
        {
            _hubContext.Clients.Group(product.Id.ToString()).GetProductCount(new ProductCountViewModel
            {
                ProductId = product.Id,
                AvailableQuantity = product.AvailableQuantity,
                ReservedQuantity = product.ReservedQuantity
            });
        }

        return new OkResult();
    }
}
