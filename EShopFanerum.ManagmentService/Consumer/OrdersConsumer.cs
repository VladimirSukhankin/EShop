using EShopFanerum.Core.RabbitMQ.Dto;
using MassTransit;

namespace EShopFanerum.ManagmentService.Consumer;

public class OrdersConsumer : IConsumer<OrderDto>
{
    public async Task Consume(ConsumeContext<OrderDto> context)
    {
        Console.WriteLine(context.Message.ProductName);
        await Task.Run(() => 1);
    }
}