using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Web.Models.ViewModels;
using Application.Interfaces;
using Web.Hubs.ClientInterfaces;
using Web.Extension;
using Domain.Entities;

namespace Web.Hubs;

public class ProductViewersCountHub : Hub<IProductViewersCountClient>
{
    private readonly IViewersCountService _viewersCountService;

    public ProductViewersCountHub(IViewersCountService viewersCountService)
    {
        _viewersCountService = viewersCountService;
    }

    public async Task JoinProductGroup(int productId)
    {        
        if(Context.User.Identity.IsAuthenticated)
        {
            var userId = Context.User.GetUserId();
            await _viewersCountService.AddWatchingUser(userId, productId);
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, productId.ToString());
        var viewersCount = await _viewersCountService.GetViewersCount(productId);
        await Clients.Group(productId.ToString()).GetViewersCount(new ProductViewersCountViewModel()
        {
            ViewersCount = viewersCount,
            ProductId = productId
        });        
    }
    public async Task LeaveProductGroup(int productId)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            var userId = Context.User.GetUserId();
            await _viewersCountService.RemoveWatchingUser(userId, productId);
        }
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, productId.ToString());

        var viewersCount = await _viewersCountService.GetViewersCount(productId);
        await Clients.Group(productId.ToString()).GetViewersCount(new ProductViewersCountViewModel()
        {
            ViewersCount = viewersCount,
            ProductId = productId
        });
    }
}
