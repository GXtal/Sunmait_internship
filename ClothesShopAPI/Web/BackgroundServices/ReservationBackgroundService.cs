using Application.Interfaces;

namespace Web.BackgroundServices;

public class ReservationBackgroundService : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromSeconds(10);
    private readonly IServiceScopeFactory _scopeFactory;
    public ReservationBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _scopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);
        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            var reservationService = scope.ServiceProvider.GetService<IReservationService>();
            var modifiedProductsIds = await reservationService.DeleteExpiredReservations();
            Console.WriteLine(modifiedProductsIds.Count());
        }
    }
}