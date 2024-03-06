using EShopFanerum.Core.RabbitMQ.Dto;
using MassTransit;

namespace EShopFanerum.Infrastructure.Services.Impl;

public class SupportService : ISupportService
{
    private readonly IBus _bus;
    
    public SupportService(IBus bus)
    {
        _bus = bus;
    }
    public async Task SendMessageToSupport(SupportMessageDto supportMessageDto, CancellationToken cancellationToken)
    {
        await _bus.Publish(supportMessageDto, cancellationToken);
    }
}