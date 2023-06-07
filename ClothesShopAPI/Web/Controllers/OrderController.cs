using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers;

[Route("api/Orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET api/Orders/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById([FromRoute] int id)
    {
        var order = await _orderService.GetOrder(id);
        var result = new OrderViewModel() { Id = order.Id, TotalCost = order.TotalCost, UserId = order.UserId };
        return new OkObjectResult(result);
    }

    // GET api/Orders/5/History
    [HttpGet("{id}/History")]
    public async Task<IActionResult> GetOrderHistory([FromRoute] int id)
    {
        var history = await _orderService.GetOrderHistory(id);
        var result = new List<OrderHistoryViewModel>();
        foreach (var item in history)
        {
            result.Add(new OrderHistoryViewModel { OrderId = item.OrderId, StatusId = item.StatusId, StatusName = item.Status.Name, SetTime = item.SetTime });
        }
        return new OkObjectResult(result);
    }

    // POST api/Orders/5/History/3
    [HttpPost("{id}/History/{statusId}")]
    public async Task<IActionResult> AddStatusToOrder([FromRoute] int id, [FromRoute] int statusId)
    {
        await _orderService.AddOrderStatus(id, statusId);
        return new OkResult();
    }

    // GET api/Orders/5/Products
    [HttpGet("{id}/Products")]
    public async Task<IActionResult> GetOrderProducts([FromRoute] int id)
    {
        var orderProducts = await _orderService.GetOrderProducts(id);
        var result = new List<OrderProductViewModel>();
        foreach (var orderProduct in orderProducts)
        {
            result.Add(new OrderProductViewModel { OrderId = orderProduct.OrderId, ProductId = orderProduct.ProductId, Count = orderProduct.Count });
        }
        return new OkObjectResult(result);
    }

    // GET api/Orders/User/5
    [HttpGet("Users/{userId}")]
    public async Task<IActionResult> GetOrders([FromRoute] int userId)
    {
        var allOrders = await _orderService.GetOrders(userId);

        var result = new List<OrderViewModel>();
        foreach (var order in allOrders)
        {
            result.Add(new OrderViewModel { Id = order.Id, TotalCost = order.TotalCost, UserId = order.UserId });
        }

        return new OkObjectResult(result);
    }

    // POST api/Orders/User/5
    [HttpPost("Users/{userId}")]
    public async Task<IActionResult> AddOrder([FromRoute] int userId, [FromBody] IEnumerable<OrderProductInputModel> orderProducts)
    {
        var productsToAdd = new List<OrderProduct>();
        foreach (var orderProduct in orderProducts)
        {
            productsToAdd.Add(new OrderProduct() { Count = orderProduct.Count, ProductId = orderProduct.ProductId });
        }

        await _orderService.AddOrder(userId, productsToAdd);

        return new OkResult();
    }
}
