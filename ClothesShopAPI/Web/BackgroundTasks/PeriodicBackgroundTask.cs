using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Web.BackgroundTasks;

public class PeriodicBackgroundTask : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromSeconds(60);
    private readonly IServiceScopeFactory _scopeFactory;
    public PeriodicBackgroundTask(IServiceScopeFactory serviceScopeFactory)
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
            using (ShopDbContext context = scope.ServiceProvider.GetService<ShopDbContext>())
            {
                Console.WriteLine(context.Users.First().ToString());
            }
            
        }
    }
}