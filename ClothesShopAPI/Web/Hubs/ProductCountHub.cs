using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs.ClientInterfaces;
using Web.Models.ViewModels;

namespace Web.Hubs;

public class ProductCountHub : Hub<IProductCountClient>
{
    public async Task JoinProductGroup(int productId, [FromServices] IProductService productService)
    {
        var product = await productService.GetProduct(productId);
        await Groups.AddToGroupAsync(Context.ConnectionId, productId.ToString());        
        await Clients.Caller.GetProductCount(new ProductCountViewModel()
        {
            AvailableQuantity = product.AvailableQuantity,
            ReservedQuantity = product.ReservedQuantity,
            ProductId = productId
        });
    }

    public async Task LeaveProductGroup(int productId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, productId.ToString());
    }
}
