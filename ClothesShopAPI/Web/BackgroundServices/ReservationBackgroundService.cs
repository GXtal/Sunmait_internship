using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs.ClientInterfaces;
using Web.Hubs;
using Domain.Entities;
using Web.Models.ViewModels;

namespace Web.BackgroundServices;

public class ReservationBackgroundService : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromSeconds(10);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<ProductCountHub, IProductCountClient> _hubContext;
    public ReservationBackgroundService(IServiceScopeFactory serviceScopeFactory, IHubContext<ProductCountHub, IProductCountClient> hubContext)
    {
        _scopeFactory = serviceScopeFactory;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);
        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            var reservationService = scope.ServiceProvider.GetService<IReservationService>();
            var modifiedProducts = await reservationService.DeleteExpiredReservations();
            foreach (var product in modifiedProducts)
            {
                _hubContext.Clients.Group(product.Id.ToString()).GetProductCount(new ProductCountViewModel
                {
                    ProductId = product.Id,
                    AvailableQuantity = product.AvailableQuantity,
                    ReservedQuantity = product.ReservedQuantity
                });
            }
        }
    }
}