using EShopFanerum.Service.BusService.Interfaces;
using MassTransit;

namespace EShopFanerum.Service.BusService.Implementations;

public class MassTransitSenderService : ISenderService
{
    private readonly IBus _bus;

    public MassTransitSenderService(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendMessage<T>(T message) where T : class
    {
        await _bus.Publish(message);
    }
}