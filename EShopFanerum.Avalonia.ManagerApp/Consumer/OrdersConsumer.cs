using System;
using System.Threading.Tasks;
using EShopFanerum.Core.RabbitMQ.Dto;
using MassTransit;

namespace EShopFanerum.Avalonia.ManagerApp.Consumer;

public class OrdersConsumer : IConsumer<OrderDto>
{
    public Task Consume(ConsumeContext<OrderDto> context)
    {
        var order = context.Message;
        Console.WriteLine($"Received order: {order.Count} - {order.ProductName} - {order.Price}");
        
        // Добавьте здесь логику обработки полученного заказа
        
        return Task.CompletedTask;
    }
}