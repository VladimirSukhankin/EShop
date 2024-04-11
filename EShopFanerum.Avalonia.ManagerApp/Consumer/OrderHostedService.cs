using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace EShopFanerum.Avalonia.ManagerApp.Consumer;

public class OrderHostedService : BackgroundService
{
    private readonly IConsumerService _consumerService;

    public OrderHostedService(IConsumerService consumerService)
    {
        _consumerService = consumerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumerService.ReadMessages();
    }
}