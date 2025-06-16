using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace EShopFanerum.Avalonia.ManagerApp.Consumer;

public class OrderHostedService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}