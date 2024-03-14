using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EShopFanerum.Avalonia.ManagerApp.Domain;
using EShopFanerum.Avalonia.ManagerApp.Persistence;
using EShopFanerum.Core.RabbitMQ.Dto;
using EShopFanerum.Domain.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Splat;

namespace EShopFanerum.Avalonia.ManagerApp.Consumer;

public class OrdersConsumer : IConsumer<OrderDto>
{
    private readonly ManagerDbContext _managerDbContext;
    
    public OrdersConsumer()
    {
        var serviceProvider = (IServiceProvider) Locator.Current.GetService(typeof(IServiceProvider))!;
        _managerDbContext = serviceProvider.GetRequiredService<ManagerDbContext>();
    }
    public async Task Consume(ConsumeContext<OrderDto> context)
    {
        var order = context.Message;
        Console.WriteLine($"Received order: {order.Guid} - {order.Price}");

        _managerDbContext.Orders.Add(new Order()
        {
            Guid = order.Guid,
            StateOrder = StateOrder.New,
            CreatedDateTime = DateTime.UtcNow,
            Price = order.Price,
            GoodIds = order.GoodsIds.ToArray()
        });

        
        //TODO: CANCELL TOKEN
        await _managerDbContext.SaveChangesAsync(CancellationToken.None);
    }
}