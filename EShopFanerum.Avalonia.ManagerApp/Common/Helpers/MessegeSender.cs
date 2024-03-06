using System.Threading.Tasks;
using MassTransit;

namespace EShopFanerum.Avalonia.ManagerApp.Common.Helpers;

public class MessegeSender : IMessageSender
{
    private readonly IBus _bus;
    
    public MessegeSender(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendMessage<T>(T message)
    {
        await _bus.Publish(message);
    }
}