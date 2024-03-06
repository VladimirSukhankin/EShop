using System;
using System.Threading.Tasks;
using EShopFanerum.Core.RabbitMQ.Dto;
using MassTransit;

namespace EShopFanerum.Avalonia.ManagerApp.Consumer;

public class SupportMessageConsumer : IConsumer<SupportMessageDto>
{
    public async Task Consume(ConsumeContext<SupportMessageDto> context)
    {
        Console.WriteLine(context.Message.DescriptionError);
        await Task.Run(() => 1);
    }
}