namespace EShopFanerum.Service.BusService.Interfaces;

public interface ISenderService
{
    Task SendMessage<T>(T message) where T : class;
}